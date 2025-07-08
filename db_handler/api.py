from flask import Flask, request, jsonify, send_from_directory, abort
import mysql.connector
import json
import os
import logging
from dotenv import load_dotenv
from flasgger import Swagger

# ---------- SETUP .env ----------
ENV_FILE = ".env"
DEFAULTS = {
    "HOST": "0.0.0.0",
    "PORT": "9898"
}

# Carica variabili da .env
load_dotenv(ENV_FILE)
HOST = os.getenv("HOST", DEFAULTS["HOST"])
PORT = int(os.getenv("PORT", DEFAULTS["PORT"]))

# ---------- FLASK APP ----------
app = Flask(__name__)
swagger = Swagger(app)

# Logger setup
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s [%(levelname)s] %(message)s',
    handlers=[logging.StreamHandler()]
)

# DB connection    TODO: creare sottoutenti per limitare l'accesso al database !!utilizzare le view!!
conn = mysql.connector.connect(
    host="localhost",
    user="root",
    password="",
    database="espckin"
)
cursor = conn.cursor(dictionary=True)

# ---------- AUTH ----------
def authenticate(route_name, api_key):
    cursor.execute("SELECT * FROM utenti WHERE api_key = %s", (api_key,))
    user = cursor.fetchone()
    if not user:
        logging.warning(f"Unauthorized access attempt to '{route_name}' with key '{api_key}'")
        abort(401, description="Unauthorized")
    logging.info(f"Access granted: user='{user['username']}' route='{route_name}'")
    return True


@app.route('/')
def homepage():
    """Serve homepage static
    ---
    parameters:
      - name: key
        in: query
        type: string
        required: true
    responses:
      200:
        description: HTML Page
    """
    authenticate("homepage", request.args.get('key'))
    return send_from_directory("static", "Homepage.html")


@app.route('/api/records', methods=['GET'])
def get_all_records():
    """Get all records
    ---
    parameters:
      - name: key
        in: query
        type: string
        required: true
    responses:
      200:
        description: List of professors and cars
        schema:
          type: array
          items:
            type: object
    """
    authenticate("all", request.args.get('key'))
    query = '''
        SELECT p.id_professore, p.nome, p.cognome, p.mail,
               m.targa, a.zona_accesso
        FROM macchina_professore mp
        JOIN professore p ON mp.id_professore = p.id_professore
        JOIN macchine m ON mp.id_macchina = m.targa
        JOIN accesso a ON p.id_zona = a.id_accesso
    '''
    cursor.execute(query)
    return jsonify(cursor.fetchall())


@app.route('/api/plate/<string:plate>', methods=['GET'])
def get_by_plate(plate):
    """Get teacher by plate
    ---
    parameters:
      - name: key
        in: query
        type: string
        required: true
      - name: plate
        in: path
        type: string
        required: true
    responses:
      200:
        description: Professor info
    """
    authenticate("plate", request.args.get('key'))
    query = '''
        SELECT p.id_professore, p.nome, p.cognome, p.mail,
               m.targa, a.zona_accesso
        FROM macchina_professore mp
        JOIN professore p ON mp.id_professore = p.id_professore
        JOIN macchine m ON mp.id_macchina = m.targa
        JOIN accesso a ON p.id_zona = a.id_accesso
        WHERE m.targa = %s
    '''
    cursor.execute(query, (plate,))
    return jsonify(cursor.fetchall())


@app.route('/api/teacher/<string:teacher_id>', methods=['GET'])
def get_by_teacher(teacher_id):
    """Get plates by teacher ID
    ---
    parameters:
      - name: key
        in: query
        type: string
        required: true
      - name: teacher_id
        in: path
        type: string
        required: true
    responses:
      200:
        description: Plates associated with a professor
    """
    authenticate("teachers", request.args.get('key'))
    query = '''
        SELECT p.id_professore, p.nome, p.cognome, p.mail,
               m.targa, a.zona_accesso
        FROM macchina_professore mp
        JOIN professore p ON mp.id_professore = p.id_professore
        JOIN macchine m ON mp.id_macchina = m.targa
        JOIN accesso a ON p.id_zona = a.id_accesso
        WHERE p.id_professore = %s
    '''
    cursor.execute(query, (teacher_id,))
    return jsonify(cursor.fetchall())


@app.route('/api/entry/<string:zone>', methods=['GET'])
def get_by_entry(zone):
    """Get teachers by access zone
    ---
    parameters:
      - name: key
        in: query
        type: string
        required: true
      - name: zone
        in: path
        type: string
        required: true
    responses:
      200:
        description: Professors in a specific zone
    """
    authenticate("entry", request.args.get('key'))
    query = '''
        SELECT p.id_professore, p.nome, p.cognome, p.mail,
               a.zona_accesso
        FROM professore p
        JOIN accesso a ON p.id_zona = a.id_accesso
        WHERE a.zona_accesso = %s
    '''
    cursor.execute(query, (zone,))
    return jsonify(cursor.fetchall())


# ---------- CRUD ----------
@app.route('/api/<string:table>', methods=['POST'])
def create_record(table):
    """Create new record in specified table
    ---
    parameters:
      - name: key
        in: query
        type: string
        required: true
      - name: body
        in: body
        required: true
        schema:
          type: object
    responses:
      201:
        description: Record created
    """
    authenticate("create", request.args.get('key'))
    data = request.json
    columns = ', '.join(data.keys())
    placeholders = ', '.join(['%s'] * len(data))
    query = f"INSERT INTO {table} ({columns}) VALUES ({placeholders})"
    cursor.execute(query, tuple(data.values()))
    conn.commit()
    logging.info(f"Record created in '{table}' with ID {cursor.lastrowid}")
    return jsonify({"message": "Created", "id": cursor.lastrowid}), 201


@app.route('/api/<string:table>', methods=['GET'])
def read_table(table):
    """Read all records from table
    ---
    parameters:
      - name: key
        in: query
        type: string
        required: true
    responses:
      200:
        description: Table data
    """
    authenticate("read", request.args.get('key'))
    query = f"SELECT * FROM {table}"
    cursor.execute(query)
    return jsonify(cursor.fetchall())


@app.route('/api/<string:table>/<int:record_id>', methods=['PUT'])
def update_record(table, record_id):
    """Update record by ID in specified table
    ---
    parameters:
      - name: key
        in: query
        type: string
        required: true
      - name: record_id
        in: path
        type: integer
        required: true
      - name: body
        in: body
        required: true
        schema:
          type: object
    responses:
      200:
        description: Updated
      404:
        description: Not found
    """
    authenticate("update", request.args.get('key'))
    data = request.json
    set_clause = ', '.join([f"{k} = %s" for k in data.keys()])
    query = f"UPDATE {table} SET {set_clause} WHERE id = %s"
    cursor.execute(query, tuple(data.values()) + (record_id,))
    conn.commit()
    if cursor.rowcount:
        logging.info(f"Record updated in '{table}' ID {record_id}")
        return jsonify({"message": "Updated"})
    return jsonify({"message": "Not found"}), 404


@app.route('/api/<string:table>/<int:record_id>', methods=['DELETE'])
def delete_record(table, record_id):
    """Delete record by ID from table
    ---
    parameters:
      - name: key
        in: query
        type: string
        required: true
      - name: record_id
        in: path
        type: integer
        required: true
    responses:
      200:
        description: Deleted
      404:
        description: Not found
    """
    authenticate("delete", request.args.get('key'))
    query = f"DELETE FROM {table} WHERE id = %s"
    cursor.execute(query, (record_id,))
    conn.commit()
    if cursor.rowcount:
        logging.info(f"Record deleted from '{table}' ID {record_id}")
        return jsonify({"message": "Deleted"})
    return jsonify({"message": "Not found"}), 404

# utenti
@app.route('/api/utenti', methods=['GET'])
def get_utenti():
    authenticate("get_utenti", request.args.get('key'))
    cursor.execute("SELECT id, username, ruolo FROM utenti")
    return jsonify(cursor.fetchall())

@app.route('/api/utenti', methods=['POST'])
def create_utente():
    authenticate("create_utente", request.args.get('key'))
    data = request.json
    cursor.execute("INSERT INTO utenti (username, api_key, ruolo) VALUES (%s, %s, %s)", (data['username'], data['api_key'], data.get('ruolo', 'user')))
    conn.commit()
    return jsonify({"message": "Utente creato", "id": cursor.lastrowid})

@app.route('/api/utenti/<int:id>', methods=['PUT'])
def update_utente(id):
    authenticate("update_utente", request.args.get('key'))
    data = request.json
    cursor.execute("UPDATE utenti SET username=%s, api_key=%s, ruolo=%s WHERE id=%s", (data['username'], data['api_key'], data['ruolo'], id))
    conn.commit()
    return jsonify({"message": "Utente aggiornato"})

@app.route('/api/utenti/<int:id>', methods=['DELETE'])
def delete_utente(id):
    authenticate("delete_utente", request.args.get('key'))
    cursor.execute("DELETE FROM utenti WHERE id=%s", (id,))
    conn.commit()
    return jsonify({"message": "Utente eliminato"})

# accesso
@app.route('/api/accesso', methods=['GET'])
def get_accesso():
    authenticate("get_accesso", request.args.get('key'))
    cursor.execute("SELECT * FROM accesso")
    return jsonify(cursor.fetchall())

@app.route('/api/accesso', methods=['POST'])
def create_accesso():
    authenticate("create_accesso", request.args.get('key'))
    data = request.json
    cursor.execute("INSERT INTO accesso (id_accesso, zona_accesso, ip, descrizione, coordinates) VALUES (%s, %s, %s, %s, %s)", (
        data['id_accesso'], data['zona_accesso'], data.get('ip', ''), data.get('descrizione', ''), data.get('coordinates', '')
    ))
    conn.commit()
    return jsonify({"message": "Accesso creato"})

@app.route('/api/accesso/<int:id>', methods=['PUT'])
def update_accesso(id):
    authenticate("update_accesso", request.args.get('key'))
    data = request.json
    cursor.execute("UPDATE accesso SET zona_accesso=%s, ip=%s, descrizione=%s, coordinates=%s WHERE id_accesso=%s", (
        data['zona_accesso'], data['ip'], data['descrizione'], data['coordinates'], id
    ))
    conn.commit()
    return jsonify({"message": "Accesso aggiornato"})

@app.route('/api/accesso/<int:id>', methods=['DELETE'])
def delete_accesso(id):
    authenticate("delete_accesso", request.args.get('key'))
    cursor.execute("DELETE FROM accesso WHERE id_accesso=%s", (id,))
    conn.commit()
    return jsonify({"message": "Accesso eliminato"})

#macchine
@app.route('/api/macchine', methods=['GET'])
def get_macchine():
    authenticate("get_macchine", request.args.get('key'))
    cursor.execute("SELECT * FROM macchine")
    return jsonify(cursor.fetchall())

@app.route('/api/macchine', methods=['POST'])
def create_macchina():
    authenticate("create_macchina", request.args.get('key'))
    data = request.json
    cursor.execute("INSERT INTO macchine (targa) VALUES (%s)", (data['targa'],))
    conn.commit()
    return jsonify({"message": "Macchina creata"})

@app.route('/api/macchine/<string:targa>', methods=['DELETE'])
def delete_macchina(targa):
    authenticate("delete_macchina", request.args.get('key'))
    cursor.execute("DELETE FROM macchine WHERE targa=%s", (targa,))
    conn.commit()
    return jsonify({"message": "Macchina eliminata"})

#macchina professore
@app.route('/api/macchina_professore', methods=['GET'])
def get_macchina_prof():
    authenticate("get_macchina_prof", request.args.get('key'))
    cursor.execute("SELECT * FROM macchina_professore")
    return jsonify(cursor.fetchall())

@app.route('/api/macchina_professore', methods=['POST'])
def create_macchina_prof():
    authenticate("create_macchina_prof", request.args.get('key'))
    data = request.json
    cursor.execute("INSERT INTO macchina_professore (id_macchina, id_professore) VALUES (%s, %s)", (
        data['id_macchina'], data['id_professore']
    ))
    conn.commit()
    return jsonify({"message": "Associazione creata"})

@app.route('/api/macchina_professore', methods=['DELETE'])
def delete_macchina_prof():
    authenticate("delete_macchina_prof", request.args.get('key'))
    data = request.json
    cursor.execute("DELETE FROM macchina_professore WHERE id_macchina=%s AND id_professore=%s", (
        data['id_macchina'], data['id_professore']
    ))
    conn.commit()
    return jsonify({"message": "Associazione eliminata"})


# ---------- MAIN ----------
if __name__ == '__main__':
    app.run(host=HOST, port=PORT)
