using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Variables and Data that needs to be maintained between Scenes and large numbers of classes. 
/// </summary>
public static class GlobalState
{
    public static int AdaptiveMode = 0; //0 = default, 1 = easier, 2 = easiest <-- Categorization of the player in ML
    public static int HintMode = 2; //0 = no hints, 1 = hints
    public static int failures = 0; //number of failures since a level was passed
    public static int tooluses = 0; //number of tool uses
    public static int elapsedTime = 0; //time elapsed since a level was passed
    public static int AdaptiveOffON = 0; //0 = no ML adaptive, 1= ML adaptive on
    public static int VerboseLoggingMode = 0; //0 = no verbose variables are logged, 1=verbose variables logged
    public static string Tech { get; set; } //description of technique
    public static string Hint1 { get; set; } //first hint for failure
    public static string Hint2 { get; set; } //second hint for failure
    public static bool RestrictGameMode = true;
    public static bool GamemodeON_BUG = false;
    public static bool LeaderBoardMode = false;
    public static bool LoggingMode = true;
    public static bool DebugMode = true;
    public static bool ObstacalMode = true;
    public static List<string> passed;
    public static string Character { get; set; }
    public static bool IsPlaying { get; set; }
    public static bool IsResume { get; set; }
    public static bool isPassed { get; set; }
    public static bool HideToolTips = false;
    public static string CurrentONLevel { get; set; }
    public static int CurrentLevelPoints { get; set; }
    public static int CurrentLevelEnergy { get; set; }
    public static int RunningScore = 0;
    public static string Language = "python";
    public static string CurrentBUGLevel { get; set; }
    public static string GameMode { get; set; }
    public static int GameState { get; set; }
    public static stringLib StringLib { get; set; }
    public static bool IsDark = true;
    public static int[] toolUse;
    public static int TextSize = 1;
    public static bool soundon = true;
    public static string FilePath = (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) ? @"\" : @"/";
    public static Level level;
    public static int timeBonus = 0;
    public static long sessionID { get; set; }
    public static int positionalID { get; set; }
    public static string currentLevelID { get; set; }
    public static string jsonStates { get; set; }
    public static string[] correctLine { get; set; }
    public static string[] obstacleLine { get; set; }
    public static string jsonOStates { get; set; }
    public static string bugLine { get; set; }
    public static CharacterStats Stats { get; set; }

    public static bool foundBug;
    public static int totalPoints { get; set; }
    public static int currentLevelStar { get; set; }
    public static int currentLevelTimeBonus { get; set; }

    public static float totalPointsCurrent { get; set; }
    public static string previousFilename { get; set; }
    public static string username { get; set; }
    public static string URL_MOVIE { get; set; }
    public static string URL_MOVIE_MENU { get; set; }
    public static string URL_MOVIE_BUG { get; set; }
    public static string URL_MOVIE_ON { get; set; }
    public static string courseCode { get; set; }

    public static int hitByEnemy = 0;
    public static int failedTool = 0;
}