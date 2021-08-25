'use strict';

var mongoose = require('mongoose'),
    Task = mongoose.model('RobotON_Logs'),
    TaskT = mongoose.model('RobotBug_Logs'),
    TaskC = mongoose.model('RobotBug_Course');

//-------------------------------EXTERNAL FUNCTIONS------------------------------------------->
function onlyUnique(value, index, self){
  return self.indexOf(value) === index;
}

function sortJsonArrByPoints(a,b){
  return b.points - a.points;
}
//-------------------------------EOF EXTERNAL FUNCTIONS!--------------------------------------->


//GET REQUEST FOR ALL DATA UNDER ON
exports.list_all_logs_ON = function(req,res){
    Task.find({}, function(err, task) {
        if (err)
          res.send(err);
        res.json(task);
      });
};

exports.list_all_leaderboard_ON = function(req,res){
  var levelName = req.params.levelName
  console.log(levelName);
  Task.aggregate([
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
          "username" : item.username,
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

//Creates a new log for that sessionID
exports.create_a_log_ON = function(req, res) {
  var new_task = new Task(req.body);
  new_task.save(function(err, task) {
    if (err)
      res.send(err);
    res.json(task);
  });
};


//GET REQUEST FOR info Related to the SessionID
exports.read_a_log_ON = function(req, res) {
  Task.findOne({name: req.params.sessionID}, function(err, task) {
    if (err)
      res.send(err);
    res.json(task);
  });
};

//Update the log that is tied with the sessionID
exports.update_a_log_ON = function(req, res) {
  Task.findOneAndUpdate(
    {name: req.params.sessionID}, 
    {$push : {
      levels: req.body['levels']}}, 
    {new: true},
    function(err, task) {
    if (err){
      res.send(err);
    }
    res.json(task);
  });
  Task.find
};

//Gets the retrieved list of completed level
exports.retrieve_comp_level_ON = function(req, res){
  var compLevel = [];
  var currentLevel;
  Task.findOne({name: req.params.sessionID}, function(err, task) {
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
exports.put_current_level_ON = function(req, res){
  var objName = req.params.name;
  var sessionID = req.params.sessionID.toString();

  Task.findOne({name: req.params.sessionID}, 'levels.name', function(err, task){
    var i = 0;
    if(task != null && typeof task == "object"){
      if(err){
        res.json(err);
      }else{
        var taskObj = task.toJSON();
        Object.entries(taskObj.levels).forEach(([key, value]) =>{

          i +=1;
        });
      }
    }
    i -=1;

    var query1 = {}
    var criteria1 = "levels." + i + "." + objName; 
    query1[criteria1] = req.body[objName];
  
    var query = {};
    var criteria = "name";
    query[criteria] = sessionID;

    
    if(req.body['totalPoints'] || req.body['upgrades']){

      var query2 = {}
      var criteria2 = objName; 
      query2[criteria2] = req.body[objName];
      var updateTypes;
      if(req.body['totalPoints']){
        Task.updateOne(
          query,{$set : query2}, {new:true},function(err1,task1){
            if(err){
              res.json("ERR " + err1);
            }else{
              res.json(task1);
            }
          }
        )
      }else{
        Task.updateOne(
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
      Task.updateOne(
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
      Task.findOneAndUpdate(
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


exports.retrieve_upgrade_points_ON = function(req,res){
  var queryCl = {};
  var criteria = req.params.name;
  var criteria2 = "_id";

  queryCl[criteria] = 1;
  queryCl[criteria2] = 0;
  Task.findOne({name: req.params.sessionID} ,queryCl, function(err, task) {
    if (err)
      res.send(err);
    res.json(task);
  });

};

exports.put_upgrade_points_ON = function(req,res){
    var queryCl = {};
    var criteria = req.params.name;

    queryCl[criteria] = req.body[ req.params.name];
    
    Task.updateOne({name: req.params.sessionID},
      {$set  : queryCl}, function(err1, task1){
          if(err1){
              res.send(err1);
          }else{
              res.json(task1);
          }
      });

}

exports.check_word_ON = function(req,res){
  var Filter = require('bad-words'),
    filter = new Filter();

    if(filter.isProfane(req.params.word)){
      res.send(true);
    }else{
      Task.findOne({username: req.params.word}, function(err, task){
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

//------------------------------------------------------------------------------->
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