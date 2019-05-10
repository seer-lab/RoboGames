using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Variables and Data that needs to be maintained between Scenes and large numbers of classes. 
/// </summary>
public static class GlobalState 
{
    public static string Character {get;set;}
    public static bool IsPlaying { get; set; }

    public static bool IsResume {get;set;}
    public static string CurrentONLevel { get; set; }
    public static string CurrentBUGLevel { get; set; }
    public static string GameMode { get; set; }
    public static int GameState { get; set; }
    public static stringLib StringLib { get; set; }
    public static bool IsDark { get; set; }
    public static string FilePath = (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) ? @"\" : @"/";
    public static Level level; 

}