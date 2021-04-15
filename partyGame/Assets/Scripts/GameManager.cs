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

    public Image[] uiObjects;

    Color32 normalColor = new Color32(212, 212, 212, 255);
    Color32 turnColor = new Color32(103, 243, 127, 255);

    // Start is called before the first frame update
    void Start()
    {
        inst = this;
        diceScreen.SetActive(false);

        ServerManager.server?.SetReady();
    }

    public void RollDice()
    {
        if (!myTurn) return;
        myTurn = false;

        diceScreen.SetActive(true);
        int rand = Random.Range(1, 4);
        Debug.Log(rand);
        Dice.inst.PickRightNum(rand);
        Dice.inst.ResetDie();
    }

    public void LoadPlayerUI(int thisPlayer, PlayerObject[] playerList)
    {
        if (thisPlayer == -1) Debug.LogError("player index not found");
        else myUser = thisPlayer;

        players = new PlayerObject[playerList.Length];

        for(int i = 0; i < playerList.Length; i++)
        {
            players[i] = new PlayerObject();

            players[i].id = playerList[i].id;
            players[i].username = playerList[i].username;
            players[i].myUI = uiObjects[i].gameObject;

            uiObjects[i].gameObject.SetActive(true);
            uiObjects[i].transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = players[i].username;

            Debug.Log("loaded player " + players[i].username);
        }

        players[myUser].myUI.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
    }

    public void SetPlayerTurn(int turn)
    {
        foreach (Image a in uiObjects) a.color = normalColor;

        Debug.Log("it is " + players[turn].username + "'s turn!");

        if (turn == myUser)
        {
            Debug.LogWarning("its your turn!");
            myTurn = true;
            actionChoice.SetActive(true);
        }

        uiObjects[turn].color = turnColor;
    }

    public void EndTurn()
    {
        myTurn = false;
        actionChoice.SetActive(false);
        foreach (Image a in uiObjects) a.color = normalColor;
        ServerManager.server?.AdvanceTurn();
    }
}


[System.Serializable]
public struct PlayerObject
{
    public string id;
    public string username;

    public GameObject myUI;
}