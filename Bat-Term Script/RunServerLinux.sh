#! /bin/bash
xfce4-terminal --working-directory=/home/ibrahim/Desktop/RobotON/ -e 'killall -9 node'
xfce4-terminal --working-directory=/home/ibrahim/Desktop/RobotON/RobotON\ DB/ -e 'npm start'
xfce4-terminal --working-directory=/home/ibrahim/Desktop/RobotON/RobotON\ Server/ -e 'npm start'
xfce4-terminal --working-directory=/home/ibrahim/Desktop/WebGLBuild/ -e 'http-server ./ -p8081'
