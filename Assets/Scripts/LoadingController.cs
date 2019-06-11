using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using UnityEngine;

public class LoadingController : MonoBehaviour
{
    Transform console;
    // Start is called before the first frame update
    void Start()
    {     
        console = this.transform.GetChild(0).GetComponent<Transform>();
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        
        AsyncOperation async = SceneManager.LoadSceneAsync("newgame");
        while (console.position.y < 550)
        {
            if (async.progress > 0.8f)
                yield return new WaitForSecondsRealtime(0.1f);
            else yield return new WaitForSecondsRealtime(0.7f);
            console.position = new Vector3(console.position.x, console.position.y + 28, console.position.z); 
        }
        while (!async.isDone)
        {
            yield return null;
        }

    }
}
