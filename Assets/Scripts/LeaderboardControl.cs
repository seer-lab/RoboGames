using System.Runtime.Versioning;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardControl : MonoBehaviour
{
    Fade fade;

    List<LeaderboardData> leaderboard; 
    LeaderboardData player; 
    bool playerInLeaderboard = false; 
    List<GameObject> leaderboardObjects; 
    GameObject playerObject; 
    // Start is called before the first frame update
    void Start()
    {
        WebHelper.i.url = stringLib.DB_URL + GlobalState.GameMode.ToUpper() + "/leaderboard/" + GlobalState.previousFilename;
        Debug.Log(WebHelper.i.url);
        WebHelper.i.webData = "";
        WebHelper.i.GetWebDataFromWeb();

        leaderboard = new List<LeaderboardData>();
        fade = GameObject.Find("Fade").GetComponent<Fade>();
        fade.onFadeIn(); 

    }
    void CreateEntry(int index){
        GameObject entry = Instantiate(Resources.Load<GameObject>("Prefabs/Entry")); 
        entry.GetComponent<EntryControl>().Data = leaderboard[index]; 
        entry.transform.parent = this.transform; 
        entry.transform.localScale = new Vector3(1,1,1);
        RectTransform position = entry.GetComponent<RectTransform>(); 
        position.localPosition = new Vector3(position.localPosition.x+3000, position.localPosition.y+190 - index*60, position.localPosition.z); 
        leaderboardObjects.Add(entry); 
    }
    IEnumerator showLeaderboard(){
        foreach(GameObject entry in leaderboardObjects){
            StartCoroutine(entry.GetComponent<EntryControl>().AnimateIn()); 
            yield return new WaitForSecondsRealtime(0.2f); 
        }
        if (!playerInLeaderboard){
            yield return new WaitForSecondsRealtime(2f); 
            StartCoroutine(playerObject.GetComponent<EntryControl>().AnimateBottom()); 
        }
    }
    List<LeaderboardData> createLeaderboard(int n, LeaderboardDataDB[] lbDB){
        List<LeaderboardData> values = new List<LeaderboardData>(); 

        for (int i = 0; i < n; i++){
            values.Add(new LeaderboardData(lbDB[i].name, Convert.ToInt32(lbDB[i].rank), Convert.ToInt32(lbDB[i].points)));        
        }
        values.OrderBy(e=> e.PlayerScore).ToList().Reverse();
        for (int i = 0; i < values.Count;i++){
            values[i].PlayerRank = i+1; 
        }
        return values; 
    }

    // Update is called once per frame
    void Update()
    {
        if(WebHelper.i.webData != ""){
            //Debug.Log("Grabing webdata: " + WebHelper.i.webData);
            string jsonData = WebHelper.i.webData;
            jsonData = jsonData.Replace("\"leaderscores\"", "\"Items\"");

            //Debug.Log(jsonData);
            try{
                LeaderboardDataDB[] leaderscores = JsonHelper.FromJson<LeaderboardDataDB>(jsonData);
                for(int i = 0; i < leaderscores.Length; i++){
                    if(GlobalState.sessionID.ToString() == leaderscores[i].name){
                        List<LeaderboardData> values = new List<LeaderboardData>(); 
                        values.Add(new LeaderboardData(leaderscores[i].name, Convert.ToInt32(leaderscores[i].rank), Convert.ToInt32(leaderscores[i].points)));
                        player = values.First();
                        break;
                    }
                }
                leaderboard = createLeaderboard(10, leaderscores);
            }catch(Exception e){
                Debug.Log(e);
            } 
            player.IsPlayer = true; 

            if (leaderboard.Any(e => e.PlayerName == player.PlayerName)){
                leaderboard.Where((e)=> e.PlayerName == player.PlayerName).First().IsPlayer = true; 
                playerInLeaderboard = false; 
            } 
            leaderboardObjects = new List<GameObject>(); 

            for(int i = 0; i < leaderboard.Count; i++){
                CreateEntry(i);            
            }
            if (!playerInLeaderboard){
                playerObject = Instantiate(Resources.Load<GameObject>("Prefabs/Entry")); 
                playerObject.GetComponent<EntryControl>().Data = player; 
                playerObject.transform.parent = this.transform; 
                playerObject.transform.localScale = new Vector3(1,1,1);
                RectTransform position = playerObject.GetComponent<RectTransform>(); 
                position.localPosition = new Vector3(position.localPosition.x, position.localPosition.y+190 - (leaderboard.Count+1)*60, position.localPosition.z); 
            }
            StartCoroutine(showLeaderboard());
            WebHelper.i.webData = "";
        }

        if(Input.GetMouseButtonDown(0) || Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Return)){
            UnityEngine.SceneManagement.SceneManager.LoadScene("newgame");
            fade.onFadeOut(); 
        }
        
    }
}
