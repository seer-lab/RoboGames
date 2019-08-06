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

        playerName.text = data.PlayerName; 
        playerRank.text = "#" + data.PlayerRank.ToString(); 
        playerScore.text = scorePrefix + data.PlayerScore.ToString();
    }
    public IEnumerator AnimateIn(){
        RectTransform position = GetComponent<RectTransform>(); 
        float speed = 0.5f; 
        while(position.position.x > 0.5){
            position.position = new Vector3(position.position.x - speed, position.position.y, position.position.z); 
            yield return null; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
