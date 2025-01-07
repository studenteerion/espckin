const exp = require("express");
const path = require("path");
const fs = require("fs");

var app = exp();

let hostname = '127.0.0.1';
const port = 9898;

app.use(exp.static(path.join(__dirname, "static")));//inizializzazione cartella static per gestione Homepage

//load homepage
function loadHomepage(req,res){
    res.setHeader('Content-Type', 'text/html; charset=utf-8');
    res.status(200);
    
    const filepath = path.join(__dirname, "static", "Homepage.html");
    res.write(fs.readFileSync(filepath, "utf-8"));

    res.end();
}

function getAll(req,res){
    res.setHeader('Content-Type', 'application/json; charset=utf-8');
    res.status(200);
    

    res.end();
}

function getTeachers(req,res){
    res.setHeader('Content-Type', 'application/json; charset=utf-8');
    res.status(200);
    

    res.end();
}

function getPlates(req,res){
    res.setHeader('Content-Type', 'application/json; charset=utf-8');
    res.status(200);
    

    res.end();
}

function getEntry(req,res){
    res.setHeader('Content-Type', 'application/json; charset=utf-8');
    res.status(200);
    

    res.end();
}

app.get("/", (req,res) => loadHomepage(req, res)); //mostra la documentazione
app.get("/all", (req,res) => getAll(req, res)); //restituisce intero dataset
app.get("/plate/:plate", (req,res) => getTeachers(req, res)); //data la targa restituisce informazioni docente proprietario
app.get("/teacers/:teachers", (req,res) => getPlates(req, res)); //dato il docente restituisce le macchine associate
app.get("/entry/:entry", (req,res) => getEntry(req, res)); //get form entry
/*app.get("/", (req,res) => loadHomepage(req, res));
app.get("/", (req,res) => loadHomepage(req, res));
app.get("/", (req,res) => loadHomepage(req, res));*/

var server = app.listen(port, hostname, () => {
    console.log(`Server online \nEndpoint: http://${hostname}:${port}`);
});