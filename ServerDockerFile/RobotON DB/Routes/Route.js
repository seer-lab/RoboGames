const express = require('express');
const router = express.Router();

const ProductController = require('../Controllers/Controller');

//Get a list of all BUG logs
router.get('/', ProductController.list_all_logs_BUG);

//List all Leaderboard Data (Currently Not Used)
router.get('/leaderboard/:levelName', ProductController.list_all_leaderboard_BUG);

//Check Word (Currently Not Used)
router.get('/check/:word',ProductController.check_word_BUG)

//Call on end of lvl to put level completed to database
router.put('/currentlevel/:courseCode/:sessionID/:name',ProductController.put_current_level_course_BUG)

//Not Implemented or changed yet
//router.get('/logsBUG/completedlevels/:sessionID')
//.get(ProductController.retrieve_comp_level_BUG);

//Send student points to log, stored outside the level array, need to fix
router.put('/points/:courseCode/:sessionID/:name',ProductController.put_upgrade_points_course_BUG)
//.get(ProductController.retrieve_upgrade_points_course_BUG)

/*
//Not used anymore, Called for the old machine learning algo, 
router.get('/logsBUG/ml/:courseCode/:sessionID')
.get(ProductController.get_ml_BUG)
*/

/*
//Not Used, Called for auto ML ON/OFF, used if based on # of students
router.get('/logsBUG/mlAuto/:courseCode/:sessionID')
.get(ProductController.change_ml_adaptive_BUG)
*/

//Function that will update the MLData stored in the TaskM Model, using the data in the other Model - We want to set this on an interval
router.get('/tryingJS', ProductController.js_Stuff)

//This will get the machine learning data in the database, this is used for debugging purposes
router.get('/ml', ProductController.get_ml_BUG)

//This will run the machine learning algo, we want to place this on an interval or to run at a certain time
router.get('/ml/callAlgo/:levelName', ProductController.do_K_Means)

//Compare the centroids to the given lvl
router.get('/ml/centroids/:levelName/:timeElapsed/:failures', ProductController.compare_level_centroid)

//Call on sessionID screen, it will create/check if course and student exist
router.post('/:courseCode', ProductController.create_a_course_BUG)
//.get(ProductController.list_all_logs_BUG)

//Call on lvl start
router.put('/:courseCode/:sessionID', ProductController.update_a_course_BUG);
//.get(ProductController.read_a_log_BUG)

module.exports = router;
