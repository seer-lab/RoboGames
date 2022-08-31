'use strict';
var mongoose = require('mongoose');
var Schema = mongoose.Schema;
//Models for the DB

var roboSchema = new Schema({
    name: {
        type: String,
        unique: true,
        required: 'Please Enter your name!'
    },
    username: String,
    timeStarted: String,
    totalPoints: String,
    currentPoints: String,
    speedUpgrades: String,
    xpUpgrades: String,
    resistanceUpgrade: String,
    energyUpgrades: String,
    upgrades: [{
        name: String,
        timestamp: String,
        prePoints: String,
        curPoints: String,
    }],
    levels:[{
        name: String,
        time: String,
        progress: String,
        timeStarted: String,
        timeEnded: String,
        stars: String,
        points: String,
        totalPoint: String,
        timeBonus: String,
        finalEnergy: String,
        AdaptiveCategorization: String,
        AdaptiveMode: String,
        HintMode: String,
        failures: String,
        hitByEnemy: String,
        failedToolUse: String,
        bugLine: Number,
        idleTime: Number,
        events : [{
            eventName: String,
            eventType: String,
            line: Number,
            success: Boolean,
            elapsedTime: String,
            realTime: String,
            preEnergy: String,
            finEnergy: String,
            position: {
                x_pos: String,
                y_pos: String,
            },
            comment: String,
        }],
    }]
})

var courseSchema = new mongoose.Schema({
    courseCode:{
        type: String,
        unique: true,
        required: 'Please Enter your course code!'
    },
    students: [roboSchema],
})

/*
var mlSchema = new Schema({
    courseCode: String,
    students: [{
        levels:[{
            name: String,
            timeStarted: String,
            timeEnded: String,
            failedToolUse: String
        }],
    }],
}) */

var mlSchema = new Schema({
   levelName: String,
   timeStarted: String,
   timeEnded: String,
   failedToolUse: String
})

var centroidSchema = new Schema({
    levelName: String,
    centroids: [{
        timeElapsed: String,
        failedToolUse: String
    }],
 })

module.exports = mongoose.model('RobotON_Logs', roboSchema);
module.exports = mongoose.model('RobotBug_Logs', roboSchema);
module.exports = mongoose.model('RobotON_Course', courseSchema);
module.exports = mongoose.model('RobotBug_Course', courseSchema);
module.exports = mongoose.model('RobotBug_ML', mlSchema);
module.exports = mongoose.model('RobotBug_Centroids', centroidSchema);