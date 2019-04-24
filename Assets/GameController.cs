using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Level level;
    LevelManager manager;
    LevelGenerator lg; 
    private void CheckWin()
    {
        if (GlobalState.GameMode == stringLib.GAME_MODE_ON)
        {
            bool winning = true;
            for (int i = 0; i < 5; i++)
            {
                if (level.Tasks[i] != level.CompletedTasks[i])
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
        yield return new WaitForSecondsRealtime(1f); 
        if (level.NextLevel != GlobalState.GameMode + "leveldata" + GlobalState.FilePath)
        {
            manager.SaveGame();
            GlobalState.GameState = stateLib.GAMESTATE_LEVEL_WIN;
            lg.ClearLevel();
            lg.GUISwitch(false); 

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        lg = GameObject.Find("CodeScreen").GetComponent<LevelGenerator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        //CheckWin(); 
    }
}
