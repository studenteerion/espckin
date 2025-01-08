const exp = require("express");
const path = require("path");
const fs = require("fs");
const bodyParser = require('body-parser');
const mysql = require('mysql2');

var app = exp(); //app for module express usage

let hostname = '127.0.0.1'; //hostname declaration to determine listening site of the server
const port = 9898; //port declaration to determine listening site of the server

const hostSQL = 'localhost';//hostname declaration for SQL
const userSQL = 'root';//user declaration for SQL
const databaseSQL = 'library';//database declaration for SQL
const passwordSQL = '';//password declaration for SQL

app.use(exp.static(path.join(__dirname, "static")));//initialize static folder to handle the Homepage

//load homepage
function loadHomepage(req,res){
    res.setHeader('Content-Type', 'text/html; charset=utf-8');
    res.status(200);
    
    const filepath = path.join(__dirname, "static", "Homepage.html");
    res.write(fs.readFileSync(filepath, "utf-8"));

    res.end();
}

//sends all the dataset
function getAll(req,res){
    res.setHeader('Content-Type', 'application/json; charset=utf-8');
    res.status(200);
    

    res.end();
}

//sends the teacher based on the plate
function getTeachers(req,res){
    res.setHeader('Content-Type', 'application/json; charset=utf-8');
    res.status(200);
    

    res.end();
}

//sends the palte based on the teacher
function getPlates(req,res){
    res.setHeader('Content-Type', 'application/json; charset=utf-8');
    res.status(200);
    

    res.end();
}

//sends all the dataset
function getEntry(req,res){
    res.setHeader('Content-Type', 'application/json; charset=utf-8');
    res.status(200);
    

    res.end();
}

app.get("/", (req,res) => loadHomepage(req, res)); //shows the docs
app.get("/all", (req,res) => getAll(req, res)); //answer the complete dataset
app.get("/plate/:plate", (req,res) => getTeachers(req, res)); //given the plates answer with the teacher infos
app.get("/teacers/:teachers", (req,res) => getPlates(req, res)); //given the teacher answer with the car infos
app.get("/entry/:entry", (req,res) => getEntry(req, res)); //get teacher based on their entry
/*app.get("/", (req,res) => loadHomepage(req, res));
app.get("/", (req,res) => loadHomepage(req, res));
app.get("/", (req,res) => loadHomepage(req, res));*/

var server = app.listen(port, hostname, () => {
    console.log(`Server online \nEndpoint: http://${hostname}:${port}`);
});