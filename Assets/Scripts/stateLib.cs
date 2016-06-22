//****************************************************************************//
// Class Name: stateLib
// Class Description: Contains all game states referenced in scripts.
// Author: Scott McLean
// Date Last Modified: 6/1/2016
//****************************************************************************//

using UnityEngine;

public static class stateLib
{

	public const float GAMESETTING_INITIAL_LINE_Y 						= 3.5f;
	public const float GAMESETTING_LINE_SPACING 						= 0.825f;

	public const int NUMBER_OF_TOOLS 									= 6;

	public const int GAMESTATE_SUBMENU     								= 0;
	public const int GAMESTATE_MENU        								= 0;
	public const int GAMESTATE_IN_GAME 	   								= 1;
	public const int GAMESTATE_LEVEL_START 								= 2;
	public const int GAMESTATE_LEVEL_WIN   								= 3;
	public const int GAMESTATE_LEVEL_LOSE  								= 4;
	// 5 through 9 are not used.
	public const int GAMESTATE_INITIAL_COMIC 							= 10;
	public const int GAMESTATE_STAGE_COMIC   							= 11;
	public const int GAMESTATE_GAME_END      							= 12;

	public const int GAMEMENU_NEW_GAME									= 0;
	public const int GAMEMENU_LOAD_GAME		 							= 1;
	public const int GAMEMENU_SOUND_OPTIONS	 							= 2;
	public const int GAMEMENU_EXIT_GAME		 							= 3;
	public const int GAMEMENU_RESUME_GAME	 							= 4;

	public const int PROJECTILE_CODE_NO_TOOLS 							= -1;
}
