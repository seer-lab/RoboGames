using System.Runtime.Versioning;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardControl : MonoBehaviour
{
    public GameObject logoText;
    public GameObject logoImage;
    public GameObject ErrorText;
    public GameObject InputField;
    public GameObject EnterButton;
    public GameObject Forms;

    public int index;

    List<LeaderboardData> leaderboard; 
    LeaderboardData player; 
    bool playerInLeaderboard = false; 
    List<GameObject> leaderboardObjects; 
    GameObject playerObject; 
    // Start is called before the first frame update
    void Start()
    {
        ErrorText.GetComponent<Text>().text = "";
        leaderboard = new List<LeaderboardData>(); 
        leaderboard = createFakeLeaderboard(10); 
        player = createFakeLeaderboard(1).First(); 
        player.IsPlayer = true; 

        var iField = GameObject.Find("InputField").GetComponent<InputField>();
        var submitE = new InputField.SubmitEvent();
        submitE.AddListener(SubmitName);
        iField.onEndEdit = submitE;



        if (leaderboard.Any(e => e.PlayerName == player.PlayerName)){
            leaderboard.Where((e)=> e.PlayerName == player.PlayerName).First().IsPlayer = true; 
            playerInLeaderboard = false; 
        } 
        leaderboardObjects = new List<GameObject>(); 

        if(GlobalState.username == null){
            //Forms.SetActive(false);
            // logoText.SetActive(false);
            // logoImage.SetActive(false);
            //this.GetComponent<Text>("Text").
        }
        // for(int i = 0; i < leaderboard.Count; i++){
        //     CreateEntry(i);            
        // }
        // if (!playerInLeaderboard){
        //     playerObject = Instantiate(Resources.Load<GameObject>("Prefabs/Entry")); 
        //     playerObject.GetComponent<EntryControl>().Data = player; 
        //     playerObject.transform.parent = this.transform; 
        //     playerObject.transform.localScale = new Vector3(1,1,1);
        //     RectTransform position = playerObject.GetComponent<RectTransform>(); 
        //     position.localPosition = new Vector3(position.localPosition.x, position.localPosition.y+190 - (leaderboard.Count+1)*60, position.localPosition.z); 
        // }
        //StartCoroutine(showLeaderboard());
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
    List<LeaderboardData> createFakeLeaderboard(int n){
        List<LeaderboardData> values = new List<LeaderboardData>(); 

        for (int i = 0; i < n; i++){
            values.Add(new LeaderboardData(createName(8), i, Random.Range(0,2000000)));        
        }
        values.OrderBy(e=> e.PlayerScore).ToList().Reverse();
        for (int i = 0; i < values.Count;i++){
            values[i].PlayerRank = i+1; 
        }
        return values; 
    }
    string createName(int length){
        const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789";
        string myString = ""; 
        for(int i=0; i<length; i++)
        {
            myString += glyphs[Random.Range(0, glyphs.Length)];
        }
        return myString; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onclickInput(){
        Debug.Log(GameObject.Find("InputField").GetComponent<InputField>().text);
    }

   private void SubmitName(string arg0)
    {
        Debug.Log(arg0);
    }
}
