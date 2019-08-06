using System.Runtime.Versioning;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardControl : MonoBehaviour
{
    List<LeaderboardData> leaderboard; 
    List<GameObject> leaderboardObjects; 
    // Start is called before the first frame update
    void Start()
    {
        leaderboard = new List<LeaderboardData>(); 
        leaderboard = createFakeLeaderboard(10); 
        leaderboardObjects = new List<GameObject>(); 
        for(int i = 0; i < leaderboard.Count; i++){
            GameObject entry = Instantiate(Resources.Load<GameObject>("Prefabs/Entry")); 
            entry.GetComponent<EntryControl>().Data = leaderboard[i]; 
            entry.transform.parent = this.transform; 
            entry.transform.localScale = new Vector3(1,1,1);
            RectTransform position = entry.GetComponent<RectTransform>(); 
            position.localPosition = new Vector3(position.localPosition.x+3000, position.localPosition.y+190 - i*60, position.localPosition.z); 
            leaderboardObjects.Add(entry); 
            //entry.transform.position = new Vector3(entry.transform.position.x, entry.transform.position.y - i, entry.transform.position.z); 
            
        }
        StartCoroutine(showLeaderboard());
    }
    IEnumerator showLeaderboard(){
        foreach(GameObject entry in leaderboardObjects){
            StartCoroutine(entry.GetComponent<EntryControl>().AnimateIn()); 
            yield return new WaitForSecondsRealtime(0.2f); 
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
}
