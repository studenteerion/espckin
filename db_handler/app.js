const exp = require("express");
const path = require("path");
const fs = require("fs");
const bodyParser = require('body-parser');
const mysql = require('mysql2');
const cors= require('cors');

const app = exp();
const hostname = '127.0.0.1';
const port = 9898;
app.use(cors());

const db = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: '',
    database: 'espckin'
});

db.connect(err => {
    if (err) throw err;
    console.log("Connected to MySQL database.");
});

app.use(bodyParser.json());
app.use(exp.static(path.join(__dirname, "static")));//initialize static folder to handle the Homepage

//load homepage
function loadHomepage(req, res) {
    res.setHeader('Content-Type', 'text/html; charset=utf-8');
    res.status(200);

    const filepath = path.join(__dirname, "static", "Homepage.html");
    res.write(fs.readFileSync(filepath, "utf-8"));

    res.end();
}

//sends all the dataset
function getAll(req, res) {
    res.setHeader('Content-Type', 'application/json; charset=utf-8');
    res.status(200);

    db.query(`SELECT professore.id_professore,professore.nome,professore.cognome,professore.mail,macchine.targa,accesso.zona_accesso 
        FROM macchina_professore 
        JOIN professore on macchina_professore.id_professore = professore.id_professore 
        JOIN macchine on macchina_professore.id_macchina = macchine.targa 
        JOIN accesso on professore.id_zona = accesso.id_accesso`, (err, results) => {
        if (err) {
            res.status(500).send({ error: err.message });
        } else {
            res.status(200).send(results);
        }
    });
}

//sends the teacher based on the plate
function getTeachers(req, res) {
    const plate = req.params.plate;
    res.setHeader('Content-Type', 'application/json; charset=utf-8');

    db.query(
        `SELECT professore.id_professore,professore.nome,professore.cognome,professore.mail,macchine.targa,accesso.zona_accesso 
        FROM macchina_professore 
        JOIN professore on macchina_professore.id_professore = professore.id_professore 
        JOIN macchine on macchina_professore.id_macchina = macchine.targa 
        JOIN accesso on professore.id_zona = accesso.id_accesso
        WHERE macchine.targa=?`,
        [plate],
        (err, results) => {
            if (err) {
                res.status(500).send({ error: err.message });
            } else {
                res.status(200).send(results);
            }
        }
    );
}

//sends the plate based on the teacher
function getPlates(req, res) {
    const teacher = req.params.teachers;
    res.setHeader('Content-Type', 'application/json; charset=utf-8');

    db.query(
        `SELECT professore.id_professore,professore.nome,professore.cognome,professore.mail,macchine.targa,accesso.zona_accesso 
        FROM macchina_professore 
        JOIN professore on macchina_professore.id_professore = professore.id_professore 
        JOIN macchine on macchina_professore.id_macchina = macchine.targa 
        JOIN accesso on professore.id_zona = accesso.id_accesso
        WHERE professore.id_professore=?`,
        [teacher],
        (err, results) => {
            if (err) {
                res.status(500).send({ error: err.message });
            } else {
                res.status(200).send(results);
            }
        }
    );
}

//sends all the dataset
function getEntry(req, res) {
    const entry = req.params.entry;
    res.setHeader('Content-Type', 'application/json; charset=utf-8');

    db.query(
        `SELECT professore.id_professore,professore.nome,professore.cognome,professore.mail,accesso.zona_accesso 
        FROM professore 
        JOIN accesso on professore.id_zona = accesso.id_accesso
        WHERE accesso.zona_accesso=?`,
        [entry],
        (err, results) => {
            if (err) {
                res.status(500).send({ error: err.message });
            } else {
                res.status(200).send(results);
            }
        }
    );
}

// CRUD Endpoints
// Create a record in a table
app.post("/create/:table", (req, res) => {
    const table = req.params.table;
    const data = req.body;

    db.query(`INSERT INTO ?? SET ?`, [table, data], (err, result) => {
        if (err) {
            res.status(500).send({ error: err.message });
        } else {
            res.status(201).send({ message: "Record created successfully", id: result.insertId });
        }
    });
});

// Read all records from a table
app.get("/read/:table", (req, res) => {
    const table = req.params.table;

    db.query(`SELECT * FROM ??`, [table], (err, results) => {
        if (err) {
            res.status(500).send({ error: err.message });
        } else {
            res.status(200).send(results);
        }
    });
});

// Update a record in a table
app.put("/update/:table/:id", (req, res) => {
    const table = req.params.table;
    const id = req.params.id;
    const data = req.body;

    db.query(`UPDATE ?? SET ? WHERE id = ?`, [table, data, id], (err, result) => {
        if (err) {
            res.status(500).send({ error: err.message });
        } else if (result.affectedRows === 0) {
            res.status(404).send({ message: "Record not found" });
        } else {
            res.status(200).send({ message: "Record updated successfully" });
        }
    });
});

// Delete a record from a table
app.delete("/delete/:table/:id", (req, res) => {
    const table = req.params.table;
    const id = req.params.id;

    db.query(`DELETE FROM ?? WHERE id = ?`, [table, id], (err, result) => {
        if (err) {
            res.status(500).send({ error: err.message });
        } else if (result.affectedRows === 0) {
            res.status(404).send({ message: "Record not found" });
        } else {
            res.status(200).send({ message: "Record deleted successfully" });
        }
    });
});

// Define endpoints
app.get("/", (req, res) => loadHomepage(req, res)); //shows the docs
app.get("/all", (req, res) => getAll(req, res)); //answer the complete dataset
app.get("/plate/:plate", (req, res) => getTeachers(req, res)); //given the plates answer with the teacher infos
app.get("/teacers/:teachers", (req, res) => getPlates(req, res)); //given the teacher answer with the car infos
app.get("/entry/:entry", (req, res) => getEntry(req, res)); //get teacher based on their entry

app.listen(port, hostname, () => {
    console.log(`Server running at http://${hostname}:${port}/`);
});