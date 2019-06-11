using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class WebGLBuilder
{
    static void build() {

    // Place all your scenes here
    string[] scenes = {"Assets/TitleScene.unity", 
                        "Assets/MainMenu.unity",
                        "Assets/newgame.unity",
                        "Assets/Cinematic.unity",
                        "Assets/CharacterSelect.unity",
                        "Assets/IntroScene.unity",
                        "Assets/Transition.unity",
                        "Assets/Credits.unity"};

    string pathToDeploy = "builds/WebGLversion/";       

    BuildPipeline.BuildPlayer(scenes, pathToDeploy, BuildTarget.WebGL, BuildOptions.None);      
    }
}
