using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class EntryControl : MonoBehaviour
{

    Text playerName; 
    Text playerRank; 
    Text playerScore; 
    string scorePrefix; 

    LeaderboardData data; 
    public LeaderboardData Data{
        set{
            data = value; 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerName = transform.GetChild(1).GetComponent<Text>(); 
        playerRank = transform.GetChild(2).GetComponent<Text>(); 
        playerScore = transform.GetChild(3).GetComponent<Text>();
        scorePrefix = playerScore.text; 

        if (data.IsPlayer){
            playerName.text = GlobalState.StringLib.node_color_print_light+ data.PlayerName + stringLib.CLOSE_COLOR_TAG; 
            playerRank.text =  GlobalState.StringLib.node_color_print_light+"#" + data.PlayerRank.ToString()  + stringLib.CLOSE_COLOR_TAG; 
            playerScore.text =GlobalState.StringLib.node_color_print_light+ scorePrefix + data.PlayerScore.ToString()  + stringLib.CLOSE_COLOR_TAG;
        } else{
            playerName.text =  data.PlayerName; 
            playerRank.text = "#" + data.PlayerRank.ToString(); 
            playerScore.text = scorePrefix + data.PlayerScore.ToString();
        }
    }
    public IEnumerator AnimateIn(){
        GetComponent<CanvasGroup>().alpha = 1; 
        RectTransform position = GetComponent<RectTransform>(); 
        float speed = 0.5f; 
        while(position.position.x > 0.5){
            position.position = new Vector3(position.position.x - speed, position.position.y, position.position.z); 
            yield return null; 
        }
    }
    public IEnumerator AnimateBottom(){
        RectTransform t = GetComponent<RectTransform>(); 
        CanvasGroup canvas = GetComponent<CanvasGroup>(); 

        t.localScale = new Vector3(0.5f, 0.5f, 1); 
        float scaleSpeed = 0.025f; 
        float alphaSpeed = 0.05f; 
        while(t.localScale.x < 1){
            t.localScale = new Vector3(t.localScale.x + scaleSpeed, t.localScale.y + scaleSpeed, t.localScale.z); 
            canvas.alpha += alphaSpeed; 
            yield return new WaitForSeconds(0.01f); 
        }
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
