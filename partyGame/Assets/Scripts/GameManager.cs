using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool myTurn;
    public GameObject actionChoice, diceScreen;

    public static GameManager inst;

    [Header("UI Things")]
    public int myUser = -1;
    public PlayerObject[] players;

    public GameObject[] uiObjects;

    Color32 normalColor = new Color32(212, 212, 212, 255);
    Color32 turnColor = new Color32(103, 243, 127, 255);

    // Start is called before the first frame update
    void Start()
    {
        inst = this;
        diceScreen.SetActive(false);

        ServerManager.server?.SetReady();
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

    public void LoadPlayerUI(int thisPlayer, PlayerObject[] players)
    {
        if (thisPlayer == -1) Debug.LogError("player index not found");
        else myUser = thisPlayer;

        this.players = new PlayerObject[players.Length];

        for(int i = 0; i < players.Length; i++)
        {
            this.players[i] = new PlayerObject();

            this.players[i].id = players[i].id;
            this.players[i].username = players[i].username;
            this.players[i].myUI = uiObjects[i];

            uiObjects[i].SetActive(true);
            uiObjects[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = this.players[i].username;
        }

        this.players[myUser].myUI.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
    }
}


[System.Serializable]
public struct PlayerObject
{
    public string id;
    public string username;

    public GameObject myUI;
}