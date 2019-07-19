//****************************************************************************//
// Class Name: stateLib
// Class Description: Contains all numeric game states and settings referenced in scripts.
// Author: Scott McLean
// Date Last Modified: 6/1/2016
//****************************************************************************//

using UnityEngine;

public static class stateLib
{
	public const float GAMESETTING_INITIAL_LINE_Y              = 3.5f;
	public const float GAMESETTING_LINE_SPACING                = 0.825f;
	
	public const float TOOLBOX_Y_OFFSET		                   = 0.1f;

	public const int NUMBER_OF_TOOLS                           = 7;

	public const int TOOL_CATCHER_OR_CONTROL_FLOW                 = 0;
	public const int TOOL_PRINTER_OR_QUESTION                  = 1;
	public const int TOOL_WARPER_OR_RENAMER                    = 2;
	public const int TOOL_COMMENTER                            = 3;
	public const int TOOL_UNCOMMENTER                         = 4;
	public const int TOOL_HELPER                               = 5;
	public const int TOOL_HINTER								=6;

	public const int GAMESTATE_SUBMENU                         = 0;
	public const int GAMESTATE_MENU                            = 0;
  public const int GAMESTATE_IN_GAME                         = 1;
	public const int GAMESTATE_LEVEL_START                     = 2;
	public const int GAMESTATE_LEVEL_WIN                       = 3;
	public const int GAMESTATE_LEVEL_LOSE                      = 4;
  public const int GAMESTATE_MENU_NEWGAME                    = -3;
  public const int GAMESTATE_MENU_LOADGAME                   = -4;
  public const int GAMESTATE_MENU_SOUNDOPTIONS               = -2;
  public const int GAMESTATE_MENU_LOADGAME_SUBMENU           = -1;
	// 5 through 9 are not used.
	public const int GAMESTATE_INITIAL_COMIC                   = 10;
	public const int GAMESTATE_STAGE_COMIC                     = 11;
	public const int GAMESTATE_GAME_END                        = 12;

	public const int GAMEMENU_NEW_GAME                         = 0;
	public const int GAMEMENU_LOAD_GAME                        = 1;
	public const int GAMEMENU_SOUND_OPTIONS                    = 2;
	public const int GAMEMENU_EXIT_GAME                        = 3;
	public const int GAMEMENU_RESUME_GAME                      = 4;

	public const int PROJECTILE_CODE_NO_TOOLS                  = -1;

	public const int ENTITY_TYPE_CORRECT_COMMENT               = 1;
	public const int ENTITY_TYPE_CORRECT_UNCOMMENT             = 2;
	public const int ENTITY_TYPE_INCORRECT_COMMENT             = 3;
	public const int ENTITY_TYPE_INCORRECT_UNCOMMENT           = 4;
	public const int ENTITY_TYPE_ROBOBUG_COMMENT               = 5;
	public const int TEXT_SIZE_VERY_LARGE			    						 = 40;
	public const int TEXT_SIZE_LARGE													 = 30;
	public const int TEXT_SIZE_NORMAL													 = 25;
	public const int TEXT_SIZE_SMALL													 = 20;


	public const int NUMBER_OF_OBSTACLE = 1;
	public const int OBSTACLE_FIREWALL = 0;
	public const int OUTPUT_ENTER = 0; 
	public const int OUTPUT_RIGHT = 1; 
	public const int OUTPUT_LEFT = 2; 
	public const int POINTS_COMMENT = 20; 
	public const int POINTS_UNCOMMENT = 40; 
	public const int POINTS_QUESTION = 40; 
	public const int POINTS_CATCHER = 100; 
	public const int POINTS_BEACON = 20; 
	public const int POINTS_RENAMER = 40; 
	public const int POINTS_CHECKER = 0; 
	public const int POINTS_WARPER = 0; 
	public const int POINTS_BREAKPOINT = 10; 

	public const int COST_SPEED = 200; 
	public const int COST_DAMAGE_REDUCE = 300; 
	public const int COST_HEALTH = 500; 
	
	}
