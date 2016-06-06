//****************************************************************************//
// Class Name: stringLib
// Class Description: Contains all string literals used by the game.
// Author: Scott McLean
// Date Last Modified: 6/1/2016
//****************************************************************************//

using UnityEngine;

public static class stringLib
{
	public const string TOOL_LOGFILE		 	 = "toollog.txt";
	public const string TOOL_STATELOGFILE		 = "statelog.txt";
	public const string LOG_BREAKPOINT_ON	 	 = "BreakpointOn, ";
	public const string LOG_BREAKPOINT_ACTIVATED = "BreakpointActivated, ";
	public const string LOG_COMMENT_ON 			 = "Commented, ";
	public const string LOG_BUG_FOUND 			 = "BugCaught, ";
	public const string LOG_PRINTED				 = "Printed, ";
	public const string LOG_TOOL_WASTED			 = "Wasted, ";
	public const string LOG_WARPED				 = "Warped, ";

	public const string PROJECTILE_ACTIVATOR = "projectileActivator(Clone)";
	public const string PROJECTILE_BUG 		 = "projectileBug(Clone)";
	public const string PROJECTILE_WARP		 = "projectileWarp(Clone)";
	public const string PROJECTILE_COMMENT	 = "projectileComment(Clone)";
	public const string PROJECTILE_DEBUG	 = "projectileDebug(Clone)";

	public const string BAD_COMMENT_TEXT_COLOR_TAG 					= "<color=#00000000>";
	public const string BAD_UNCOMMENT_TEXT_COLOR_TAG_1 				= "<color=#ff0000ff>/*";
	public const string BAD_UNCOMMENT_TEXT_COLOR_TAG_2 				= "<color=#00000000>/*";
	public const string CHECKER_TEXT_COLOR_TAG 						= "<color=#ffff00ff>";

	public const string CHECKLIST_COMPLETE_COLOR_TAG    	        = "<color=#00ff00ff>";
	public const string CHECKLIST_INCOMPLETE_ACTIVATE_COLOR_TAG  	= "<color=#cccccccc>";
	public const string CHECKLIST_INCOMPLETE_CHECK_COLOR_TAG        = "<color=#ffff00ff>";
	public const string CHECKLIST_INCOMPLETE_NAME_COLOR_TAG         = "<color=#ff00ffff>";
	public const string CHECKLIST_INCOMPLETE_COMMENT_COLOR_TAG      = "<color=#00ff00ff>";
	public const string CHECKLIST_INCOMPLETE_UNCOMMENT_COLOR_TAG    = "<color=#ff0000ff>";

	// This one is used when the player successfully comments a block.
	public const string COMMENT_BLOCK_COLOR_TAG 					= "<color=#00ff00ff>/*";

	public const string MENU_SOUND_ON_COLOR_TAG						= "<color=#000000ff>";
	public const string MENU_SOUND_OFF_COLOR_TAG					= "<color=#ff0000ff>";

	public const string RENAME_COLOR_TAG							= "<color=#ff00ffff>";
	// This one is for the uncomment line after the player hits it with the uncomment tool.
	public const string UNCOMMENT_COLOR_TAG							= "<color=#ff0000ff>/*";

	public const string CLOSE_COLOR_TAG = "</color>";
	public const string COMMENT_CLOSE_COLOR_TAG = "*/</color>";

	public const string LOSE_TEXT     = "Try Again!\n";
	public const string RETRY_TEXT    = "Press Enter to try again\nor ESC to quit";
	public const string CONTINUE_TEXT = "Press Enter to continue.";

	public const string COMIC_LEVEL_ZERO = "tut1.xml";

	public const string GAME_MODE_ON = "on";
	public const string GAME_MODE_BUG = "bug";
	public const string GAME_ROBOT_ON = "Robot ON!";
	public const string GAME_ROBOT_BUG = "RoboBUG!";

	public const string GAME_LEVEL_ZERO = "tut1.xml";

	public const string CODENODE_NAME_CODE 				= "code";
	public const string CODENODE_NAME_DESCRIPTION		= "description";
	public const string CODENODE_NAME_TOOLS				= "tools";

	public const string NODE_NAME_TIME					= "time";
	public const string NODE_NAME_NEXT_LEVEL			= "nextlevel";
	public const string NODE_NAME_INTRO_TEXT			= "introtext";
	public const string NODE_NAME_END_TEXT				= "endtext";

	// Keywords used in XML tags
	public const string NODE_NAME_PRINT 				= "print";
	public const string NODE_NAME_WARP 					= "warp";
	public const string NODE_NAME_RENAME 				= "rename";
	public const string NODE_NAME_ON_CHECK 				= "oncheck";
	public const string NODE_NAME_UNCOMMENT 			= "uncom";
	public const string NODE_NAME_BAD_UNCOMMENT 		= "baduncom";
	public const string NODE_NAME_ON_COMMENT 			= "oncomment";
	public const string NODE_NAME_BAD_COMMENT 			= "badcomment";
	public const string NODE_NAME_COMMENT 				= "comment";
	public const string NODE_NAME_BUG					= "bug";
	public const string NODE_NAME_BEACON				= "becon";
	public const string NODE_NAME_BREAKPOINT			= "breakpoint";
	public const string NODE_NAME_PRIZE					= "prize";


	public const string NODE_COLOR_PRINT 				= "<color=#ffff00ff>";
	public const string NODE_COLOR_WARP 				= "<color=#ff00ffff>";
	public const string NODE_COLOR_RENAME 				= "<color=#ff00ffff>";
	public const string NODE_COLOR_ON_CHECK 			= "<color=#ffff00ff>";
	// When the level is generated, the correct uncomment choice is labelled with this one.
	public const string NODE_COLOR_UNCOMMENT 			= "<color=#ff0000ff>/*";
	// When the level is generated, the incorrect uncomment choice is labelled with this one.
	public const string NODE_COLOR_BAD_UNCOMMENT 		= "<color=#ff0000ff>/*";
	// When the level is generated, correct comment choice is labelled with this one.
	public const string NODE_COLOR_ON_COMMENT 			= "<color=#00ff00ff>/*";
	// When the level is generated, the incorrect/bad comment choice is labelled with this one.
	public const string NODE_COLOR_BAD_COMMENT		 	= "<color=#00ff00ff>/*";
	public const string NODE_COLOR_COMMENT				= "<color=#00ff00ff>/*";

	public const string SYNTAX_COLOR					= "<color=#00ff00ff>";



}
