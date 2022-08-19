//Server for the API calls for DB
// The only security in this, is that noone is touch the DB, everything has to go through a API Call
var express = require('express'),
app = express(),
port = 3000,
mongoose = require('mongoose'),
Task = require('./api/models/model'),
bodyParser = require('body-parser');

//Make use of CORS
var cors = require('cors');

app.use(cors());


var url = "mongodb://localhost:27017/robo";

//Intialize mongo
mongoose.Promise = global.Promise;
mongoose.connect(url, { useNewUrlParser: true });
//Deprecated Option
mongoose.set('useFindAndModify', false);

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

//Sets the route
var routes = require('./api/routes/routes');
routes(app);


app.listen(port);

console.log("Server started on port " + port);

//Third Party Library, may not be best options tbh
var XMLHttpRequest = require("xhr2").XMLHttpRequest;

//Here you can set how often the MLData is updated
updateMLData();
getMLData();
setInterval(updateMLData, 6000);
runServerML();

//This is the function that calls the server MLDataUpdate function
function updateMLData() {
    console.log("Machine Learning Data Updated");
    var request = new XMLHttpRequest();

    //This Line needs to be changed when we go to Production
    request.open('GET', "http://localhost:3000/logs/tryingJS")
    request.send();
}

//This function won't be used, here for debugging
function getMLData(){
    console.log("Machine Learning Data Gotten");
    var request = new XMLHttpRequest();

    //This Line needs to be changed when we go to Production
    request.open('GET', "http://localhost:3000/logs/ml")
    request.send();
    request.onload = ()=>{
        //console.log(JSON.parse(request.response));
    }
}

function runServerML(){
    console.log("Machine Learning model being updated");
    var request = new XMLHttpRequest();

    //This Line needs to be changed when we go to Production
    request.open('GET', "http://localhost:3000/ml/level1a.xml/50000/0")
    request.send();
    request.onload = ()=>{
        console.log("Machine Learning done!");
    }
}