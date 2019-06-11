  set mypath=%cd%
    @echo %mypath%
    "C:\Program Files\Unity\Hub\Editor\2019.1.0f2\Editor\Unity.exe" -quit -batchmode -logFile stdout.log -projectPath %mypath% -executeMethod WebGLBuilder.build