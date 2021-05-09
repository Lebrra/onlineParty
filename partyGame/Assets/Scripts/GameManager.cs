using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool initialLoad = false;

    public bool myTurn;
    //public GameObject actionChoice, diceScreen;

    public static GameManager inst;
    MinigameManager myMM;

    [Header("Players")]
    public int myUser = -1;
    public PlayerObject[] players;

    [Header("Minigames")]
    public string[] minigameActivePool;
    [SerializeField]
    public Dictionary<string, Sprite> minigameSprites;

    private void Awake()
    {
        if (inst) Destroy(this);

        myMM = GetComponent<MinigameManager>();
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
        Debug.Log("it is " + players[turn].username + "'s turn! Index: " + turn);

        if (turn == myUser)
        {
            //Debug.LogWarning("its your turn!");
            myTurn = true;
            GameBoardConnector.inst?.EnableActions(true);
        }
        else GameBoardConnector.inst?.EnableActions(false);

        GameBoardConnector.inst?.SetPlayerTurn(turn);
    }

    public void EndTurn()
    {
        if (myTurn)
        {
            myTurn = false;
            ServerManager.server?.AdvanceTurn();
            Debug.LogWarning("turn advanced");
        }
    }

    public void PlayerRoll(int index, int amount)
    {
        Debug.Log("Index: " + index);
        Debug.Log(players[index].username + " has rolled a " + amount);
        GameBoardConnector.inst?.ShowDiceRoll(amount);

        // then maybe start corutine for movement?
        StartCoroutine(GameBoardConnector.inst?.MovePlayer(2F, index, amount));
    }

    public void GetActiveMinigames(List<string> minigames)
    {
        if (minigames.Count > 0) foreach (string s in minigames) myMM.AddActiveMinigame(s);
        else Debug.Log("no minigames found.");
    }
}


[System.Serializable]
public struct PlayerObject
{
    public string id;
    public string username;

    public int currentSpace;

    //public GameObject myUI;
}