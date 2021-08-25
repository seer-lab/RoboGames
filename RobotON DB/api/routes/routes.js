'use strict';
//The api routes
module.exports = function(app){
    var logControl = require('../controllers/controller');

    //A Get Request will show all the information stored on the DB
    //A Post Request will start the new session for a USER
    app.route('/logsON')
    .get(logControl.list_all_logs_ON)
    .post(logControl.create_a_log_ON);

    app.route('/logsON/leaderboard/:levelName')
    .get(logControl.list_all_leaderboard_ON);

    //A Get Request will show only the information that is tied with the sessionID
    //A Put requst will update the information related to the sessionID
    app.route('/logsON/:sessionID')
    .get(logControl.read_a_log_ON)
    .put(logControl.update_a_log_ON);

    //A Put requst will update currentlevel with the related info
    app.route('/logsON/currentlevel/:sessionID/:name')
    .put(logControl.put_current_level_ON);

    //A Get Request will grab all the completed level that the user has done
    app.route('/logsON/completedlevels/:sessionID')
    .get(logControl.retrieve_comp_level_ON);

    app.route('/logsON/points/:sessionID/:name')
    .get(logControl.retrieve_upgrade_points_ON)
    .put(logControl.put_upgrade_points_ON);

    app.route('/logsON/check/:word')
    .get(logControl.check_word_ON);

    app.route('/logsBUG')
    .get(logControl.list_all_logs_BUG)

    app.route('/logsBUG/leaderboard/:levelName')
    .get(logControl.list_all_leaderboard_BUG);

    app.route('/logsBUG/check/:word')
    .get(logControl.check_word_BUG);

    //Call on sessionID screen
    app.route('/logsBUG/:courseCode')
    .get(logControl.list_all_logs_BUG)
    .post(logControl.create_a_course_BUG)

    //Call on lvl start
    app.route('/logsBUG/:courseCode/:sessionID')
    .get(logControl.read_a_log_BUG)
    .put(logControl.update_a_course_BUG);

    //Call on end of lvl
    app.route('/logsBUG/currentlevel/:courseCode/:sessionID/:name')
    .put(logControl.put_current_level_course_BUG)

    //Not Implemented or changed yet
    app.route('/logsBUG/completedlevels/:sessionID')
    .get(logControl.retrieve_comp_level_BUG);

    //Called between lvls
    app.route('/logsBUG/points/:courseCode/:sessionID/:name')
    .get(logControl.retrieve_upgrade_points_course_BUG)
    .put(logControl.put_upgrade_points_course_BUG);

    //Called for the machine learning algo
    app.route('/logsBUG/ml/:courseCode/:sessionID')
    .get(logControl.retrieve_ml_BUG)

    //Called for auto ML ON/OFF
    app.route('/logsBUG/mlAuto/:courseCode/:sessionID')
    .get(logControl.change_ml_adaptive_BUG)
};