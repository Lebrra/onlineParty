using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonPresser : MonoBehaviour
{
    Animator anim;
    public int presses = 0;
    public float timeRemaining = 10f;
    public bool gameState = true;
    public TextMeshProUGUI pressCount;

    void Start()
    {
        anim = GetComponent<Animator>();
        timeRemaining = 10f;
    }

    void Update()
    {
        if(timeRemaining <= 0f)
        {
            gameState = false;
            pressCount.text = presses.ToString() + " Button presses.";
        }

        if (gameState == true)
        {
            timeRemaining -= Time.deltaTime;
            pressCount.text = System.Math.Round(timeRemaining, 1).ToString();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("ButtonPress");
                presses++;
                //ServerManager.server?.SendMinigameData(presses.ToString());
            }
        }
    }
}
