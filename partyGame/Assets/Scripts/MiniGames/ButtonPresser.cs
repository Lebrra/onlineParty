using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonPresser : MinigameLoader
{
    Animator anim;
    public int presses = 0;
    public float timeRemaining = 10f;
    public TextMeshProUGUI pressCount;

    new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        timeRemaining = 10f;
    }

    public IEnumerator SendToServer(float delay)
    {
        yield return new WaitForSeconds(delay);
        ServerManager.server?.SendMinigameData(presses.ToString());
        if (minigameState) StartCoroutine(SendToServer(delay));
    }

    public override void StartGame()
    {
        base.StartGame();
        StartCoroutine(SendToServer(1f));
        minigameState = true;
    }

    void Update()
    {
        if (minigameState == true)
        {
            if (timeRemaining <= 0f)
            {
                minigameState = false;
                pressCount.text = presses.ToString() + " Button presses.";
                SendFinalScore(presses.ToString());
            }

            timeRemaining -= Time.deltaTime;
            pressCount.text = System.Math.Round(timeRemaining, 1).ToString();

            //StartCoroutine(SendToServer(2f));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("ButtonPress");
                presses++;
            }
        }
    }
}
