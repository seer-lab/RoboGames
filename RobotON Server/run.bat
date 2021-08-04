cd ../
git reset --hard
git pull
"C:\Program Files\Unity\Hub\Editor\2019.2.0b7\Editor\Unity.exe" -quit -batchmode -logFile stdout.log -projectPath "C:\Users\Ibrahim Mushtaq\Desktop\RobotON" -executeMethod WebGLBuilder.build
cd "C:\Users\Ibrahim Mushtaq\Desktop\RobotON\RobotON Server"
node .\mailingService.js
cd "C:\Users\Ibrahim Mushtaq\Desktop\RobotON\Bat-Term Script"
CopyFileToServerWindow.bat