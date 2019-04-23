using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeDisplayController : MonoBehaviour
{
    private float endTime = -100;
    private float startTime = 0;
    private Text text;
    AudioSource audio;
    private bool isTimerAlarmTriggered;
    public ITimeUser Callback { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<Text>();
        audio = this.GetComponent<AudioSource>();
        isTimerAlarmTriggered = false;
    }
    public float EndTime
    {
        set
        {
            startTime = Time.time;
            endTime = value + startTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GlobalState.GameState == stateLib.GAMESTATE_IN_GAME && endTime != -100)
        {
            if (endTime - startTime >= 9000)
            {
                text.text = "Time Remaining: --:--:--";
            }
            else if (endTime - Time.time < 30)
            {
                text.text = "Time Remaining: <size=50><color=red>" + ((int)(endTime - Time.time)).ToString() + "</color></size> seconds";
                if (!isTimerAlarmTriggered)
                {
                    isTimerAlarmTriggered = true;
                    audio.Play();
                }
            }
            else
            {
                int nNumberOfSeconds = (int)(endTime - Time.time);
                if (nNumberOfSeconds > 3600)
                {
                    int nNumberOfHours = nNumberOfSeconds / 3600;
                    nNumberOfSeconds -= nNumberOfHours * 3600;
                    int nNumberOfMinutes = (nNumberOfSeconds) / 60;
                    nNumberOfSeconds -= nNumberOfMinutes * 60;
                    text.text = "Time Remaining: ";
                    if (nNumberOfHours < 10) text.text += "0";
                    text.text += nNumberOfHours.ToString() + ":";
                    if (nNumberOfMinutes < 10) text.text += "0";
                    text.text += nNumberOfMinutes.ToString() + ":";
                    if (nNumberOfSeconds < 10) text.text += "0";
                    text.text += nNumberOfSeconds.ToString();
                }
                else if (nNumberOfSeconds > 60)
                {
                    int nNumberOfMinutes = nNumberOfSeconds / 60;
                    nNumberOfSeconds -= nNumberOfMinutes * 60;
                    text.text = "Time Remaining: 00:";
                    if (nNumberOfMinutes < 10) text.text += "0";
                    text.text += nNumberOfMinutes.ToString() + ":";
                    if (nNumberOfSeconds < 10) text.text += "0";
                    text.text += nNumberOfSeconds.ToString() + ":";
                }
                else
                {
                    text.text = "Time Remaining: 00:00:";
                    if (nNumberOfSeconds < 10) text.text += "0";
                    text.text += nNumberOfSeconds.ToString();
                }
                isTimerAlarmTriggered = false;
            }
            if (endTime < Time.time && endTime - startTime < 9000)
            {
                Callback.OnTimeFinish(); 
            }
        }
    }
}
