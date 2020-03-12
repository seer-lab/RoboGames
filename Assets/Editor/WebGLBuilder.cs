//I WOULD LOVE TO THANK THE USER PlayItSafe_Fries From the UNITY FORUM https://forum.unity.com/threads/webgl-template.363057/
//  For providing the code for building with templates

/********************************************************************************************
Author: PlayItSafe_Fries
Date: August 19 2019
Availability: https://forum.unity.com/threads/webgl-template.363057/
Code Used: FixIndexHtml(string pathToBuiltProject),
            All Function within Class FileUtilExtended
 ********************************************************************************************/


using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEditor.Build.Reporting;

public class WebGLBuilder
{
    const string TEMPLATE_NAME = "OnTechU Template";
    const string BUILD_PATH = @"/home/ibrahim/Desktop/WebGLBuildServer/RoboTestBUG";
    static void build() {
        string template_path = Path.Combine(Application.dataPath, "WebGLTemplates", TEMPLATE_NAME);

        FileUtilExtended.CreateOrCleanDirectory(Path.Combine(BUILD_PATH, "TemplateData"));
 
        //Copy contents from WebGLTemplate. Ignore all .meta files
        FileUtilExtended.CopyDirectoryFiltered(template_path, BUILD_PATH, true, @".*/\.+|\.meta$", true);

        FixIndexHtml(BUILD_PATH);

        Application.targetFrameRate = 30;

        PlayerSettings.WebGL.dataCaching = true;
        //PlayerSettings.defaultWebScreenHeight = 720;
        //PlayerSettings.defaultWebScreenWidth = 1280;
        PlayerSettings.runInBackground = true;
        AspectRatio aspectRatio = AspectRatio.Aspect16by9;
        PlayerSettings.SetAspectRatio(aspectRatio, true);
        PlayerSettings.productName = "RoboGames";
        PlayerSettings.companyName = "OnTechU";

        WebGLExceptionSupport web = WebGLExceptionSupport.FullWithoutStacktrace;
        PlayerSettings.WebGL.exceptionSupport = web;
        Application.runInBackground = true;

        BuildPlayerOptions buildPlayer = new BuildPlayerOptions();
        buildPlayer.scenes = new [] {"Assets/TitleScene.unity",
                            "Assets/TitleMenu.unity",
                            "Assets/StartScene.unity",
                            "Assets/Leaderboard.unity", 
                            "Assets/MainMenu.unity",
                            "Assets/newgame.unity",
                            "Assets/Cinematic.unity",
                            "Assets/CharacterSelect.unity",
                            "Assets/IntroScene.unity",
                            "Assets/Transition.unity",
                            "Assets/Credits.unity",
                            "Assets/TutorialDemo.unity",
                            "Assets/Progression.unity"};

        buildPlayer.locationPathName = BUILD_PATH;
        buildPlayer.target = BuildTarget.WebGL;
       // buildPlayer.options = (BuildOptions)web | (BuildOptions)compressionFormat;
        buildPlayer.options = (BuildOptions)web;
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayer);
        BuildSummary summary = report.summary;

        if(summary.result == BuildResult.Succeeded){
            //File.AppendAllText(@"/home/ibrahim/Desktop/RobotON/stdout1.log", "Build Succeed: " + summary.totalSize + " bytes, Date :" + System.DateTime.Now.ToString()+"\n");

        }else if(summary.result == BuildResult.Failed){
            //File.AppendAllText(@"/home/ibrahim/Desktop/RobotON/stdout1.log", "Build Failed: " + summary.totalSize + " bytes, Date :" + System.DateTime.Now.ToString()+"\n");
        }
    }

    static void FixIndexHtml(string pathToBuiltProject)
    {
        //Fetch filenames to be referenced in index.html
        string webglBuildUrl, webglLoaderUrl;
     
        if (File.Exists(Path.Combine(BUILD_PATH, "Build", "UnityLoader.js")))
        {
            webglLoaderUrl = "Build/UnityLoader.js";
        }
        else
        {
            webglLoaderUrl = "Build/UnityLoader.min.js";
        }
 
        string buildName = pathToBuiltProject.Substring(BUILD_PATH.LastIndexOf("/") + 1);
        webglBuildUrl = string.Format("Build/{0}.json", buildName);
 
        //webglLoaderUrl = EditorUserBuildSettings.development? "Build/UnityLoader.js": "Build/UnityLoader.min.js";
        Dictionary<string, string> replaceKeywordsMap = new Dictionary<string, string> {
                {
                    "%UNITY_WIDTH%",
                    PlayerSettings.defaultWebScreenWidth.ToString()
                },
                {
                    "%UNITY_HEIGHT%",
                    PlayerSettings.defaultWebScreenHeight.ToString()
                },
                {
                    "%UNITY_WEB_NAME%",
                    PlayerSettings.productName
                },
                {
                    "%UNITY_WEBGL_LOADER_URL%",
                    webglLoaderUrl
                },
                {
                    "%UNITY_WEBGL_BUILD_URL%",
                    webglBuildUrl
                }
            };
 
        string indexFilePath = Path.Combine(BUILD_PATH, "index.html");
       Func<string, KeyValuePair<string, string>, string> replaceFunction = (current, replace) => current.Replace(replace.Key, replace.Value);
        if (File.Exists(indexFilePath))
        {
           File.WriteAllText(indexFilePath, replaceKeywordsMap.Aggregate<KeyValuePair<string, string>, string>(File.ReadAllText(indexFilePath), replaceFunction));
        }
 
    }

    private class FileUtilExtended
    {
     
        internal static void CreateOrCleanDirectory(string dir)
        {
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }
            Directory.CreateDirectory(dir);
        }
 
        //Fix forward slashes on other platforms than windows
        internal static string FixForwardSlashes(string unityPath)
        {
            return ((Application.platform != RuntimePlatform.WindowsEditor) ? unityPath : unityPath.Replace("/", @"\"));
        }
 
 
 
        //Copies the contents of one directory to another.
       public static void CopyDirectoryFiltered(string source, string target, bool overwrite, string regExExcludeFilter, bool recursive)
        {
            RegexMatcher excluder = new RegexMatcher()
            {
                exclude = null
            };
            try
            {
                if (regExExcludeFilter != null)
                {
                    excluder.exclude = new Regex(regExExcludeFilter);
                }
            }
            catch (ArgumentException)
            {
               UnityEngine.Debug.Log("CopyDirectoryRecursive: Pattern '" + regExExcludeFilter + "' is not a correct Regular Expression. Not excluding any files.");
                return;
            }
            CopyDirectoryFiltered(source, target, overwrite, excluder.CheckInclude, recursive);
        }
       internal static void CopyDirectoryFiltered(string sourceDir, string targetDir, bool overwrite, Func<string, bool> filtercallback, bool recursive)
        {
            // Create directory if needed
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
                overwrite = false;
            }
 
            // Iterate all files, files that match filter are copied.
            foreach (string filepath in Directory.GetFiles(sourceDir))
            {
                if (filtercallback(filepath))
                {
                    string fileName = Path.GetFileName(filepath);
                    string to = Path.Combine(targetDir, fileName);
 
                 
                    File.Copy(FixForwardSlashes(filepath),FixForwardSlashes(to), overwrite);
                }
            }
 
            // Go into sub directories
            if (recursive)
            {
                foreach (string subdirectorypath in Directory.GetDirectories(sourceDir))
                {
                    if (filtercallback(subdirectorypath))
                    {
                        string directoryName = Path.GetFileName(subdirectorypath);
                       CopyDirectoryFiltered(Path.Combine(sourceDir, directoryName), Path.Combine(targetDir, directoryName), overwrite, filtercallback, recursive);
                    }
                }
            }
        }
 
        internal struct RegexMatcher
        {
            public Regex exclude;
            public bool CheckInclude(string s)
            {
                return exclude == null || !exclude.IsMatch(s);
            }
        }
 
    }

}
