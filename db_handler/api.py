from flask import Flask, request, jsonify, send_from_directory
import mysql.connector
import os
import json
import savelog as log

HOST='0.0.0.0'
PORT=9898

app = Flask(__name__)

ALLOWED_USERS = [{"user":"frontend","key":"BJFLMJTARU"},{"user":"prof","key":"VYPPMADJXD"}]
print(ALLOWED_USERS)

# Database connection
conn = mysql.connector.connect(
    host="localhost",
    user="root",
    password="",
    database="espckin"
)

cursor = conn.cursor(dictionary=True)

def auth(route, key):
    if not any(obj.get("key") == key for obj in ALLOWED_USERS):
        log.savelog_errauth(route)
        return False
    user = [obj for obj in ALLOWED_USERS if obj.get("key") == key]
    log.savelog_access(json.dumps(user[0]),route)
    return True

# Serve static files
@app.route('/')
def serve_homepage():
    key = request.args.get('key')
    if not auth("homepage", key):
        return ""
    return send_from_directory("static", "Homepage.html")

# Get all data
@app.route('/all', methods=['GET'])
def get_all():
    key = request.args.get('key')
    if not auth("all", key):
        return ""
    query = '''SELECT professore.id_professore, professore.nome, professore.cognome, professore.mail,
                      macchine.targa, accesso.zona_accesso 
               FROM macchina_professore 
               JOIN professore ON macchina_professore.id_professore = professore.id_professore 
               JOIN macchine ON macchina_professore.id_macchina = macchine.targa 
               JOIN accesso ON professore.id_zona = accesso.id_accesso'''
    cursor.execute(query)
    results = cursor.fetchall()
    return jsonify(results)

# Get teacher by plate
@app.route('/plate/<string:plate>', methods=['GET'])
def get_teachers(plate):
    key = request.args.get('key')
    if not auth("plate", key):
        return ""
    query = '''SELECT professore.id_professore, professore.nome, professore.cognome, professore.mail,
                      macchine.targa, accesso.zona_accesso 
               FROM macchina_professore 
               JOIN professore ON macchina_professore.id_professore = professore.id_professore 
               JOIN macchine ON macchina_professore.id_macchina = macchine.targa 
               JOIN accesso ON professore.id_zona = accesso.id_accesso
               WHERE macchine.targa=?'''
    cursor.execute(query, (plate,))
    results = cursor.fetchall()
    return jsonify(results)

# Get plates by teacher
@app.route('/teachers/<string:teacher>', methods=['GET'])
def get_plates(teacher):
    key = request.args.get('key')
    if not auth("teachers", key):
        return ""
    query = '''SELECT professore.id_professore, professore.nome, professore.cognome, professore.mail,
                      macchine.targa, accesso.zona_accesso 
               FROM macchina_professore 
               JOIN professore ON macchina_professore.id_professore = professore.id_professore 
               JOIN macchine ON macchina_professore.id_macchina = macchine.targa 
               JOIN accesso ON professore.id_zona = accesso.id_accesso
               WHERE professore.id_professore=%s'''
    cursor.execute(query, (teacher,))
    results = cursor.fetchall()
    return jsonify(results)

# Get entry-based records
@app.route('/entry/<string:entry>', methods=['GET'])
def get_entry(entry):
    key = request.args.get('key')
    if not auth("entry", key):
        return ""
    query = '''SELECT professore.id_professore, professore.nome, professore.cognome, professore.mail,
                      accesso.zona_accesso 
               FROM professore 
               JOIN accesso ON professore.id_zona = accesso.id_accesso
               WHERE accesso.zona_accesso=%s'''
    cursor.execute(query, (entry,))
    results = cursor.fetchall()
    return jsonify(results)

# CRUD Operations
@app.route('/create/<string:table>', methods=['POST'])
def create_record(table):
    key = request.args.get('key')
    if not auth("create", key):
        return ""
    data = request.json
    columns = ', '.join(data.keys())
    values = ', '.join(['%s'] * len(data))
    query = f"INSERT INTO {table} ({columns}) VALUES ({values})"
    cursor.execute(query, tuple(data.values()))
    conn.commit()
    return jsonify({"message": "Record created successfully", "id": cursor.lastrowid}), 201

@app.route('/read/<string:table>', methods=['GET'])
def read_records(table):
    key = request.args.get('key')
    if not auth("read", key):
        return ""
    query = f"SELECT * FROM {table}"
    cursor.execute(query)
    results = cursor.fetchall()
    return jsonify(results)

@app.route('/update/<string:table>/<int:id>', methods=['PUT'])
def update_record(table, id):
    key = request.args.get('key')
    if not auth("update", key):
        return ""
    data = request.json
    set_clause = ', '.join([f"{key}=%s" for key in data.keys()])
    query = f"UPDATE {table} SET {set_clause} WHERE id=%s"
    cursor.execute(query, tuple(data.values()) + (id,))
    conn.commit()
    return jsonify({"message": "Record updated successfully"}) if cursor.rowcount else jsonify({"message": "Record not found"}), 404

@app.route('/delete/<string:table>/<int:id>', methods=['DELETE'])
def delete_record(table, id):
    key = request.args.get('key')
    if not auth("delete", key):
        return ""
    query = f"DELETE FROM {table} WHERE id=%s"
    cursor.execute(query, (id,))
    conn.commit()
    return jsonify({"message": "Record deleted successfully"}) if cursor.rowcount else jsonify({"message": "Record not found"}), 404

if __name__ == '__main__':
    app.run(host=HOST, port=PORT)