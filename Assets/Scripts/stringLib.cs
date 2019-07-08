//****************************************************************************//
// Class Name: stringLib
// Class Description: Library of string literals used across scripts. This allows
//                    the code to be more modular.
// Author: Scott McLean
// Date Last Modified: 6/1/2016
//****************************************************************************//

using UnityEngine;

public class stringLib
{

    public const string INTERFACE_SIDEBAR_AVAILABLE_TOOLS = "Available Tools:";
    public const string INTERFACE_SIDEBAR_OUT_OF_TOOLS = "Out of Tools!";
    public const string INTERFACE_TASK_COMPLETE = "Task Complete!";
    public const string INTERFACE_NEW_TOOLS = "New Tools!";

    public const string INTERFACE_TOOL_NAME_0_ROBOBUG = "Bug Fixer";
    public const string INTERFACE_TOOL_NAME_0_ROBOTON = "Beacon Activator";
    public const string INTERFACE_TOOL_NAME_1_ROBOBUG = "Printer";
    public const string INTERFACE_TOOL_NAME_1_ROBOTON = "Checker";
    public const string INTERFACE_TOOL_NAME_2_ROBOBUG = "Warper";
    public const string INTERFACE_TOOL_NAME_2_ROBOTON = "Renamer";
    public const string INTERFACE_TOOL_NAME_3 = "Commenter";
    public const string INTERFACE_TOOL_NAME_4_ROBOBUG = "Breakpointer";
    public const string INTERFACE_TOOL_NAME_4_ROBOTON = "Un-Commenter";
    public const string INTERFACE_TOOL_NAME_5 = "Helper";
    public const string INTERFACE_TOOL_NAME_6 = "Hinter";

    public const string TOOL_LOGFILE = "toollog.txt";
    public const string TOOL_STATELOGFILE = "statelog.txt";
    public const string LOG_BREAKPOINT_ON = "BreakpointOn, ";
    public const string LOG_BREAKPOINT_ACTIVATED = "BreakpointActivated, ";
    public const string LOG_COMMENT_ON = "Commented, ";
    public const string LOG_BUG_FOUND = "BugCaught, ";
    public const string LOG_PRINTED = "Printed, ";
    public const string LOG_TOOL_WASTED = "Wasted, ";
    public const string LOG_WARPED = "Warped, ";

    public const string PROJECTILE_BUG = "projectileBug(Clone)";
    public const string PROJECTILE_ACTIVATOR = "projectileActivator(Clone)";
    public const string PROJECTILE_WARP = "projectileWarp(Clone)";
    public const string PROJECTILE_COMMENT = "projectileComment(Clone)";
    public const string PROJECTILE_DEBUG = "projectileDebug(Clone)";
    public const string PROJECTILE_HINT = "projectileHint(Clone)";

    public const string LOSE_TEXT = "Try Again!\n";
    public const string RETRY_TEXT = "Press Enter to try again\nor ESC to quit";
    public const string CONTINUE_TEXT = "Press Enter to continue.";

    public const string GAME_MODE_ON = "on";
    public const string GAME_MODE_BUG = "bug";
    public const string GAME_ROBOT_ON = "Robot ON!";
    public const string GAME_ROBOT_BUG = "RoboBUG!";

    public const string TOOL_NAME_0_ROBOBUG = "catcher";
    public const string TOOL_NAME_0_ROBOTON = "activator";
    public const string TOOL_NAME_1_ROBOBUG = "printer";
    public const string TOOL_NAME_1_ROBOTON = "question";
    public const string TOOL_NAME_2_ROBOBUG = "warper";
    public const string TOOL_NAME_2_ROBOTON = "namer";
    public const string TOOL_NAME_3 = "commenter";
    public const string TOOL_NAME_4 = "uncommenter";
    public const string TOOL_NAME_5 = "helper";
    public const string TOOL_NAME_6 = "hinter";

    public const string OBSTACLE_FIREWALL = "Firewall";

    public const string CLOSE_COLOR_TAG = "</color>";

    public const string NODE_NAME_CODE = "code";
    public const string NODE_NAME_DESCRIPTION = "description";
    public const string NODE_NAME_TOOLS = "tools";
    public const string NODE_NAME_TOOL = "tool";
    public const string NODE_NAME_TIME = "timelimit";
    public const string NODE_NAME_NEXT_LEVEL = "next-level";
    public const string NODE_NAME_INTRO_TEXT = "introtext";
    public const string NODE_NAME_END_TEXT = "endtext";
    public const string NODE_NAME_PRINT = "print";
    public const string NODE_NAME_WARP = "warp";
    public const string NODE_NAME_RENAME = "variable-rename";
    public const string NODE_NAME_QUESTION = "question";
    public const string NODE_NAME_COMMENT = "comment";
    public const string NODE_NAME_BUG = "bug";
    public const string NODE_NAME_BEACON = "beacon";
    public const string NODE_NAME_BREAKPOINT = "breakpoint";
    public const string NODE_NAME_PRIZE = "prize";
    public const string NODE_NAME_VARIABLE_COLOR = "variable-color";
    public const string NODE_NAME_NEWLINE = "newline";
    public const string NODE_NAME_HINT = "hint";

    public const string XML_ATTRIBUTE_LANGUAGE = "language";
    public const string XML_ATTRIBUTE_TEXT = "text";
    public const string XML_ATTRIBUTE_TOOL = "tool";
    public const string XML_ATTRIBUTE_FILE = "file";
    public const string XML_ATTRIBUTE_LINE = "line";
    public const string XML_ATTRIBUTE_SIZE = "size";
    public const string XML_ATTRIBUTE_ROW = "row";
    public const string XML_ATTRIBUTE_COL = "col";
    public const string XML_ATTRIBUTE_GROUPID = "groupid";
    public const string XML_ATTRIBUTE_TYPE = "type";
    public const string XML_ATTRIBUTE_CORRECT = "correct";
    public const string XML_ATTRIBUTE_ANSWER = "answer";
public const string XML_ATTRIBUTE_OPTIONS = "options";
    public const string XML_ATTRIBUTE_BONUSES = "bonuses";
    public const string XML_ATTRIBUTE_FLOWORDER = "flow-order";
    public const string XML_ATTRIBUTE_NAME = "name";
    public const string XML_ATTRIBUTE_COUNT = "count";
    public const string XML_ATTRIBUTE_ENABLED = "enabled";
    public const string XML_ATTRIBUTE_COMMENT_STYLE = "style";
    public const string XML_ATTRIBUTE_OLDNAME = "oldname";
    public const string XML_ATTRIBUTE_HINT = "hint";

    public Color[] COLORS = new Color[]{Color.white, Color.yellow, Color.magenta, Color.green,
        Color.cyan, Color.blue, Color.blue};
    public string[] namesON = new string[] { "Activator", "Question", "Renamer", "Commenter", "Uncommenter", "Helper", "Hinter" };
    public string[] namesBug = new string[] { "Bug Fixer", "Printer", "Warper", "Commenter", "Breakpointer", "Helper", "Hinter" };
    public const string START_LEVEL_FILE = "level0-1.xml";

    public string comic_level_zero = "tut1.xml";
    public string game_level_zero = "tut1.xml";
    //REFACTOR: Check that this isn't used anywhere and replace


    public string question_text_color_tag = "<color=#ffff00ff>";

    public string checklist_complete_color_tag = "<color=#00ff00ff>";
    public string checklist_incomplete_activate_color_tag = "<color=#cccccccc>";
    public string checklist_incomplete_question_color_tag = "<color=#ffff00ff>";
    public string checklist_incomplete_name_color_tag = "<color=#ff00ffff>";
    public string checklist_incomplete_comment_color_tag = "<color=#00ff00ff>";
    public string checklist_incomplete_uncomment_color_tag = "<color=#ff0000ff>";

    public string checklist_complete_color_tag_light = "<color=#00ff00ff>";
    public string checklist_incomplete_activate_color_tag_light = "<color=#cccccccc>";
    public string checklist_incomplete_question_color_tag_light = "<color=#ffff00ff>";
    public string checklist_incomplete_name_color_tag_light = "<color=#ff00ffff>";
    public string checklist_incomplete_comment_color_tag_light = "<color=#00ff00ff>";
    public string checklist_incomplete_uncomment_color_tag_light = "<color=#ff0000ff>";

    public string checklist_complete_color_tag_dark = "<color=#009933ff>";
    public string checklist_incomplete_activate_color_tag_dark = "<color=#737373ff>";
    public string checklist_incomplete_question_color_tag_dark = "<color=#996633ff>";
    public string checklist_incomplete_name_color_tag_dark = "<color=#cc00ccff>";
    public string checklist_incomplete_comment_color_tag_dark = "<color=#009933ff>";
    public string checklist_incomplete_uncomment_color_tag_dark = "<color=#ff0000ff>";

    public string comment_block_color_tag = "<color=#00ff00ff>";
    public string menu_sound_on_color_tag = "<color=#000000ff>";
    public string menu_sound_off_color_tag = "<color=#ff0000ff>";

    public string node_color_print = "<color=#ffff00ff>";
    public string node_color_question = "<color=#ffff00ff>";
    public string node_color_warp = "<color=#ff00ffff>";
    public string node_color_rename = "<color=#ff00ffff>";
    public string node_color_uncomment = "<color=#ff0000ff>";
    public string node_color_incorrect_uncomment = "<color=#ff0000ff>";
    public string node_color_correct_comment = "<color=#00ff00ff>";
    public string node_color_incorrect_comment = "<color=#00ff00ff>";
    public string node_color_comment = "<color=#00ff00ff>";
    public string node_color_print_light = "<color=#ffff00ff>";
    public string node_color_question_light = "<color=#ffff00ff>";
    public string node_color_warp_light = "<color=#ff00ffff>";
    public string node_color_rename_light = "<color=#ff00ffff>";
    public string node_color_uncomment_light = "<color=#ff0000ff>";
    public string node_color_incorrect_uncomment_light = "<color=#ff0000ff>";
    public string node_color_correct_comment_light = "<color=#00ff00ff>";
    public string node_color_incorrect_comment_light = "<color=#00ff00ff>";
    public string node_color_comment_light = "<color=#00ff00ff>";
    public string node_color_print_dark = "<color=#996633ff>";
    public string node_color_question_dark = "<color=#996633ff>";
    public string node_color_warp_dark = "<color=#cc00ccff>";
    public string node_color_rename_dark = "<color=#cc00ccff>";
    public string node_color_uncomment_dark = "<color=#ff0000ff>";
    public string node_color_incorrect_uncomment_dark = "<color=#ff0000ff>";
    public string node_color_correct_comment_dark = "<color=#009933ff>";
    public string node_color_incorrect_comment_dark = "<color=#009933ff>";
    public string node_color_comment_dark = "<color=#009933ff>";

    public string syntax_color_comment = "<color=#00ff00ff>";
    public string syntax_color_keyword = "<color=#00ffffff>";
    public string syntax_color_badcomment = "<color=#00ff00ff>";
    public string syntax_color_string = "<color=#ffa500ff>";

    public string syntax_color_comment_light = "<color=#00ff00ff>";
    public string syntax_color_keyword_light = "<color=#00ffffff>";
    public string syntax_color_badcomment_light = "<color=#00ff00ff>";
    public string syntax_color_string_light = "<color=#ffa500ff>";

    public string syntax_color_comment_dark = "<color=#009933ff>";
    public string syntax_color_keyword_dark = "<color=#0000ffff>";
    public string syntax_color_badcomment_dark = "<color=#006600ff>";
    public string syntax_color_string_dark = "<color=#ff3300ff>";

    public string syntax_color_include = "<color=#e6005cff>";
    public float LEFT_CODESCREEN_X_COORDINATE = -10;

    public const string SERVER_URL = "http://199.212.33.5:8080/";
    public const string DB_URL = "http://199.212.33.5:3000/logs";
    public const int DOWNLOAD_TIME = 200;

    public string[] nameObstacle = new string[] { "firewall" };
    public const string STREAMING_ASSETS = "StreamingAssets/";
    public const string ASSETS_BUNDLE = "AssetBundles/";
    public const string VIDEO_FILE = "videofiles";
    public const string MOVIE_INTRO = "TitleSequence.mp4";
    public const string MOVIE_INTRO_MENU = "Menu.mp4";
    public const string MOVIE_ON = "IntroScene.mp4";
    public const string MOVIE_BUG = "RoboBugIntro_1.mp4";

}
