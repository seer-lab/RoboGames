const createError = require('http-errors');

var TaskT = require('./../Models/Model.js');
var mongoose = require('mongoose'),
  TaskT = mongoose.model('RobotBug_Logs'),
  TaskC = mongoose.model('RobotBug_Course'),
  TaskM = mongoose.model('RobotBug_ML'),
  TaskCT = mongoose.model('RobotBug_Centroids');


//-------------------------------EXTERNAL FUNCTIONS------------------------------------------->
function onlyUnique(value, index, self){
  return self.indexOf(value) === index;
}

function sortJsonArrByPoints(a,b){
  return b.points - a.points;
}
//-------------------------------EOF EXTERNAL FUNCTIONS!--------------------------------------->

exports.list_all_logs_BUG = function(req,res){
  console.log("List all Logs bug");

  TaskC.find({}, function(err, task) {
      if (err)
        res.send(err);
      res.json(task);
    });
};

exports.create_a_log_BUG = function(req, res) {
  console.log("create a Logs bug");
  var new_task = new TaskT(req.body);
  new_task.save(function(err, task) {
    if (err)
      res.send(err);
    res.json(task);
    console.log(task);
  });
};


exports.read_a_log_BUG = function(req, res) {
  console.log("Read a Log bug");
  console.log(req.body)
  TaskT.findOne({name: req.params.sessionID}, function(err, task) {
    if (err)
      res.send(err);
    res.json(task);
    console.log(task)
  });
};


exports.update_a_log_BUG = function(req, res) {
  console.log("Update a log bug");
  console.log(req.body)
  TaskT.findOneAndUpdate({name: req.params.sessionID}, 
    {$push : {levels: req.body['levels']}}, 
    {new: true},
    function(err, task) {
    if (err)
      res.send(err);
    
    if(task == null)
    res.json(task);
  });
  TaskT.find
};

exports.retrieve_comp_level_BUG = function(req, res){
  console.log("Retrieve Completed Level");
  var compLevel = [];
  var currentLevel;
  TaskT.findOne({name: req.params.sessionID}, function(err, task) {
    if (err){
      res.send(err);
    }else{
      if(task != null && typeof task == "object"){
        var jsonObjects = task.toJSON();
        Object.entries(jsonObjects.levels).forEach(([key, value]) =>{
          currentLevel = value.name;
          if(value.progress == "Passed" && currentLevel != ""){
            compLevel.push(currentLevel + " " + 1);
            currentLevel = "";
          }else{
            compLevel.push(currentLevel + " " + 0);
          }
          currentLevel = "";
        });
      };
      compLevel = compLevel.filter(onlyUnique);
      //sendBack = compLevel.sort();
      res.json(compLevel.toString());
    }
  });
}

//Updates the current level with the related info
exports.put_current_level_BUG = function(req, res){
  console.log("Update Completed Level");
  console.log(req.body);
  var objName = req.params.name;
  var sessionID = req.params.sessionID.toString();

  console.log(objName);
  console.log(sessionID);


  TaskT.findOne({name: req.params.sessionID}, 'levels.name', function(err, task){
    var i = 0;
    if(task != null && typeof task == "object"){
      if(err){
        res.json(err);
      }else{
        var taskObj = task.toJSON();
        console.log("taskObj" + taskObj)
        Object.entries(taskObj.levels).forEach(([key, value]) =>{
          i +=1;
        });
      }
    }
    i -=1;

    console.log("i after if" + i)

    var query1 = {}
    var criteria1 = "levels." + i + "." + objName; 
    query1[criteria1] = req.body[objName];

    console.log("query1" + query1)
    console.log("criteria" + criteria1)
    console.log("query[crit1]" + query1[criteria1])
  
    var query = {};
    var criteria = "name";
    query[criteria] = sessionID;
    
    if(req.body['totalPoints'] || req.body['upgrades']){

      var query2 = {}
      var criteria2 = objName; 
      query2[criteria2] = req.body[objName];
      var updateTypes;
      if(req.body['totalPoints']){
        TaskT.updateOne(
          query,{$set : query2}, {new:true},function(err1,task1){
            if(err){
              res.json("ERR " + err1);
            }else{
              res.json(task1);
            }
          }
        )
      }else{
        TaskT.updateOne(
          query,{$push : query2}, {new:true},function(err1,task1){
            if(err){
              res.json("ERR " + err1);
            }else{
              res.json(task1);
            }
          }
        )
      }
    }
    else if(req.body["failures"]||req.body["AdaptiveMode"]||req.body["HintMode"]||req.body["totalPoint"] ||req.body["timeEnded"] || req.body["finalEnergy"] || req.body["progress"] || req.body["time"] || req.body["progress"] || req.body["points"] || req.body["timeBonus"]){
      TaskT.updateOne(
        query,
        {$set : query1}, 
        {new:true},
        function(err1,task1){
          if(err){
            res.json("ERR " + err1);
          }else{
            res.json(task1);
          }
        }
      )
  
    }else{
      TaskT.findOneAndUpdate(
        query,
        {$push : query1}, 
        {new:true},
        function(err1,task1){
          if(err){
            res.json("ERR " + err1);
          }else{
            res.json(task1);
          }
        }
      )
  
    }

  });
};

exports.retrieve_upgrade_points_BUG = function(req,res){
  console.log("Get Upgrade points")
  var queryCl = {};
  var criteria = req.params.name;
  var criteria2 = "_id";

  queryCl[criteria] = 1;
  queryCl[criteria2] = 0;
  TaskT.findOne({name: req.params.sessionID} ,queryCl, function(err, task) {
    if (err)
      res.send(err);
    res.json(task);
  });

};

//Used when going between levels saves upgrades at student lvl
exports.put_upgrade_points_BUG = function(req,res){
    console.log("Put Upgrade points")
    console.log(req.body)
    var queryCl = {};
    var criteria = req.params.name;

    queryCl[criteria] = req.body[ req.params.name];
    
    TaskT.updateOne({name: req.params.sessionID},
      {$set  : queryCl}, function(err1, task1){
          if(err1){
              res.send(err1);
          }else{
              res.json(task1);
          }
      });

}

exports.list_all_leaderboard_BUG = function(req,res){
  var levelName = req.params.levelName
  console.log(levelName);
  TaskT.aggregate([
    {$match: {'levels.name' : levelName}},
    {$unwind: '$levels'},
    {$project:{name: 1,username: 1, _id:0, "levels":{
      $map :{
        input: {
          $filter:{
            input: ['$levels'],
            as: 'level',
            cond:{ $and:[
              {$eq: ['$$level.name', levelName]},
              {$eq : ['$$level.progress', 'Passed']},
              {$gte : ['$$level.points', "10"]},
              {$ne :['$$level', null]}
            ]}
          }},
          as: 'levelM',
          in: {
            'level_name': '$$levelM.name',
            'level_point': '$$levelM.points'
          }
        }
      }
    }},
    {$sort: {'levels.level_point': -1}}

  ]).exec(function(err,task){
    if(err){
      res.send(err);
    }
    
    var scores = {
      leaderscores:[]
    };

    var tmp = "";
    var iter = 0;
    for(var i in task){
      var item = task[i];
      if(item.levels[0] != null && !(tmp.includes(item.name))){
        tmp += item.name + " ";
        var item2 = item.levels[0]
        scores.leaderscores.push({
          "name": item.name,
          "username": item.username,
          "levelName" : item2.level_name,
          "points": item2.level_point,
          "rank": ""
        });
      }
    }
    scores.leaderscores = scores.leaderscores.sort(sortJsonArrByPoints);

    scores.leaderscores.forEach(function(arr){
      arr.rank = iter + 1;
      iter +=1;
    });
    res.json(scores);
  })
}

exports.check_word_BUG = function(req,res){
  var Filter = require('bad-words'),
    filter = new Filter();

    if(filter.isProfane(req.params.word)){
      res.send(true);
    }else{
      TaskT.findOne({username: req.params.word}, function(err, task){
        if(err){
          res.send(true);
        }else if(task){
          res.send(true);
        }else{
          res.send(false);
        }
      })
    }
}


//Function is working as intended
exports.create_a_course_BUG = function(req,res){
  TaskC.findOne({'courseCode': req.params.courseCode},function(err,task){
    if (task!=null){
      console.log("CourseCode found")
      var newStudent = new TaskT(req.body['studentStart'])

      TaskC.findOne({'courseCode': req.params.courseCode, 'students.name': newStudent.name},function(err,obj){
        if (obj==null){
          console.log("No Similar Student")
          TaskC.findOneAndUpdate({'courseCode': req.params.courseCode},{$push: {'students': req.body['studentStart']}},function(err,obj){
            if(err){
              console.log(err)
            }
          })
        } else {
          console.log("Already Student")
        }
      })
    } else {
      console.log("CourseCode not found")
      var newStudent = new TaskT(req.body['studentStart']);
      var newCourse = new TaskC ({
        courseCode: req.params.courseCode,
        students: newStudent
      });

      newCourse.save(function(err, task) {
        if (err)
          res.send(err);
        res.json(task);
      });
    }
  })
  
};

//Work on gets later, Also this one currently isn't being called
exports.list_all_course_BUG = function(req,res){
  console.log("List all course Called")
};

//Called when you start the level only
exports.update_a_course_BUG = function(req, res) {
  console.log("New Level Called")

  TaskC.findOneAndUpdate({'courseCode': req.params.courseCode, 'students.name': req.params.sessionID}, 
  {
      "$push" : {
          "students.$.levels": {
            $each: req.body['levels'],
            $position: 0
          }
      }
  },
  function(err,obj){
    if(err){
      res.send("ERR " + err);
    }else{
      res.send("req1");
    }
  })
  
};

//Work on gets later
exports.read_a_course_BUG = function(req,res){
  console.log("Read a course Called")
};

//Work on get later, not called
exports.retrieve_comp_level_course_BUG = function (req,res){
  console.log("Retrieve Level Called")
};

exports.put_current_level_course_BUG = function (req,res){

  var objName = req.params.name;
  
  var query1 = {};
  var criteria1 = "students.$[outer].levels.0." + objName; 
  query1[criteria1] = req.body[objName];

  console.log(objName);

  //Used when upgrading your character
  if(req.body['totalPoints'] || req.body['upgrades']){

      if(req.body['totalPoints']){

        var query1 = {};
        var setTo = "students.$." + objName;
        query1[setTo] = req.body[objName];

        TaskC.update({'courseCode': req.params.courseCode, 'students.name': req.params.sessionID}, 
        {$set:query1},
        function(err,obj){
          if(err){
            console.log("Mario Implement this!")
            res.send(err);
          }else{
            console.log("Mario Implement this!")
            res.json(obj);
          }
        })
      }else{

        console.log(req.body)
        var query2 = {};
        var setTo = "students.$[outer]." + objName;
        query2[setTo] = req.body[objName];

        TaskC.update(
          {}, 
          {$push:query2},
          {
            "arrayFilters" : [{"outer.name": req.params.sessionID}]
          },
          function(err,obj){
            if(err){
              res.send(err);
            }else{
              res.json(obj);
            }
          })
      }

  }
  else if (req.body["bugLine"]||req.body["AdaptiveMode"]||req.body["hitByEnemy"]||req.body["failedToolUse"]||req.body["failures"]||req.body["AdaptiveCategorization"]||req.body["AdaptiveMode"]||req.body["HintMode"]||req.body["totalPoint"] ||req.body["timeEnded"] || req.body["finalEnergy"] || req.body["progress"] || req.body["time"] || req.body["progress"] || req.body["points"] || req.body["timeBonus"] || req.body["star"]){
    
    TaskC.findOneAndUpdate(
      {'courseCode': req.params.courseCode},
      {$set: query1},
      {
        "arrayFilters" : [{"outer.name": req.params.sessionID}]
      },
      function(err,obj){
        if(err){
          res.send("ERR " + err);
        }else{
          res.json(obj);
        }
      }
    )
  } else if (req.body["idleTime"]) {

    TaskC.findOneAndUpdate(
      {'courseCode': req.params.courseCode},
      {$inc: query1},
      {
        "arrayFilters" : [{"outer.name": req.params.sessionID}]
      },
      function(err,obj){
        if(err){
          res.send("ERR " + err);
        }else{
          res.json(obj);
        }
      }
    )
  }
  else {
    objName = req.params.name;
  
    var query1 = {};
    var criteria1 = "students.$[outer].levels.0.events"; 
    query1[criteria1] = req.body[objName];

    TaskC.findOneAndUpdate(
      {'courseCode': req.params.courseCode},
      {$push: query1},
      {
        "arrayFilters" : [{"outer.name": req.params.sessionID}]
      },
      function(err,obj){
        if(err){
          res.send("ERR " + err);
        }else{
          res.json(obj);
        }
      }
    )    
  }
  
};

//Not Implemented
exports.retrieve_upgrade_points_course_BUG = function(req,res){
  console.log("Get Upgrade points course")
}

//Used when going between levels saves upgrades at student lvl
exports.put_upgrade_points_course_BUG = function(req,res){
  var objName = req.params.name;
  var query1 = {};
  var setTo = "students.$." + objName;
  query1[setTo] = req.body[objName];

  TaskC.update({'courseCode': req.params.courseCode, 'students.name': req.params.sessionID}, 
  {$set:query1},
  function(err,obj){
    if(err){
      res.send(err);
    }else{
      res.json(obj);
    }
  })
}

/*
exports.retrieve_ml_BUG = function(req,res){
  console.log("ML Called")

  //Does not include levels that quit a level right away not to menu, and levels that are ongoing levels.
  TaskC.aggregate(
    [{$unwind:'$students'},{$unwind:'$students.levels'},{ $match: {'students.levels.timeEnded': {$ne:"N/A"}} },{ $match : { 'students.levels.progress' : "Passed", } },{ $project:{'courseCode':1,'students.levels.name':1, 'students.levels.timeStarted':1, 'students.levels.timeEnded':1, 'students.levels.failedToolUse':1,}}],
    function(err,obj){
      if(err){
        res.send(err);
      }else{
        res.json(obj);
      }
    }
  )
}
*/

exports.change_ml_adaptive_BUG = function(req,res){
  console.log("ML auto change called")

  TaskC.aggregate(
    [{$unwind:'$students'},{ $match : { 'students.levels.AdaptiveMode' : "0", } },{ $count: "amountOfMLOn"}],
    function(err,obj){
      if(err){
        res.send(err);
      }else{
        res.json(obj);
      }
    }
  )
}

//Function that will update the MLData stored in the TaskM Model
exports.js_Stuff = function(req,res){

  //Does not include levels that quit a level right away not to menu, and levels that are ongoing levels.
  TaskC.aggregate(
    [{$unwind:'$students'},{$unwind:'$students.levels'},{ $match: {'students.levels.timeEnded': {$ne:"N/A"}} },{ $match : { 'students.levels.progress' : "Passed", } },{ $project:{'courseCode':1,'students.levels.name':1, 'students.levels.timeStarted':1, 'students.levels.timeEnded':1, 'students.levels.failedToolUse':1,}}],
    function(err,obj){
      if(err){
        res.send(err);
      }else{

        //This will clear the MLSchema Model
        TaskM.deleteMany(
          function(err){
            if(err){
              res.send(err);
            }
        });

        //Loops through the levels and adds each one to the Model, need to do like this due to _ids
        for (const key in obj) {
          var new_task = new TaskM({
            levelName: obj[key].students.levels.name,
            timeStarted: obj[key].students.levels.timeStarted,
            timeEnded: obj[key].students.levels.timeEnded,
            failedToolUse: obj[key].students.levels.failedToolUse
          });
          new_task.save(function(err){
            if(err){
              res.send(err);
            }});
        }

        res.json("Updated the Machine Learning Data");
      }
    }
  )
}


//This will return the MLData when called.
exports.get_ml_BUG = function(req,res){
  console.log("Machine Learning Data Gotten");
  TaskM.find({},function(err,obj){
    if(err){
      res.send(err);
    }else {
      res.json(obj);
    }
  });
}


//This will return the Centroids for a level when called.
exports.get_levelCentroids_BUG = function(req,res){
  console.log("Centroids Gotten");
  TaskCT.find({'levelName': req.params.levelName},function(err,obj){
    if(err){
      res.send(err);
    }else {
      res.json(obj);
    }
  });

  res.send("Not Found")
}



const MAX_ITERATIONS = 500;
exports.do_K_Means = function(req,res){


  //Number of groups/clusters
  var k = 3
  //Get the MLData from Schema
  TaskM.find({},function(err,obj){
    if(err){
      console.log("Error getting ml data from database");
    }else {
      //console.log(obj);
      const dataset = new Array();
      var levelDoneCounter = 0;
      var maxTime = Number.MIN_VALUE;
      var minTime = Number.MAX_VALUE;
      var maxFail = Number.MIN_VALUE;
      var minFail = Number.MAX_VALUE;

      //Parse the result to use in ML
      //console.log(obj.length);

      for (let i = 0; i < obj.length; i++) {  

        //console.log(obj[i].levelName);
        //Check for the level to be the same
        //Also get the Min and Max values for the normalization of the data
        if (obj[i].levelName == req.params.levelName){
          levelDoneCounter++;
          var timeElapsed = Date.parse(obj[i].timeEnded) - Date.parse(obj[i].timeStarted); 
          //console.log(timeElapsed);

          var failedToolUse = parseFloat(obj[i].failedToolUse)

          if (timeElapsed > maxTime){
            maxTime = timeElapsed;
          } else if (timeElapsed < minTime){
            minTime = timeElapsed;
          }

          if (failedToolUse > maxFail){
            maxFail = failedToolUse;
          } else if (failedToolUse < minFail){
            minFail = failedToolUse;
          }          

          //console.log(maxTime);
          //console.log(minTime);
          //console.log(maxFail);
          //console.log(minFail);

          dataset.push([timeElapsed,failedToolUse]);
        }
      }

      //Will only run the algo, if the lvl has been played at least 3 times
      if (levelDoneCounter>3){
        //console.log(dataset);
      
        //This will loop through and normalize the values in the dataset
        for (let j = 0; j < dataset.length; j++) {  
          dataset[j][0] = ((dataset[j][0] - minTime) / (maxTime - minTime)) *100 //(xi – min(x)) / (max(x) – min(x)) * 100 I normalize the data between 0 and 100
          dataset[j][1] = ((dataset[j][1] - minFail) / (maxFail - minFail)) *100
        }

        //console.log(dataset);

        //Run the KMeans with dataset and clusters
        var result = kmeans(dataset, k)
        //console.log(result);
        var centroids = result.centroids;

        //Fix some formatting issues
        const centroids2 = new Array();
        for (let j = 0; j < centroids.length; j++) {
            centroids2.push({timeElapsed: centroids[j][0], failedToolUse: centroids[j][1]}); 
        }

        //Add to new Schema
        //Loops through the centroids and adds each one to the CentroidModel
        var new_task = new TaskCT({
          levelName: req.params.levelName,
          centroids: centroids2,
          maxTime: maxTime,
          minTime: minTime,
          maxFail: maxFail,
          minFail: minFail
        });
        
        //Find the last instance of the level in the model and delete it
        TaskCT.findOneAndRemove({'levelName': req.params.levelName},function(err){
          if(err){
            res.send(err);
          }
        });

        //Save the results
        new_task.save(function(err){
          if(err){
            res.send(err);
          }
        });

        res.send("Kmeans Successfully run for: " + req.params.levelName);
        console.log("Kmeans Successfully run for: " + req.params.levelName);
      } else{
        res.send("Kmeans Not Run for: " + req.params.levelName);
        console.log("Kmeans Not Run for: " + req.params.levelName);
      } 
    }
  });
}


//This function will need take the timeElapsed and failures, and use that to call the centroids function and compare to find the group for the given data

exports.compare_level_centroid = function(req,res){
  
  TaskCT.find({'levelName': req.params.levelName},function(err,obj){
    if(err){
      console.log("Level Name does not exist yet");
      res.json(0);
    } else if(obj.length == 0){

      //If machine learning not yet run on lvl, return 0
      res.json(0);
    }
    else{
      var minTime = obj[0].minTime;
      var maxTime = obj[0].maxTime;
      var minFail = obj[0].minFail;
      var maxFail = obj[0].maxFail;

      //Get the levelData and normalize and centroids
      //Note the 1000x for going from seconds to milliseconds
      
      var point = [req.params.timeElapsed*1000,req.params.failures];
      
      point[0] = ((point[0] - minTime) / (maxTime - minTime)) *100 //(xi – min(x)) / (max(x) – min(x)) * 100 I normalize the data between 0 and 100
      point[1] = ((point[1] - minFail) / (maxFail - minFail)) *100
     
      
      //Find the group for the level
      var currentLowestDistance;
      var currentLowestDistanceGroup;
      var euclidianDistance;

      var centroids = obj[0].centroids;

      //Sort based on amount of failures
      centroids.sort((a, b) => {
          return a.failedToolUse - b.failedToolUse;
      });
        
      for (let i = 0; i < centroids.length; i++) {
          //Not true Euclidian added a 20/80 split weight to each factor
          euclidianDistance = Math.sqrt(Math.pow(centroids[i].timeElapsed-point[0],2) + Math.pow(centroids[i].failedToolUse-point[1],2));

          if (i==0){
              currentLowestDistance = euclidianDistance;
              currentLowestDistanceGroup = i;
          } else if(currentLowestDistance>euclidianDistance){
              currentLowestDistance = euclidianDistance;
              currentLowestDistanceGroup = i;
          }

          console.log(euclidianDistance);
      }

      console.log("Lowest Distance:" + currentLowestDistance);
      console.log("Group for this person: " + currentLowestDistanceGroup);
      
      res.json(currentLowestDistanceGroup);
    }
  })
}


function get_ml(){
  TaskM.find({},function(err,obj){
    if(err){
      console.log("Error getting ml data from database");
    }else {
      //console.log(obj);
      return obj;
    }
  });
}



//These functions actually compute the ML Model using K-Means Clusters
//These will return the clusters and centroids
//Taken partially from: https://medium.com/geekculture/implementing-k-means-clustering-from-scratch-in-javascript-13d71fbcb31e

function getRandomCentroids(dataset, k) {
    // selects random points as centroids from the dataset
    const numSamples = dataset.length;
    const centroidsIndex = [];
    let index;
    while (centroidsIndex.length < k) {
      index = randomBetween(0, numSamples);
      if (centroidsIndex.indexOf(index) === -1) {
        centroidsIndex.push(index);
      }
    }
    const centroids = [];
    for (let i = 0; i < centroidsIndex.length; i++) {
      const centroid = [...dataset[centroidsIndex[i]]];
      centroids.push(centroid);
    }
    return centroids;
}

// Calculate Squared Euclidean Distance
function getDistanceSQ(a, b) {
  const diffs = [];
  for (let i = 0; i < a.length; i++) {
    diffs.push(a[i] - b[i]);
  }
  return diffs.reduce((r, e) => (r + (e * e)), 0);
}

// Returns a label for each piece of data in the dataset. 
function getLabels(dataSet, centroids) {
  // prep data structure:
  const labels = {};
  for (let c = 0; c < centroids.length; c++) {
    labels[c] = {
      points: [],
      centroid: centroids[c],
    };
  }
  // For each element in the dataset, choose the closest centroid. 
  // Make that centroid the element's label.
  for (let i = 0; i < dataSet.length; i++) {
    const a = dataSet[i];
    let closestCentroid, closestCentroidIndex, prevDistance;
    for (let j = 0; j < centroids.length; j++) {
      let centroid = centroids[j];
      if (j === 0) {
        closestCentroid = centroid;
        closestCentroidIndex = j;
        prevDistance = getDistanceSQ(a, closestCentroid);
      } else {
        // get distance:
        const distance = getDistanceSQ(a, centroid);
        if (distance < prevDistance) {
          prevDistance = distance;
          closestCentroid = centroid;
          closestCentroidIndex = j;
        }
      }
    }
    // add point to centroid labels:
    labels[closestCentroidIndex].points.push(a);
  }
  return labels;
}

function getPointsMean(pointList) {
    const totalPoints = pointList.length;
    const means = [];
    for (let j = 0; j < pointList[0].length; j++) {
      means.push(0);
    }
    for (let i = 0; i < pointList.length; i++) {
      const point = pointList[i];
      for (let j = 0; j < point.length; j++) {
        const val = point[j];
        means[j] = means[j] + val / totalPoints;
      }
    }
    return means;
  }
  
  function recalculateCentroids(dataSet, labels, k) {
    // Each centroid is the geometric mean of the points that
    // have that centroid's label. Important: If a centroid is empty (no points have
    // that centroid's label) you should randomly re-initialize it.
    let newCentroid;
    const newCentroidList = [];
    for (const k in labels) {
      const centroidGroup = labels[k];
      if (centroidGroup.points.length > 0) {
        // find mean:
        newCentroid = getPointsMean(centroidGroup.points);
      } else {
        // get new random centroid
        newCentroid = getRandomCentroids(dataSet, 1)[0];
      }
      newCentroidList.push(newCentroid);
    }
    return newCentroidList;
  }

function randomBetween(min, max) {
  return Math.floor(
    Math.random() * (max - min) + min
  );
}

function calcMeanCentroid(dataSet, start, end) {
  const features = dataSet[0].length;
  const n = end - start;
  let mean = [];
  for (let i = 0; i < features; i++) {
    mean.push(0);
  }
  for (let i = start; i < end; i++) {
    for (let j = 0; j < features; j++) {
      mean[j] = mean[j] + dataSet[i][j] / n;
    }
  }
  return mean;
}

function getRandomCentroidsNaiveSharding(dataset, k) {
  // implementation of a variation of naive sharding centroid initialization method
  // (not using sums or sorting, just dividing into k shards and calc mean)
  // https://www.kdnuggets.com/2017/03/naive-sharding-centroid-initialization-method.html
  const numSamples = dataset.length;
  // Divide dataset into k shards:
  const step = Math.floor(numSamples / k);
  const centroids = [];
  for (let i = 0; i < k; i++) {
    const start = step * i;
    let end = step * (i + 1);
    if (i + 1 === k) {
      end = numSamples;
    }
    centroids.push(calcMeanCentroid(dataset, start, end));
  }
  return centroids;
}

function getRandomCentroids(dataset, k) {
  // selects random points as centroids from the dataset
  const numSamples = dataset.length;
  const centroidsIndex = [];
  let index;
  while (centroidsIndex.length < k) {
    index = randomBetween(0, numSamples);
    if (centroidsIndex.indexOf(index) === -1) {
      centroidsIndex.push(index);
    }
  }
  const centroids = [];
  for (let i = 0; i < centroidsIndex.length; i++) {
    const centroid = [...dataset[centroidsIndex[i]]];
    centroids.push(centroid);
  }
  return centroids;
}

function compareCentroids(a, b) {
  for (let i = 0; i < a.length; i++) {
    if (a[i] !== b[i]) {
      return false;
    }
  }
  return true;
}

function shouldStop(oldCentroids, centroids, iterations) {
  if (iterations > MAX_ITERATIONS) {
    return true;
  }
  if (!oldCentroids || !oldCentroids.length) {
    return false;
  }
  let sameCount = true;
  for (let i = 0; i < centroids.length; i++) {
    if (!compareCentroids(centroids[i], oldCentroids[i])) {
      sameCount = false;
    }
  }
  return sameCount;
}

// Calculate Squared Euclidean Distance
function getDistanceSQ(a, b) {
  const diffs = [];
  for (let i = 0; i < a.length; i++) {
    diffs.push(a[i] - b[i]);
  }
  return diffs.reduce((r, e) => (r + (e * e)), 0);
}

// Returns a label for each piece of data in the dataset. 
function getLabels(dataSet, centroids) {
  // prep data structure:
  const labels = {};
  for (let c = 0; c < centroids.length; c++) {
    labels[c] = {
      points: [],
      centroid: centroids[c],
    };
  }
  // For each element in the dataset, choose the closest centroid. 
  // Make that centroid the element's label.
  for (let i = 0; i < dataSet.length; i++) {
    const a = dataSet[i];
    let closestCentroid, closestCentroidIndex, prevDistance;
    for (let j = 0; j < centroids.length; j++) {
      let centroid = centroids[j];
      if (j === 0) {
        closestCentroid = centroid;
        closestCentroidIndex = j;
        prevDistance = getDistanceSQ(a, closestCentroid);
      } else {
        // get distance:
        const distance = getDistanceSQ(a, centroid);
        if (distance < prevDistance) {
          prevDistance = distance;
          closestCentroid = centroid;
          closestCentroidIndex = j;
        }
      }
    }
    // add point to centroid labels:
    labels[closestCentroidIndex].points.push(a);
  }
  return labels;
}

function getPointsMean(pointList) {
  const totalPoints = pointList.length;
  const means = [];
  for (let j = 0; j < pointList[0].length; j++) {
    means.push(0);
  }
  for (let i = 0; i < pointList.length; i++) {
    const point = pointList[i];
    for (let j = 0; j < point.length; j++) {
      const val = point[j];
      means[j] = means[j] + val / totalPoints;
    }
  }
  return means;
}

function recalculateCentroids(dataSet, labels, k) {
  // Each centroid is the geometric mean of the points that
  // have that centroid's label. Important: If a centroid is empty (no points have
  // that centroid's label) you should randomly re-initialize it.
  let newCentroid;
  const newCentroidList = [];
  for (const k in labels) {
    const centroidGroup = labels[k];
    if (centroidGroup.points.length > 0) {
      // find mean:
      newCentroid = getPointsMean(centroidGroup.points);
    } else {
      // get new random centroid
      newCentroid = getRandomCentroids(dataSet, 1)[0];
    }
    newCentroidList.push(newCentroid);
  }
  return newCentroidList;
}

function kmeans(dataset, k, useNaiveSharding = true) {
  if (dataset.length && dataset[0].length && dataset.length > k) {
    // Initialize book keeping variables
    let iterations = 0;
    let oldCentroids, labels, centroids;

    // Initialize centroids randomly
    if (useNaiveSharding) {
      centroids = getRandomCentroidsNaiveSharding(dataset, k);
    } else {
      centroids = getRandomCentroids(dataset, k);
    }

    // Run the main k-means algorithm
    while (!shouldStop(oldCentroids, centroids, iterations)) {
      // Save old centroids for convergence test.
      oldCentroids = [...centroids];
      iterations++;

      // Assign labels to each datapoint based on centroids
      labels = getLabels(dataset, centroids);
      centroids = recalculateCentroids(dataset, labels, k);
    }

    const clusters = [];
    for (let i = 0; i < k; i++) {
      clusters.push(labels[i]);
    }
    const results = {
      clusters: clusters,
      centroids: centroids,
      iterations: iterations,
      converged: iterations <= MAX_ITERATIONS,
    };
    return results;
  } else {
    throw new Error('Invalid dataset');
  }
}


/*
module.exports = {
  getAllProducts: async (req, res, next) => {
    try {
      const results = await Product.find({}, { __v: 0 });
      // const results = await Product.find({}, { name: 1, price: 1, _id: 0 });
      // const results = await Product.find({ price: 699 }, {});
      res.send(results);
    } catch (error) {
      console.log(error.message);
    }
  },

  createNewProduct: async (req, res, next) => {
    try {
      const product = new Product(req.body);
      const result = await product.save();
      res.send(result);
    } catch (error) {
      console.log(error.message);
      if (error.name === 'ValidationError') {
        next(createError(422, error.message));
        return;
      }
      next(error);
    }

    /*Or:
  If you want to use the Promise based approach*/
    /*
  const product = new Product({
    name: req.body.name,
    price: req.body.price
  });
  product
    .save()
    .then(result => {
      console.log(result);
      res.send(result);
    })
    .catch(err => {
      console.log(err.message);
    }); 
    */
    /*
  },

  findProductById: async (req, res, next) => {
    const id = req.params.id;
    try {
      const product = await Product.findById(id);
      // const product = await Product.findOne({ _id: id });
      if (!product) {
        throw createError(404, 'Product does not exist.');
      }
      res.send(product);
    } catch (error) {
      console.log(error.message);
      if (error instanceof mongoose.CastError) {
        next(createError(400, 'Invalid Product id'));
        return;
      }
      next(error);
    }
  },

  updateAProduct: async (req, res, next) => {
    try {
      const id = req.params.id;
      const updates = req.body;
      const options = { new: true };

      const result = await Product.findByIdAndUpdate(id, updates, options);
      if (!result) {
        throw createError(404, 'Product does not exist');
      }
      res.send(result);
    } catch (error) {
      console.log(error.message);
      if (error instanceof mongoose.CastError) {
        return next(createError(400, 'Invalid Product Id'));
      }

      next(error);
    }
  },

  deleteAProduct: async (req, res, next) => {
    const id = req.params.id;
    try {
      const result = await Product.findByIdAndDelete(id);
      // console.log(result);
      if (!result) {
        throw createError(404, 'Product does not exist.');
      }
      res.send(result);
    } catch (error) {
      console.log(error.message);
      if (error instanceof mongoose.CastError) {
        next(createError(400, 'Invalid Product id'));
        return;
      }
      next(error);
    }
  }
};

*/
