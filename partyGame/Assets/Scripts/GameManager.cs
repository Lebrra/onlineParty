using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool initialLoad = false;

    public bool myTurn;
    //public GameObject actionChoice, diceScreen;

    public static GameManager inst;

    [Header("UI Things")]
    public int myUser = -1;
    public PlayerObject[] players;

    //public Image[] uiObjects;

    //Color32 normalColor = new Color32(212, 212, 212, 255);
    //Color32 turnColor = new Color32(103, 243, 127, 255);

    // Start is called before the first frame update
    private void Awake()
    {
        if (inst) Destroy(this);

        inst = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RollDice()
    {
        if (!myTurn) return;

        int rand = Random.Range(1, 4);
        Debug.Log(rand);
        ServerManager.server?.RolledDice(rand);
    }

    public void LoadPlayerList(int thisPlayer, PlayerObject[] playerList)
    {
        if (thisPlayer == -1) Debug.LogError("player index not found");
        else myUser = thisPlayer;

        players = new PlayerObject[playerList.Length];

        for (int i = 0; i < playerList.Length; i++)
        {
            players[i] = new PlayerObject();

            players[i].id = playerList[i].id;
            players[i].username = playerList[i].username;

            Debug.Log("loaded player " + players[i].username);
        }

        GameBoardConnector.inst?.LoadPlayersUI(thisPlayer, playerList);
        initialLoad = true;
    }

    public void SetPlayerTurn(int turn)
    {
        Debug.Log("it is " + players[turn].username + "'s turn!");

        if (turn == myUser)
        {
            Debug.LogWarning("its your turn!");
            myTurn = true;
            GameBoardConnector.inst?.EnableActions(true);
        }
        else GameBoardConnector.inst?.EnableActions(false);

        GameBoardConnector.inst?.SetPlayerTurn(turn);
    }

    public void EndTurn()
    {
        GameBoardConnector.inst?.EndTurn();
        if (myTurn)
            ServerManager.server?.AdvanceTurn();
        myTurn = false;
    }

    public void PlayerRoll(int index, int amount)
    {
        Debug.Log(players[index].username + " has rolled a " + amount);
        GameBoardConnector.inst?.ShowDiceRoll(amount);

        // then maybe start corutine for movement?
    }
}


[System.Serializable]
public struct PlayerObject
{
    public string id;
    public string username;

    //public GameObject myUI;
}