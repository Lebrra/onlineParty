using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClockWatcher : MinigameLoader
{
    public TextMeshProUGUI clock;
    public Animator anim, buttonAnim;

    public float startTime, myTime;
    public bool canPress;

    new void Start()
    {
        base.Start();
        //StartCoroutine("StartClock", 3f);
        //StartCoroutine("HideClock", 7f);
    }

    public override void StartGame()
    {
        base.StartGame();
        minigameState = true;
        StartCoroutine("StartClock", 3f);
        StartCoroutine("HideClock", 7f);
    }

    // Update is called once per frame
    void Update()
    {
        if (minigameState)
        {
            startTime -= Time.deltaTime;
            //clock.text = System.Math.Round(startTime, 1).ToString();
        }
        else
        {
            System.Math.Round(startTime, 1).ToString();
        }

        if (Input.GetKeyDown(KeyCode.Space) && canPress)
        {
            buttonAnim.SetTrigger("ButtonPress");
            canPress = false;
            StopClock();
        }
    }

    public void StopClock()
    {
        minigameState = false;
        //clock.color = new Color(255, 255, 255, 255);
        myTime = startTime;
        myTime = (float)System.Math.Round(myTime, 3);
        float sendTime = -Mathf.Abs(myTime);
        SendFinalScore(sendTime.ToString());
    }

    public IEnumerator StartClock(float delay)
    {
        yield return new WaitForSeconds(delay);
        minigameState = true;
        anim.SetTrigger("Start");
    }

    public IEnumerator HideClock(float delay)
    {
        yield return new WaitForSeconds(delay);
        //clock.color = new Color(0, 0, 0, 0);
        canPress = true;
    }
}
