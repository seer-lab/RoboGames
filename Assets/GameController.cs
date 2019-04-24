using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    LevelFactory factory; 
    LevelManager manager;
    LevelGenerator lg;
    bool winning = false; 
    private void CheckWin()
    {
        if (GlobalState.GameMode == stringLib.GAME_MODE_ON && GlobalState.level != null)
        {
            winning = true;
            for (int i = 0; i < 5; i++)
            {
                if (GlobalState.level.Tasks[i] != GlobalState.level.CompletedTasks[i])
                {
                    winning = false;
                }
            }
            if (winning)
            {
                StartCoroutine(Win()); 
            }
        }
    }

    IEnumerator Win()
    {
        yield return new WaitForSecondsRealtime(2.2f); 
        if (GlobalState.level.NextLevel != GlobalState.GameMode + "leveldata" + GlobalState.FilePath && winning)
        {
            manager.SaveGame();
            GlobalState.GameState = stateLib.GAMESTATE_LEVEL_WIN;
            lg.GUISwitch(false);
            winning = false; 
        }
    }
    public void SetLevel(string file)
    {
        factory = new LevelFactory(file);
        GlobalState.level = factory.GetLevel();
        lg.BuildLevel(); 
    }
    // Start is called before the first frame update
    void Start()
    {
        manager = new LevelManager(); 
        lg = GameObject.Find("CodeScreen").GetComponent<LevelGenerator>();
        factory = new LevelFactory(GlobalState.GameMode + "leveldata" + GlobalState.FilePath + GlobalState.CurrentONLevel);
        GlobalState.level = factory.GetLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME)
            CheckWin(); 
    }
}
