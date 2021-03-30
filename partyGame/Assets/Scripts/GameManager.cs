using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool myTurn;
    public GameObject actionChoice, diceScreen;

    public static GameManager inst;

    // Start is called before the first frame update
    void Start()
    {
        inst = this;
        diceScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (myTurn)
            actionChoice.SetActive(true);
        else
            actionChoice.SetActive(false);
    }

    public void RollDice()
    {
        diceScreen.SetActive(true);
        int rand = Random.Range(1, 4);
        Debug.Log(rand);
        Dice.inst.PickRightNum(rand);
        Dice.inst.ResetDie();
    }
}
