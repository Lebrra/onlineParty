﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClockWatcher : MonoBehaviour
{
    public TextMeshProUGUI clock;
    public Animator anim;

    public float startTime, myTime;
    public bool timerActice;

    // Start is called before the first frame update
    void Start()
    {
        clock.text = startTime.ToString();
        StartCoroutine("StartClock", 3f);
        StartCoroutine("HideClock", 7f);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActice)
        {
            startTime -= Time.deltaTime;
            clock.text = System.Math.Round(startTime, 1).ToString();
        }
        else
        {
            System.Math.Round(startTime, 1).ToString();
        }
    }

    public void StopClock()
    {
        timerActice = false;
        clock.color = new Color(255, 255, 255, 255);
        myTime = startTime;
    }

    public IEnumerator StartClock(float delay)
    {
        yield return new WaitForSeconds(delay);
        timerActice = true;
        anim.SetTrigger("Start");
    }

    public IEnumerator HideClock(float delay)
    {
        yield return new WaitForSeconds(delay);
        clock.color = new Color(0, 0, 0, 0);   
    }
}
