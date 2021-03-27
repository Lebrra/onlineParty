using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool myTurn;
    public GameObject actionChoice, diceScreen;

    // Start is called before the first frame update
    void Start()
    {
        diceScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (myTurn)
            actionChoice.SetActive(true);
    }

    public void RollDice()
    {
        int rand = Random.Range(1, 4);
        Debug.Log(rand);
        diceScreen.SetActive(true);
        Dice.inst.Roll();
        Dice.inst.GetNum(rand);
    }
}
