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
mongoose.set('useFindAndModify', false);

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

//Sets the route
var routes = require('./api/routes/routes');
routes(app);


app.listen(port);

console.log("Server started on port " + port);

