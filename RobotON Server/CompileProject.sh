#! /bin/bash
git reset --hard
git pull
cd ../
echo Now Compiling Project
/home/ibrahim/Unity/Hub/Editor/2019.2.0b7/Editor/Unity -quit -force-free -nographics -batchmode -logFile stdout.log -projectPath /home/ibrahim/Desktop/RobotON/ -executeMethod WebGLBuilder.build
echo Finished Job, check Logs!
cd RobotON\ Server
node mailingService.js
cd ../
xfce4-terminal --working-directory=/home/ibrahim/Desktop/RobotON/Bat-Term\ Script/ -e './CopyFIleToServerLinux.sh'