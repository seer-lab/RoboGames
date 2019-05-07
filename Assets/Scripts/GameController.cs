using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Xml; 
using UnityEngine;

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
    bool winning = false;

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
                StartCoroutine(Win()); 
            }
        }
    }
    /// <summary>
    /// Checks if the Game meets the Lose Condition
    /// </summary>
    private void CheckLose()
    {
        if ((selectedTool.isLosing && GlobalState.GameMode == stringLib.GAME_MODE_ON)
            || (selectedTool.toolCounts[0] <= 0 && GlobalState.GameMode == stringLib.GAME_MODE_BUG && GlobalState.level.Tasks[0]>0 && 
            selectedTool.noRemainingActivators))
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
        GlobalState.IsPlaying = false;
        GlobalState.CurrentONLevel = "level5";
        GlobalState.GameState = stateLib.GAMESTATE_GAME_END;
        SceneManager.LoadScene("Credits");
    }

    /// <summary>
    /// Switches to the Level Lose and Loads the Cinematic Scene. 
    /// </summary>
    public void GameOver()
    {
        GlobalState.IsPlaying = false;
        GlobalState.GameState = stateLib.GAMESTATE_LEVEL_LOSE;
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
        yield return new WaitForSecondsRealtime(2.4f); 
        if (!winning)
        {
            GameOver();  
        }
    }
    /// <summary>
    /// Delays the Win Operation to ensure the player actually won, then 
    /// completes win game operations. 
    /// </summary>
    /// <returns></returns>
    IEnumerator Win()
    {
        yield return new WaitForSecondsRealtime(2.2f);
        //if the end of the level string is empty then there is no anticipated next level. 
        if (GlobalState.level.NextLevel != GlobalState.GameMode + "leveldata" + GlobalState.FilePath )
        {
            lg.manager.SaveGame();
            GlobalState.GameState = stateLib.GAMESTATE_LEVEL_WIN;
            SceneManager.LoadScene("Cinematic"); 
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
        factory = new LevelFactory(file, true);
        GlobalState.level = factory.GetLevel();
        lg.BuildLevel(true);
        lg.WarpPlayer(line); 

    }
    // Start is called before the first frame update
    void Start()
    {
        GlobalState.IsDark = true; 
        lg = GameObject.Find("CodeScreen").GetComponent<LevelGenerator>();
        Debug.Log(GlobalState.CurrentONLevel);
        factory = new LevelFactory(GlobalState.GameMode + "leveldata" + GlobalState.FilePath + GlobalState.CurrentONLevel);
        GlobalState.level = factory.GetLevel();
        output = GameObject.Find("OutputCanvas").transform.GetChild(0).gameObject.GetComponent<Output>();
        sidebar = GameObject.Find("Sidebar").GetComponent<SidebarController>();
        background = GameObject.Find("BackgroundCanvas").GetComponent<BackgroundController>();
        selectedTool = sidebar.transform.Find("Sidebar Tool").GetComponent<SelectedTool>(); 
    }
    /// <summary>
    /// Handles operations regarding the UI of the game. 
    /// </summary>
    private void HandleInterface()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !Output.IsAnswering)
        {
            ToggleTheme(); 
        }
        else if (Input.GetKeyDown(KeyCode.C) && !Output.IsAnswering)
        {
            Debug.Log("Handling Sidebar"); 
            sidebar.ToggleSidebar(); 
        }
        else if (Input.GetKeyDown(KeyCode.X) && !Output.IsAnswering){
            lg.TransformTextSize(); 
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !Output.IsAnswering)
        {
            GlobalState.GameState = stateLib.GAMESTATE_MENU;
            SceneManager.LoadScene("MainMenu");
        }
    }
    /// <summary>
    /// Triggers all elements with the Toggle theme capability to 
    /// update their theme. 
    /// </summary>
    private void ToggleTheme()
    {
        GlobalState.IsDark = !GlobalState.IsDark; 
        if (GlobalState.IsDark)
        {
            lg.ToggleDark();
            sidebar.ToggleDark();
            output.ToggleDark();
            background.ToggleDark(); 
        }
        else
        {
            lg.ToggleLight();
            sidebar.ToggleLight();
            output.ToggleLight();
            background.ToggleLight(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME)
        {
            CheckWin();
            CheckLose(); 
            HandleInterface(); 
        }
    }
}
