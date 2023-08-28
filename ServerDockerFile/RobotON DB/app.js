const express = require('express');
const createError = require('http-errors');
const dotenv = require('dotenv').config();

const app = express();

app.use(express.json());
app.use(express.urlencoded({ extended: true }));

var cors = require('cors')
app.use(cors())

// Initialize DB
require('./initDB')();

const ProductRoute = require('./Routes/Route');
app.use('/logsBUG', ProductRoute);

//404 handler and pass to error handler
app.use((req, res, next) => {
  next(createError(404, 'Cannot Find'));
});

//Error handler
app.use((err, req, res, next) => {
  res.status(err.status || 500);
  res.send({
    error: {
      status: err.status || 500,
      message: err.message
    }
  });
});

const PORT = process.env.PORT || 3000;

app.listen(PORT, () => {
  console.log('Server started on port ' + PORT + '...');
});

//Third Party Library to make API requests, may not be best options tbh
var XMLHttpRequest = require("xhr2").XMLHttpRequest;

//Make intervals for the machine learning to be called
setInterval(updateMLData,20000)
setInterval(runKMeans,60000)

//This is the function that calls the server MLDataUpdate function
function updateMLData() {
  console.log("Machine Learning Data Updated");
  var request = new XMLHttpRequest();

  //This Line needs to be changed when we go to Production
  request.open('GET', "https://robogames.science.uoit.ca/logsBUG/tryingJS")
  request.send();
}

//Runs the K Means Machine Learning for each level
function runKMeans(){
  console.log("Running the KMeans ML")
  var levels = ["level1a.xml","level1b.xml","level1.xml" ,"level2a.xml" ,"level2b.xml" ,"level2.xml" ,"level3a.xml" ,"level3b.xml" ,"level3.xml" ,"level4a.xml" ,"level4b.xml" ,"level4c.xml" ,"level4.xml"]

  for (let i = 0; i < levels.length; i++) {
      
      var request = new XMLHttpRequest();

      //This Line needs to be changed when we go to Production
      request.open('GET', "https://robogames.science.uoit.ca/logsBUG/ml/callAlgo/"+levels[i])
      request.send();
  }

}