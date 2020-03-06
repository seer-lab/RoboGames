using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Xml;
using System.IO;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

/// <summary>
/// Oversees the Game Logic such as winning, losing, picking which level to Load. 
/// </summary>
public class GameController : MonoBehaviour, ITimeUser
{
    LevelFactory factory;
    LevelGenerator lg;
    Output output;
    SidebarController sidebar;
    SelectedTool selectedTool;
    BackgroundController background;
    BackButton backButton;
    bool calledDead = false;
    GameObject hero;
    EnergyController EnergyController;
    DateTime startDate, endDate;
    bool firstUpdate = true;

    public Logger logger;
    bool winning = false;
    bool finalized = false;
    float leftCodescreen;

    bool hasCalled = false; 

    /// <summary>
    /// Checks if the game meets the win condition. 
    /// </summary>
    private void CheckWin()
    {
        if (GlobalState.level != null)
        {
            winning = true;
            //Check if all the tasks have been completed. 
            for (int i = 0; i < 5; i++)
            {
                if (GlobalState.level.Tasks[i] != GlobalState.level.CompletedTasks[i])
                {
                    winning = false;

                }
            }
            if (GlobalState.level.Tasks[0] != 0 && GlobalState.level.Tasks[0] == GlobalState.level.CompletedTasks[0] && GlobalState.GameMode == stringLib.GAME_MODE_BUG)
                winning = true;
            if (winning)
            {
                finalized = true; 
                StopCoroutine(Lose());
                StartCoroutine(Win());
            }
        }
    }
    /// <summary>
    /// Checks if the Game meets the Lose Condition
    /// </summary>
    private void CheckLose()
    {
        if (EnergyController.currentEnergy <= 0)
        {
            StartCoroutine(Lose());
        }
    }
    /// <summary>
    /// ITimeUser Callback function
    /// </summary>
    public void OnTimeFinish()
    {
        GameOver();
    }

    /// <summary>
    /// Initialize the Timer with the Level's time. 
    /// </summary>
    /// <param name="time"></param>
    private void LoadTimer(float time)
    {
        TimeDisplayController controller = sidebar.timer.GetComponent<TimeDisplayController>();
        controller.EndTime = time;
        controller.Callback = this;
    }

    /// <summary>
    /// Switch to the GAME_END state and load the credits scene. 
    /// </summary>
    public void Victory()
    {
        //logger.onGameEnd(); 
        GlobalState.IsPlaying = false;
        GlobalState.CurrentONLevel = "level5";
        GlobalState.GameState = stateLib.GAMESTATE_GAME_END;
        SceneManager.LoadScene("IntroScene");
    }

    /// <summary>
    /// Switches to the Level Lose and Loads the Cinematic Scene. 
    /// </summary>
    public void GameOver()
    {
        GlobalState.IsPlaying = false;
        GlobalState.GameState = stateLib.GAMESTATE_LEVEL_LOSE;
        GlobalState.level.NextLevel = GlobalState.level.Failure_Level;
        //logger.onGameEnd(startDate, false);
        SceneManager.LoadScene("Cinematic");
    }
    /// <summary>
    /// Delays the Lose Operation to ensure the player isn't about to win 
    /// and then ends the game.
    /// </summary>
    /// <returns></returns>
    IEnumerator Lose()
    {
        CheckWin();

        do
        {
            yield return new WaitForSecondsRealtime(2.7f);
        } while (GlobalState.GameState != stateLib.GAMESTATE_IN_GAME || output.Text.text != "");
        if (!winning && !finalized)
        {
            //Glitch all of the code wen the player loses by messig up with the 
            // Bug Fixer. (This Solution is based on UNITY bugging out when switching fonts quickly fyi)
            if (GlobalState.GameMode == stringLib.GAME_MODE_BUG){
                TextMesh text = GameObject.Find("Code").GetComponent<TextMesh>(); 
                text.font = Resources.Load<Font>("Fonts/HACKED"); 
                yield return new WaitForSeconds(0.12f); 
                text.font = Resources.Load<Font>("Fonts/CFGlitchCity-Regular"); 
                yield return new WaitForSeconds(0.12f); 
                text.font = Resources.Load<Font>("Fonts/HACKED"); 
                yield return new WaitForSeconds(0.1f); 
                text.text = ""; 
                
            }
            if (!calledDead)
            {
                hero.GetComponent<Animator>().SetTrigger("Dead");
                calledDead = true;
				
				GlobalState.failures++;	//ADAPTIVE CODE
				int time = DateTime.Now.Subtract(startDate).Seconds;
				GlobalState.elapsedTime += time;
            }
            yield return new WaitForSecondsRealtime(1.5f);
            GameObject.Find("Fade").GetComponent<Fade>().onFadeOut();
            GameOver();
        }
    }
    int CalculateTimeBonus(){
        int time = DateTime.Now.Subtract(startDate).Seconds;
        int value = GlobalState.level.Code.Length*stateLib.POINTS_TIME; 

        value = value - time*stateLib.TIME_DEDUCTION; 

        if (value < 0) value = 0; 
        return value; 
    }
    /// <summary>
    /// Delays the Win Operation to ensure the player actually won, then 
    /// completes win game operations. 
    /// </summary>
    /// <returns></returns>
    IEnumerator Win()
    {
        logger.onGameEnd(startDate, true, EnergyController.currentEnergy);
        GlobalState.previousFilename = GlobalState.CurrentONLevel;
        GlobalState.timeBonus = logger.CalculateTimeBonus();
        GlobalState.timeBonus = CalculateTimeBonus();
        GlobalState.currentLevelTimeBonus = GlobalState.timeBonus;
        GlobalState.CurrentLevelEnergy = (int)EnergyController.currentEnergy; 
        if (GlobalState.GameMode == stringLib.GAME_MODE_BUG){
            GlobalState.CurrentLevelPoints = stateLib.DEFAULT_BUG_POINTS; 
        }
        GlobalState.RunningScore+= GlobalState.CurrentLevelPoints; 
        GlobalState.isPassed = true;
        //logger.sendPoints();
        do
        {
            yield return new WaitForSecondsRealtime(2.2f);
        } while (GlobalState.GameState != stateLib.GAMESTATE_IN_GAME);
        while(output.Text.text != "" && !GlobalState.level.IsDemo) yield return new WaitForSecondsRealtime(0.5f); 
        //if the end of the level string is empty then there is no anticipated next level. 
        Debug.Log("Next Level: " + GlobalState.level.NextLevel);
        if (GlobalState.level.NextLevel != "")
        {
            GlobalState.GameState = stateLib.GAMESTATE_LEVEL_WIN;
			
			
			//ADAPTIVE CODE: In desperate need of refactoring :( 
			
			string [] dataNames = {"level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1a.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level1b.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2a.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level2b.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3a.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level3b.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4a.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4b.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml","level4c.xml"};
			 
			double[][] rawData = new double[647][];
rawData[0] = new double[] {62.0,0.0};
rawData[1] = new double[] {124.0,0.0};
rawData[2] = new double[] {64.0,0.0};
rawData[3] = new double[] {91.0,0.0};
rawData[4] = new double[] {61.0,0.0};
rawData[5] = new double[] {226.0,0.0};
rawData[6] = new double[] {130.0,0.0};
rawData[7] = new double[] {129.0,0.0};
rawData[8] = new double[] {60.0,0.0};
rawData[9] = new double[] {144.0,0.0};
rawData[10] = new double[] {74.0,0.0};
rawData[11] = new double[] {134.0,1.0};
rawData[12] = new double[] {120.0,0.0};
rawData[13] = new double[] {60.0,0.0};
rawData[14] = new double[] {251.0,0.0};
rawData[15] = new double[] {75.0,0.0};
rawData[16] = new double[] {68.0,0.0};
rawData[17] = new double[] {34.0,0.0};
rawData[18] = new double[] {134.0,1.0};
rawData[19] = new double[] {26.0,0.0};
rawData[20] = new double[] {121.0,0.0};
rawData[21] = new double[] {130.0,0.0};
rawData[22] = new double[] {212.0,1.0};
rawData[23] = new double[] {449.0,1.0};
rawData[24] = new double[] {75.0,0.0};
rawData[25] = new double[] {125.0,0.0};
rawData[26] = new double[] {10.0,0.0};
rawData[27] = new double[] {131.0,0.0};
rawData[28] = new double[] {127.0,0.0};
rawData[29] = new double[] {117.0,0.0};
rawData[30] = new double[] {124.0,0.0};
rawData[31] = new double[] {67.0,0.0};
rawData[32] = new double[] {57.0,0.0};
rawData[33] = new double[] {124.0,0.0};
rawData[34] = new double[] {5.0,0.0};
rawData[35] = new double[] {122.0,0.0};
rawData[36] = new double[] {150.0,1.0};
rawData[37] = new double[] {123.0,0.0};
rawData[38] = new double[] {129.0,0.0};
rawData[39] = new double[] {140.0,0.0};
rawData[40] = new double[] {122.0,0.0};
rawData[41] = new double[] {128.0,0.0};
rawData[42] = new double[] {127.0,0.0};
rawData[43] = new double[] {140.0,0.0};
rawData[44] = new double[] {67.0,0.0};
rawData[45] = new double[] {124.0,0.0};
rawData[46] = new double[] {140.0,0.0};
rawData[47] = new double[] {14.0,0.0};
rawData[48] = new double[] {17.0,0.0};
rawData[49] = new double[] {127.0,0.0};
rawData[50] = new double[] {66.0,0.0};
rawData[51] = new double[] {48.0,0.0};
rawData[52] = new double[] {78.0,1.0};
rawData[53] = new double[] {134.0,0.0};
rawData[54] = new double[] {23.0,0.0};
rawData[55] = new double[] {72.0,1.0};
rawData[56] = new double[] {63.0,0.0};
rawData[57] = new double[] {69.0,0.0};
rawData[58] = new double[] {124.0,0.0};
rawData[59] = new double[] {134.0,0.0};
rawData[60] = new double[] {70.0,1.0};
rawData[61] = new double[] {135.0,2.0};
rawData[62] = new double[] {128.0,0.0};
rawData[63] = new double[] {259.0,1.0};
rawData[64] = new double[] {6.0,0.0};
rawData[65] = new double[] {201.0,1.0};
rawData[66] = new double[] {69.0,0.0};
rawData[67] = new double[] {125.0,0.0};
rawData[68] = new double[] {127.0,0.0};
rawData[69] = new double[] {63.0,0.0};
rawData[70] = new double[] {194.0,1.0};
rawData[71] = new double[] {63.0,0.0};
rawData[72] = new double[] {12.0,0.0};
rawData[73] = new double[] {28.0,1.0};
rawData[74] = new double[] {62.0,0.0};
rawData[75] = new double[] {13.0,0.0};
rawData[76] = new double[] {186.0,1.0};
rawData[77] = new double[] {127.0,1.0};
rawData[78] = new double[] {7.0,0.0};
rawData[79] = new double[] {87.0,0.0};
rawData[80] = new double[] {34.0,0.0};
rawData[81] = new double[] {71.0,0.0};
rawData[82] = new double[] {66.0,0.0};
rawData[83] = new double[] {260.0,2.0};
rawData[84] = new double[] {131.0,0.0};
rawData[85] = new double[] {134.0,0.0};
rawData[86] = new double[] {62.0,0.0};
rawData[87] = new double[] {134.0,1.0};
rawData[88] = new double[] {123.0,0.0};
rawData[89] = new double[] {8.0,0.0};
rawData[90] = new double[] {279.0,1.0};
rawData[91] = new double[] {76.0,0.0};
rawData[92] = new double[] {8.0,0.0};
rawData[93] = new double[] {157.0,1.0};
rawData[94] = new double[] {196.0,0.0};
rawData[95] = new double[] {150.0,0.0};
rawData[96] = new double[] {57.0,0.0};
rawData[97] = new double[] {195.0,1.0};
rawData[98] = new double[] {163.0,0.0};
rawData[99] = new double[] {67.0,0.0};
rawData[100] = new double[] {71.0,0.0};
rawData[101] = new double[] {15.0,0.0};
rawData[102] = new double[] {139.0,2.0};
rawData[103] = new double[] {382.0,3.0};
rawData[104] = new double[] {141.0,0.0};
rawData[105] = new double[] {60.0,0.0};
rawData[106] = new double[] {72.0,0.0};
rawData[107] = new double[] {10.0,0.0};
rawData[108] = new double[] {66.0,0.0};
rawData[109] = new double[] {75.0,1.0};
rawData[110] = new double[] {127.0,0.0};
rawData[111] = new double[] {125.0,0.0};
rawData[112] = new double[] {135.0,1.0};
rawData[113] = new double[] {123.0,0.0};
rawData[114] = new double[] {77.0,1.0};
rawData[115] = new double[] {10.0,0.0};
rawData[116] = new double[] {261.0,2.0};
rawData[117] = new double[] {133.0,1.0};
rawData[118] = new double[] {140.0,1.0};
rawData[119] = new double[] {265.0,1.0};
rawData[120] = new double[] {16.0,0.0};
rawData[121] = new double[] {147.0,1.0};
rawData[122] = new double[] {118.0,0.0};
rawData[123] = new double[] {250.0,1.0};
rawData[124] = new double[] {123.0,0.0};
rawData[125] = new double[] {68.0,1.0};
rawData[126] = new double[] {72.0,0.0};
rawData[127] = new double[] {144.0,2.0};
rawData[128] = new double[] {263.0,2.0};
rawData[129] = new double[] {65.0,0.0};
rawData[130] = new double[] {68.0,0.0};
rawData[131] = new double[] {63.0,0.0};
rawData[132] = new double[] {126.0,0.0};
rawData[133] = new double[] {26.0,0.0};
rawData[134] = new double[] {65.0,0.0};
rawData[135] = new double[] {138.0,1.0};
rawData[136] = new double[] {135.0,1.0};
rawData[137] = new double[] {73.0,0.0};
rawData[138] = new double[] {7.0,0.0};
rawData[139] = new double[] {138.0,1.0};
rawData[140] = new double[] {252.0,1.0};
rawData[141] = new double[] {123.0,0.0};
rawData[142] = new double[] {129.0,0.0};
rawData[143] = new double[] {70.0,0.0};
rawData[144] = new double[] {182.0,1.0};
rawData[145] = new double[] {73.0,0.0};
rawData[146] = new double[] {63.0,0.0};
rawData[147] = new double[] {72.0,1.0};
rawData[148] = new double[] {24.0,2.0};
rawData[149] = new double[] {92.0,0.0};
rawData[150] = new double[] {76.0,0.0};
rawData[151] = new double[] {34.0,0.0};
rawData[152] = new double[] {61.0,0.0};
rawData[153] = new double[] {67.0,0.0};
rawData[154] = new double[] {132.0,0.0};
rawData[155] = new double[] {122.0,0.0};
rawData[156] = new double[] {315.0,0.0};
rawData[157] = new double[] {8.0,0.0};
rawData[158] = new double[] {197.0,1.0};
rawData[159] = new double[] {9.0,0.0};
rawData[160] = new double[] {136.0,0.0};
rawData[161] = new double[] {65.0,0.0};
rawData[162] = new double[] {126.0,0.0};
rawData[163] = new double[] {74.0,0.0};
rawData[164] = new double[] {279.0,1.0};
rawData[165] = new double[] {278.0,0.0};
rawData[166] = new double[] {130.0,0.0};
rawData[167] = new double[] {122.0,0.0};
rawData[168] = new double[] {153.0,0.0};
rawData[169] = new double[] {11.0,1.0};
rawData[170] = new double[] {16.0,0.0};
rawData[171] = new double[] {151.0,0.0};
rawData[172] = new double[] {187.0,1.0};
rawData[173] = new double[] {62.0,0.0};
rawData[174] = new double[] {188.0,1.0};
rawData[175] = new double[] {128.0,0.0};
rawData[176] = new double[] {128.0,0.0};
rawData[177] = new double[] {64.0,0.0};
rawData[178] = new double[] {309.0,1.0};
rawData[179] = new double[] {69.0,0.0};
rawData[180] = new double[] {123.0,0.0};
rawData[181] = new double[] {126.0,0.0};
rawData[182] = new double[] {201.0,2.0};
rawData[183] = new double[] {125.0,0.0};
rawData[184] = new double[] {126.0,0.0};
rawData[185] = new double[] {260.0,2.0};
rawData[186] = new double[] {131.0,0.0};
rawData[187] = new double[] {23.0,0.0};
rawData[188] = new double[] {134.0,0.0};
rawData[189] = new double[] {151.0,0.0};
rawData[190] = new double[] {6.0,0.0};
rawData[191] = new double[] {91.0,1.0};
rawData[192] = new double[] {69.0,0.0};
rawData[193] = new double[] {65.0,0.0};
rawData[194] = new double[] {267.0,3.0};
rawData[195] = new double[] {256.0,4.0};
rawData[196] = new double[] {152.0,1.0};
rawData[197] = new double[] {77.0,0.0};
rawData[198] = new double[] {191.0,1.0};
rawData[199] = new double[] {395.0,2.0};
rawData[200] = new double[] {327.0,1.0};
rawData[201] = new double[] {617.0,1.0};
rawData[202] = new double[] {294.0,1.0};
rawData[203] = new double[] {122.0,0.0};
rawData[204] = new double[] {126.0,0.0};
rawData[205] = new double[] {137.0,0.0};
rawData[206] = new double[] {121.0,0.0};
rawData[207] = new double[] {262.0,1.0};
rawData[208] = new double[] {129.0,0.0};
rawData[209] = new double[] {125.0,0.0};
rawData[210] = new double[] {147.0,0.0};
rawData[211] = new double[] {109.0,0.0};
rawData[212] = new double[] {571.0,2.0};
rawData[213] = new double[] {156.0,0.0};
rawData[214] = new double[] {194.0,0.0};
rawData[215] = new double[] {189.0,0.0};
rawData[216] = new double[] {144.0,0.0};
rawData[217] = new double[] {140.0,0.0};
rawData[218] = new double[] {371.0,3.0};
rawData[219] = new double[] {829.0,1.0};
rawData[220] = new double[] {238.0,0.0};
rawData[221] = new double[] {126.0,0.0};
rawData[222] = new double[] {78.0,0.0};
rawData[223] = new double[] {143.0,0.0};
rawData[224] = new double[] {409.0,1.0};
rawData[225] = new double[] {62.0,0.0};
rawData[226] = new double[] {252.0,1.0};
rawData[227] = new double[] {446.0,2.0};
rawData[228] = new double[] {124.0,0.0};
rawData[229] = new double[] {83.0,0.0};
rawData[230] = new double[] {457.0,1.0};
rawData[231] = new double[] {187.0,0.0};
rawData[232] = new double[] {202.0,0.0};
rawData[233] = new double[] {141.0,0.0};
rawData[234] = new double[] {68.0,0.0};
rawData[235] = new double[] {126.0,0.0};
rawData[236] = new double[] {154.0,0.0};
rawData[237] = new double[] {218.0,1.0};
rawData[238] = new double[] {454.0,0.0};
rawData[239] = new double[] {123.0,0.0};
rawData[240] = new double[] {338.0,0.0};
rawData[241] = new double[] {81.0,0.0};
rawData[242] = new double[] {22.0,0.0};
rawData[243] = new double[] {125.0,0.0};
rawData[244] = new double[] {147.0,0.0};
rawData[245] = new double[] {247.0,1.0};
rawData[246] = new double[] {124.0,0.0};
rawData[247] = new double[] {215.0,2.0};
rawData[248] = new double[] {314.0,2.0};
rawData[249] = new double[] {63.0,0.0};
rawData[250] = new double[] {146.0,1.0};
rawData[251] = new double[] {187.0,1.0};
rawData[252] = new double[] {188.0,0.0};
rawData[253] = new double[] {210.0,1.0};
rawData[254] = new double[] {86.0,0.0};
rawData[255] = new double[] {134.0,0.0};
rawData[256] = new double[] {64.0,0.0};
rawData[257] = new double[] {77.0,0.0};
rawData[258] = new double[] {136.0,0.0};
rawData[259] = new double[] {93.0,0.0};
rawData[260] = new double[] {134.0,0.0};
rawData[261] = new double[] {160.0,0.0};
rawData[262] = new double[] {112.0,0.0};
rawData[263] = new double[] {282.0,2.0};
rawData[264] = new double[] {160.0,1.0};
rawData[265] = new double[] {218.0,1.0};
rawData[266] = new double[] {161.0,0.0};
rawData[267] = new double[] {133.0,0.0};
rawData[268] = new double[] {126.0,0.0};
rawData[269] = new double[] {64.0,0.0};
rawData[270] = new double[] {122.0,0.0};
rawData[271] = new double[] {129.0,0.0};
rawData[272] = new double[] {22.0,0.0};
rawData[273] = new double[] {64.0,0.0};
rawData[274] = new double[] {150.0,0.0};
rawData[275] = new double[] {68.0,0.0};
rawData[276] = new double[] {73.0,0.0};
rawData[277] = new double[] {12.0,0.0};
rawData[278] = new double[] {70.0,0.0};
rawData[279] = new double[] {164.0,1.0};
rawData[280] = new double[] {124.0,0.0};
rawData[281] = new double[] {16.0,0.0};
rawData[282] = new double[] {131.0,0.0};
rawData[283] = new double[] {67.0,0.0};
rawData[284] = new double[] {178.0,0.0};
rawData[285] = new double[] {74.0,0.0};
rawData[286] = new double[] {149.0,0.0};
rawData[287] = new double[] {38.0,0.0};
rawData[288] = new double[] {320.0,2.0};
rawData[289] = new double[] {153.0,0.0};
rawData[290] = new double[] {127.0,0.0};
rawData[291] = new double[] {62.0,0.0};
rawData[292] = new double[] {61.0,0.0};
rawData[293] = new double[] {197.0,1.0};
rawData[294] = new double[] {68.0,0.0};
rawData[295] = new double[] {363.0,1.0};
rawData[296] = new double[] {345.0,1.0};
rawData[297] = new double[] {476.0,2.0};
rawData[298] = new double[] {91.0,0.0};
rawData[299] = new double[] {65.0,0.0};
rawData[300] = new double[] {130.0,0.0};
rawData[301] = new double[] {65.0,0.0};
rawData[302] = new double[] {63.0,0.0};
rawData[303] = new double[] {125.0,0.0};
rawData[304] = new double[] {83.0,0.0};
rawData[305] = new double[] {138.0,0.0};
rawData[306] = new double[] {151.0,0.0};
rawData[307] = new double[] {118.0,0.0};
rawData[308] = new double[] {132.0,0.0};
rawData[309] = new double[] {128.0,0.0};
rawData[310] = new double[] {140.0,0.0};
rawData[311] = new double[] {31.0,0.0};
rawData[312] = new double[] {996.0,2.0};
rawData[313] = new double[] {173.0,0.0};
rawData[314] = new double[] {193.0,1.0};
rawData[315] = new double[] {378.0,2.0};
rawData[316] = new double[] {87.0,0.0};
rawData[317] = new double[] {301.0,1.0};
rawData[318] = new double[] {214.0,0.0};
rawData[319] = new double[] {132.0,0.0};
rawData[320] = new double[] {62.0,0.0};
rawData[321] = new double[] {296.0,1.0};
rawData[322] = new double[] {126.0,0.0};
rawData[323] = new double[] {216.0,1.0};
rawData[324] = new double[] {117.0,0.0};
rawData[325] = new double[] {158.0,0.0};
rawData[326] = new double[] {165.0,1.0};
rawData[327] = new double[] {87.0,0.0};
rawData[328] = new double[] {18.0,0.0};
rawData[329] = new double[] {147.0,1.0};
rawData[330] = new double[] {226.0,1.0};
rawData[331] = new double[] {328.0,2.0};
rawData[332] = new double[] {70.0,0.0};
rawData[333] = new double[] {456.0,1.0};
rawData[334] = new double[] {159.0,0.0};
rawData[335] = new double[] {126.0,0.0};
rawData[336] = new double[] {125.0,0.0};
rawData[337] = new double[] {69.0,0.0};
rawData[338] = new double[] {152.0,1.0};
rawData[339] = new double[] {218.0,1.0};
rawData[340] = new double[] {767.0,1.0};
rawData[341] = new double[] {118.0,0.0};
rawData[342] = new double[] {221.0,0.0};
rawData[343] = new double[] {229.0,0.0};
rawData[344] = new double[] {107.0,0.0};
rawData[345] = new double[] {129.0,0.0};
rawData[346] = new double[] {128.0,0.0};
rawData[347] = new double[] {96.0,0.0};
rawData[348] = new double[] {258.0,0.0};
rawData[349] = new double[] {125.0,0.0};
rawData[350] = new double[] {358.0,0.0};
rawData[351] = new double[] {126.0,0.0};
rawData[352] = new double[] {190.0,0.0};
rawData[353] = new double[] {157.0,0.0};
rawData[354] = new double[] {109.0,0.0};
rawData[355] = new double[] {114.0,0.0};
rawData[356] = new double[] {195.0,0.0};
rawData[357] = new double[] {118.0,0.0};
rawData[358] = new double[] {154.0,0.0};
rawData[359] = new double[] {111.0,0.0};
rawData[360] = new double[] {149.0,0.0};
rawData[361] = new double[] {448.0,0.0};
rawData[362] = new double[] {127.0,0.0};
rawData[363] = new double[] {160.0,0.0};
rawData[364] = new double[] {141.0,0.0};
rawData[365] = new double[] {315.0,0.0};
rawData[366] = new double[] {138.0,0.0};
rawData[367] = new double[] {112.0,0.0};
rawData[368] = new double[] {122.0,0.0};
rawData[369] = new double[] {127.0,0.0};
rawData[370] = new double[] {36.0,0.0};
rawData[371] = new double[] {197.0,0.0};
rawData[372] = new double[] {249.0,1.0};
rawData[373] = new double[] {88.0,0.0};
rawData[374] = new double[] {197.0,0.0};
rawData[375] = new double[] {85.0,0.0};
rawData[376] = new double[] {143.0,0.0};
rawData[377] = new double[] {273.0,1.0};
rawData[378] = new double[] {681.0,6.0};
rawData[379] = new double[] {204.0,0.0};
rawData[380] = new double[] {309.0,2.0};
rawData[381] = new double[] {358.0,2.0};
rawData[382] = new double[] {115.0,0.0};
rawData[383] = new double[] {407.0,0.0};
rawData[384] = new double[] {127.0,0.0};
rawData[385] = new double[] {143.0,0.0};
rawData[386] = new double[] {423.0,1.0};
rawData[387] = new double[] {215.0,1.0};
rawData[388] = new double[] {98.0,0.0};
rawData[389] = new double[] {53.0,0.0};
rawData[390] = new double[] {351.0,1.0};
rawData[391] = new double[] {184.0,0.0};
rawData[392] = new double[] {549.0,2.0};
rawData[393] = new double[] {241.0,1.0};
rawData[394] = new double[] {128.0,0.0};
rawData[395] = new double[] {77.0,0.0};
rawData[396] = new double[] {94.0,0.0};
rawData[397] = new double[] {266.0,0.0};
rawData[398] = new double[] {196.0,0.0};
rawData[399] = new double[] {138.0,0.0};
rawData[400] = new double[] {95.0,0.0};
rawData[401] = new double[] {137.0,0.0};
rawData[402] = new double[] {155.0,0.0};
rawData[403] = new double[] {119.0,0.0};
rawData[404] = new double[] {68.0,0.0};
rawData[405] = new double[] {441.0,2.0};
rawData[406] = new double[] {397.0,1.0};
rawData[407] = new double[] {306.0,0.0};
rawData[408] = new double[] {180.0,0.0};
rawData[409] = new double[] {107.0,0.0};
rawData[410] = new double[] {175.0,0.0};
rawData[411] = new double[] {138.0,0.0};
rawData[412] = new double[] {113.0,0.0};
rawData[413] = new double[] {186.0,0.0};
rawData[414] = new double[] {105.0,0.0};
rawData[415] = new double[] {123.0,0.0};
rawData[416] = new double[] {78.0,0.0};
rawData[417] = new double[] {122.0,0.0};
rawData[418] = new double[] {72.0,0.0};
rawData[419] = new double[] {168.0,0.0};
rawData[420] = new double[] {82.0,0.0};
rawData[421] = new double[] {461.0,1.0};
rawData[422] = new double[] {191.0,0.0};
rawData[423] = new double[] {112.0,0.0};
rawData[424] = new double[] {207.0,0.0};
rawData[425] = new double[] {175.0,0.0};
rawData[426] = new double[] {184.0,0.0};
rawData[427] = new double[] {116.0,0.0};
rawData[428] = new double[] {162.0,0.0};
rawData[429] = new double[] {136.0,0.0};
rawData[430] = new double[] {195.0,0.0};
rawData[431] = new double[] {141.0,0.0};
rawData[432] = new double[] {136.0,0.0};
rawData[433] = new double[] {118.0,0.0};
rawData[434] = new double[] {502.0,0.0};
rawData[435] = new double[] {143.0,0.0};
rawData[436] = new double[] {299.0,1.0};
rawData[437] = new double[] {305.0,1.0};
rawData[438] = new double[] {286.0,1.0};
rawData[439] = new double[] {74.0,0.0};
rawData[440] = new double[] {22.0,0.0};
rawData[441] = new double[] {143.0,0.0};
rawData[442] = new double[] {412.0,1.0};
rawData[443] = new double[] {90.0,0.0};
rawData[444] = new double[] {75.0,0.0};
rawData[445] = new double[] {26.0,0.0};
rawData[446] = new double[] {29.0,0.0};
rawData[447] = new double[] {83.0,0.0};
rawData[448] = new double[] {216.0,0.0};
rawData[449] = new double[] {133.0,0.0};
rawData[450] = new double[] {45.0,0.0};
rawData[451] = new double[] {137.0,0.0};
rawData[452] = new double[] {237.0,2.0};
rawData[453] = new double[] {20.0,0.0};
rawData[454] = new double[] {24.0,0.0};
rawData[455] = new double[] {18.0,0.0};
rawData[456] = new double[] {137.0,0.0};
rawData[457] = new double[] {98.0,1.0};
rawData[458] = new double[] {21.0,0.0};
rawData[459] = new double[] {25.0,0.0};
rawData[460] = new double[] {38.0,0.0};
rawData[461] = new double[] {128.0,0.0};
rawData[462] = new double[] {302.0,1.0};
rawData[463] = new double[] {69.0,0.0};
rawData[464] = new double[] {99.0,0.0};
rawData[465] = new double[] {239.0,1.0};
rawData[466] = new double[] {192.0,1.0};
rawData[467] = new double[] {120.0,0.0};
rawData[468] = new double[] {127.0,0.0};
rawData[469] = new double[] {410.0,1.0};
rawData[470] = new double[] {115.0,0.0};
rawData[471] = new double[] {698.0,2.0};
rawData[472] = new double[] {784.0,3.0};
rawData[473] = new double[] {23.0,0.0};
rawData[474] = new double[] {201.0,1.0};
rawData[475] = new double[] {268.0,0.0};
rawData[476] = new double[] {36.0,0.0};
rawData[477] = new double[] {413.0,0.0};
rawData[478] = new double[] {515.0,0.0};
rawData[479] = new double[] {270.0,2.0};
rawData[480] = new double[] {28.0,0.0};
rawData[481] = new double[] {147.0,0.0};
rawData[482] = new double[] {225.0,0.0};
rawData[483] = new double[] {132.0,0.0};
rawData[484] = new double[] {197.0,0.0};
rawData[485] = new double[] {130.0,0.0};
rawData[486] = new double[] {122.0,0.0};
rawData[487] = new double[] {140.0,0.0};
rawData[488] = new double[] {87.0,0.0};
rawData[489] = new double[] {969.0,3.0};
rawData[490] = new double[] {142.0,0.0};
rawData[491] = new double[] {211.0,0.0};
rawData[492] = new double[] {204.0,0.0};
rawData[493] = new double[] {127.0,0.0};
rawData[494] = new double[] {311.0,2.0};
rawData[495] = new double[] {161.0,0.0};
rawData[496] = new double[] {1248.0,2.0};
rawData[497] = new double[] {126.0,0.0};
rawData[498] = new double[] {24.0,0.0};
rawData[499] = new double[] {514.0,0.0};
rawData[500] = new double[] {527.0,2.0};
rawData[501] = new double[] {60.0,1.0};
rawData[502] = new double[] {152.0,0.0};
rawData[503] = new double[] {64.0,0.0};
rawData[504] = new double[] {15.0,0.0};
rawData[505] = new double[] {256.0,1.0};
rawData[506] = new double[] {340.0,1.0};
rawData[507] = new double[] {131.0,0.0};
rawData[508] = new double[] {115.0,0.0};
rawData[509] = new double[] {35.0,0.0};
rawData[510] = new double[] {283.0,2.0};
rawData[511] = new double[] {128.0,0.0};
rawData[512] = new double[] {333.0,2.0};
rawData[513] = new double[] {196.0,1.0};
rawData[514] = new double[] {192.0,2.0};
rawData[515] = new double[] {92.0,1.0};
rawData[516] = new double[] {349.0,5.0};
rawData[517] = new double[] {395.0,2.0};
rawData[518] = new double[] {315.0,0.0};
rawData[519] = new double[] {367.0,2.0};
rawData[520] = new double[] {606.0,1.0};
rawData[521] = new double[] {432.0,4.0};
rawData[522] = new double[] {83.0,1.0};
rawData[523] = new double[] {614.0,8.0};
rawData[524] = new double[] {31.0,0.0};
rawData[525] = new double[] {146.0,0.0};
rawData[526] = new double[] {68.0,0.0};
rawData[527] = new double[] {85.0,0.0};
rawData[528] = new double[] {598.0,8.0};
rawData[529] = new double[] {72.0,0.0};
rawData[530] = new double[] {77.0,0.0};
rawData[531] = new double[] {31.0,0.0};
rawData[532] = new double[] {74.0,0.0};
rawData[533] = new double[] {48.0,0.0};
rawData[534] = new double[] {170.0,0.0};
rawData[535] = new double[] {153.0,0.0};
rawData[536] = new double[] {180.0,0.0};
rawData[537] = new double[] {127.0,0.0};
rawData[538] = new double[] {169.0,0.0};
rawData[539] = new double[] {266.0,1.0};
rawData[540] = new double[] {136.0,0.0};
rawData[541] = new double[] {190.0,1.0};
rawData[542] = new double[] {195.0,0.0};
rawData[543] = new double[] {12.0,0.0};
rawData[544] = new double[] {447.0,0.0};
rawData[545] = new double[] {133.0,0.0};
rawData[546] = new double[] {384.0,0.0};
rawData[547] = new double[] {167.0,0.0};
rawData[548] = new double[] {212.0,1.0};
rawData[549] = new double[] {95.0,0.0};
rawData[550] = new double[] {103.0,0.0};
rawData[551] = new double[] {132.0,1.0};
rawData[552] = new double[] {66.0,0.0};
rawData[553] = new double[] {75.0,0.0};
rawData[554] = new double[] {123.0,0.0};
rawData[555] = new double[] {142.0,0.0};
rawData[556] = new double[] {88.0,0.0};
rawData[557] = new double[] {153.0,2.0};
rawData[558] = new double[] {185.0,1.0};
rawData[559] = new double[] {86.0,0.0};
rawData[560] = new double[] {125.0,0.0};
rawData[561] = new double[] {69.0,0.0};
rawData[562] = new double[] {14.0,0.0};
rawData[563] = new double[] {19.0,0.0};
rawData[564] = new double[] {156.0,0.0};
rawData[565] = new double[] {65.0,0.0};
rawData[566] = new double[] {64.0,0.0};
rawData[567] = new double[] {124.0,0.0};
rawData[568] = new double[] {185.0,0.0};
rawData[569] = new double[] {154.0,0.0};
rawData[570] = new double[] {146.0,0.0};
rawData[571] = new double[] {223.0,0.0};
rawData[572] = new double[] {74.0,0.0};
rawData[573] = new double[] {136.0,0.0};
rawData[574] = new double[] {192.0,0.0};
rawData[575] = new double[] {126.0,0.0};
rawData[576] = new double[] {193.0,1.0};
rawData[577] = new double[] {480.0,2.0};
rawData[578] = new double[] {173.0,0.0};
rawData[579] = new double[] {156.0,0.0};
rawData[580] = new double[] {173.0,0.0};
rawData[581] = new double[] {175.0,0.0};
rawData[582] = new double[] {100.0,0.0};
rawData[583] = new double[] {153.0,0.0};
rawData[584] = new double[] {140.0,0.0};
rawData[585] = new double[] {427.0,0.0};
rawData[586] = new double[] {94.0,0.0};
rawData[587] = new double[] {156.0,0.0};
rawData[588] = new double[] {256.0,0.0};
rawData[589] = new double[] {231.0,0.0};
rawData[590] = new double[] {189.0,0.0};
rawData[591] = new double[] {262.0,1.0};
rawData[592] = new double[] {316.0,0.0};
rawData[593] = new double[] {203.0,0.0};
rawData[594] = new double[] {172.0,0.0};
rawData[595] = new double[] {300.0,1.0};
rawData[596] = new double[] {140.0,0.0};
rawData[597] = new double[] {253.0,1.0};
rawData[598] = new double[] {255.0,1.0};
rawData[599] = new double[] {120.0,0.0};
rawData[600] = new double[] {165.0,0.0};
rawData[601] = new double[] {317.0,2.0};
rawData[602] = new double[] {187.0,0.0};
rawData[603] = new double[] {110.0,0.0};
rawData[604] = new double[] {245.0,0.0};
rawData[605] = new double[] {138.0,0.0};
rawData[606] = new double[] {73.0,0.0};
rawData[607] = new double[] {77.0,0.0};
rawData[608] = new double[] {81.0,0.0};
rawData[609] = new double[] {616.0,0.0};
rawData[610] = new double[] {209.0,0.0};
rawData[611] = new double[] {159.0,0.0};
rawData[612] = new double[] {70.0,0.0};
rawData[613] = new double[] {123.0,0.0};
rawData[614] = new double[] {79.0,0.0};
rawData[615] = new double[] {155.0,0.0};
rawData[616] = new double[] {129.0,0.0};
rawData[617] = new double[] {77.0,0.0};
rawData[618] = new double[] {153.0,0.0};
rawData[619] = new double[] {128.0,0.0};
rawData[620] = new double[] {89.0,0.0};
rawData[621] = new double[] {192.0,1.0};
rawData[622] = new double[] {398.0,1.0};
rawData[623] = new double[] {123.0,0.0};
rawData[624] = new double[] {165.0,0.0};
rawData[625] = new double[] {137.0,0.0};
rawData[626] = new double[] {182.0,0.0};
rawData[627] = new double[] {371.0,0.0};
rawData[628] = new double[] {115.0,0.0};
rawData[629] = new double[] {100.0,0.0};
rawData[630] = new double[] {81.0,0.0};
rawData[631] = new double[] {134.0,0.0};
rawData[632] = new double[] {93.0,0.0};
rawData[633] = new double[] {361.0,2.0};
rawData[634] = new double[] {70.0,0.0};
rawData[635] = new double[] {116.0,0.0};
rawData[636] = new double[] {87.0,0.0};
rawData[637] = new double[] {261.0,0.0};
rawData[638] = new double[] {148.0,0.0};
rawData[639] = new double[] {84.0,0.0};
rawData[640] = new double[] {70.0,0.0};
rawData[641] = new double[] {160.0,0.0};
rawData[642] = new double[] {110.0,0.0};
rawData[643] = new double[] {102.0,0.0};
rawData[644] = new double[] {97.0,0.0};
rawData[645] = new double[] {70.0,0.0};

		


		rawData[646] = new double[] {(double)GlobalState.elapsedTime, (double)GlobalState.failures};
			
		
		//for (int i = 0; i < 647;i++){
		//	Debug.Log("Clustering " + clustering[i].ToString());
		//}
		
		Debug.Log("This Level = " + GlobalState.level.FileName);
		string[] intermediateNames = GlobalState.level.FileName.Split('\\');
		int lenOfInter = intermediateNames.Length;
		string levelname = intermediateNames[lenOfInter-1];
		if (GlobalState.HintMode == 0 && GlobalState.AdaptiveMode > 0){
			levelname = levelname.Substring(1);
		}
		Debug.Log("Levelname = " + levelname);
		Debug.Log("Old Adaptive Mode = " + GlobalState.AdaptiveMode.ToString());
		if (!(levelname.Contains("tut"))){
			int[] clustering = ClusteringKMeans.KMeansDemo.Cluster(rawData, 3);	
			
			int count0 = 0;
			int count1 = 0;
			int count2 = 0;
			double fail0 = 0;
			double fail1 = 0;
			double fail2 = 0;
			double time0 = 0;
			double time1 = 0;
			double time2 = 0;
			
			for (int i = 0;i < 646; i++){
				if (levelname == dataNames[i]){
					if (clustering[i] == 0){
						count0++;
						time0 += rawData[i][0];
						fail0 += rawData[i][1];
					}
					else if (clustering[i] == 1){
						count1++;
						time1 += rawData[i][0];
						fail1 += rawData[i][1];
					}
					else{
						count2++;
						time2 += rawData[i][0];
						fail2 += rawData[i][1];
					}
				}
			}
			if (clustering[646] == 0){
				count0++;
				time0 += rawData[646][0];
				fail0 += rawData[646][1];
			}
			else if (clustering[646] == 1){
				count1++;
				time1 += rawData[646][0];
				fail1 += rawData[646][1];
			}
			else{
				count2++;
				time2 += rawData[646][0];
				fail2 += rawData[646][1];
			}
			
			double avgT0 = time0 / count0;
			double avgT1 = time1 / count1;
			double avgT2 = time2 / count2;

			double avgF0 = fail0 / count0;
			double avgF1 = fail1 / count1;
			double avgF2 = fail2 / count2;
			
			Debug.Log("Adaptive Results:");
			
			Debug.Log("Group Counts: " + count0.ToString() + " " + count1.ToString() + " " + count2.ToString());
			Debug.Log("Fail Counts: " + fail0.ToString() + " " + fail1.ToString() + " " + fail2.ToString());
			Debug.Log("Fail Averages: " + avgF0.ToString() + " " + avgF1.ToString() + " " + avgF2.ToString());
			Debug.Log("This Player's Group: " + clustering[646].ToString());
			Debug.Log("This Player's Elapsed Time: " + rawData[646][0].ToString());
			
			int max=0;
			int mid=0;
			int low=0;
			if (avgF0 < avgF1){
				if (avgF0 < avgF2){
					max = 0;
					if (avgF1 > avgF2){
						mid = 1;
						low = 2;
					}
					else{
						mid = 2;
						low = 1;
					}
				}
				else{
					max = 2;
					mid = 0;
					low = 1;
				}
			}
			else if (avgF1 < avgF2){
				max = 1;
				if (avgF0 < avgF2){
					mid = 0;
					low = 2;
				}
				else{
					mid = 2;
					low = 0;
				}
			}
			else{
				max = 2;
				mid = 1;
				low = 0;
			}
			
			if (clustering[646] == max){
				GlobalState.AdaptiveMode = 0;
			}
			else if (clustering[646] == mid){
				GlobalState.AdaptiveMode = 1;
			}
			else{
				GlobalState.AdaptiveMode = 2;
			}
			
			Debug.Log("New Adaptive Mode = " + GlobalState.AdaptiveMode.ToString());
		}
		else{
			Debug.Log("Tutorial level; no ML done");
		}
			GlobalState.elapsedTime = 0;
			GlobalState.failures = 0;			
            SceneManager.LoadScene("Cinematic", LoadSceneMode.Single);
        }
        else
        {
            Victory();
        }
    }
    /// <summary>
    /// Use this if the level needs to be loaded because of the player warping. 
    /// </summary>
    /// <param name="file">file of the warped level</param>
    /// <param name="line">line number to take the player</param>
    public void WarpLevel(string file, string line)
    {
#if (UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN) && !UNITY_WEBGL
        Debug.Log("GameController: WarpLevel() WINDOWS");
#endif

        //Want to check if the player is WebGL, and if it is, grab the xml as a string and put it in levelfactory

#if UNITY_WEBGL
            WebHelper.i.url = stringLib.SERVER_URL + file;
            WebHelper.i.GetWebDataFromWeb();
            file = WebHelper.i.webData;
#endif

        factory = new LevelFactory(file, true);
        GlobalState.level = factory.GetLevel();
        lg.BuildLevel(true);
        lg.WarpPlayer(line);
        lg.manager.ResizeObjects();
    }
    void Awake()
    {
        if (GlobalState.level.IsDemo)
        {
            GlobalState.level.IsDemo = true;
            hero = Instantiate(Resources.Load<GameObject>("Prefabs/DemoRobot"));
        }
        else{
            GlobalState.level.IsDemo = false; 
            hero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"+GlobalState.Character)); 
        }
        hero.name = "Hero"; 
    }
    // Start is called before the first frame update
    void Start()
    {
        GlobalState.foundBug = false;
        GlobalState.isPassed = false;
        GameObject.Find("Fade").GetComponent<Fade>().onFadeIn();
        lg = GameObject.Find("CodeScreen").GetComponent<LevelGenerator>();

        startDate = DateTime.Now;
        GlobalState.CurrentLevelPoints = 0; 

        //GlobalState.level = factory.GetLevel();
        backButton = GameObject.Find("BackButton").GetComponent<BackButton>();
        output = GameObject.Find("OutputCanvas").transform.GetChild(0).gameObject.GetComponent<Output>();
        sidebar = GameObject.Find("Sidebar").GetComponent<SidebarController>();
        background = GameObject.Find("BackgroundCanvas").GetComponent<BackgroundController>();
        selectedTool = sidebar.transform.GetChild(2).transform.Find("Sidebar Tool").GetComponent<SelectedTool>();
        EnergyController = GameObject.Find("Energy").GetComponent<EnergyController>();
        leftCodescreen = GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE;
        logger = new Logger();
    }
    /// <summary>
    /// Brings the player back to the main menu. 
    /// The character moves 
    /// </summary>
    public void Escape()
    {
        if (!GlobalState.level.IsDemo)
        {
            //SaveGameState();
            GlobalState.IsResume = true;
            firstUpdate = true;
            GlobalState.GameState = stateLib.GAMESTATE_MENU;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
            CodeProperties properties = new CodeProperties();
            hero.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            hero.transform.position = new Vector3(GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE + 0.5f, properties.initialLineY, hero.transform.position.z);
            hero.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        else
        {
            GlobalState.GameState = stateLib.GAMESTATE_LEVEL_WIN;
            logger.onGameEnd(startDate, true, EnergyController.currentEnergy);
            GlobalState.previousFilename = GlobalState.CurrentONLevel;
            SceneManager.LoadScene("Cinematic", LoadSceneMode.Single);
        }
    }
    /// <summary>
    /// Handles operations regaserding the UI of the game. 
    /// </summary>
    private void HandleInterface()
    {
        if (Input.GetKeyDown(KeyCode.C) && !Output.IsAnswering)
        {
            sidebar.ToggleSidebar(); 
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !Output.IsAnswering)
        {
            Escape();
        }

    }
    /// <summary>
    /// Triggers all elements with the Toggle theme capability to 
    /// update their theme. 
    /// </summary>
    private void SetTheme()
    {
        if (!GlobalState.IsDark)
        {
            lg.ToggleLight();
            sidebar.ToggleLight();
            output.ToggleLight();
            background.ToggleLight();
            backButton.ToggleColor();
            EnergyController.ToggleLight();
            GlobalState.level.ToggleLight();
            lg.DrawInnerXmlLinesToScreen();
        }
        else
        {
            lg.ToggleDark();
            sidebar.ToggleDark();
            output.ToggleDark();
            background.ToggleDark();
            backButton.ToggleColor();
            GlobalState.level.ToggleDark();
            EnergyController.ToggleDark();
            lg.DrawInnerXmlLinesToScreen();
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (GlobalState.DebugMode && Input.GetKeyDown(KeyCode.G)){
            GlobalState.Stats.GrantPower(); 
            // Debug.Log("All Powers Maxed Out!"); 
            //  Debug.Log("XP Boost: " + GlobalState.Stats.XPBoost.ToString() 
            // +"\n Speed: " + GlobalState.Stats.Speed.ToString()
            // + "\n DamageLevel: " + GlobalState.Stats.DamageLevel.ToString()
            //  + "\n Energy: " + GlobalState.Stats.Energy.ToString() 
            //  +"\n Points: " + GlobalState.Stats.Points.ToString()); 

        }
		if (GlobalState.DebugMode && Input.GetKeyDown(KeyCode.Alpha0)){
			GlobalState.AdaptiveMode = 0;
			Debug.Log("Adaptive Mode = 0");
		}
		if (GlobalState.DebugMode && Input.GetKeyDown(KeyCode.Alpha1)){
			GlobalState.AdaptiveMode = 1;
			Debug.Log("Adaptive Mode = 1");
		}
		if (GlobalState.DebugMode && Input.GetKeyDown(KeyCode.Alpha2)){
			GlobalState.AdaptiveMode = 2;
			Debug.Log("Adaptive Mode = 2");
		}
		if (GlobalState.DebugMode && Input.GetKeyDown(KeyCode.Alpha3) && GlobalState.HintMode == 0){
			GlobalState.HintMode = 1;
			Debug.Log("Hint Mode ON");
		}
		if (GlobalState.DebugMode && Input.GetKeyDown(KeyCode.Alpha3) && GlobalState.HintMode == 1){
			GlobalState.HintMode = 0;
			Debug.Log("Hint Mode OFF");
		}
        if (firstUpdate && !GlobalState.IsResume)
        {
            firstUpdate = false;
            SetTheme();
            lg.TransformTextSize();
        }
        if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME)
        {
            if (!finalized){
                CheckWin();
                CheckLose();
            }
            HandleInterface();
        }
        if (leftCodescreen != GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE && GlobalState.GameState == stateLib.GAMESTATE_IN_GAME){
            leftCodescreen = GlobalState.StringLib.LEFT_CODESCREEN_X_COORDINATE; 
            lg.DrawInnerXmlLinesToScreen(); 
        }
    }
}
