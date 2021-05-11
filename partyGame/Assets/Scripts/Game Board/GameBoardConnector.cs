using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameBoardConnector : MonoBehaviour
{
    public static GameBoardConnector inst;

    [Header("UI")]
    public GameObject actionChoice, diceScreen;
    public TextMeshProUGUI turnText;
    public Image[] uiObjects;

    [Header("Not UI")]
    public PlayerToken[] playerTokens;

    public BoardSpace[] allSpaces;

    Color32 normalColor = new Color32(212, 212, 212, 255);
    Color32 turnColor = new Color32(103, 243, 127, 255);

    void Awake()
    {
        if (inst)
            if (inst != this) Destroy(inst);
        inst = this;
    }

    private void Start()
    {
        diceScreen.SetActive(false);

        if (GameManager.inst.initialLoad)
            LoadPlayersUI(GameManager.inst.myUser, GameManager.inst.players);

        ServerManager.server?.SetReady();   // needed?
        for (int i = 0; i < GameManager.inst.players.Length; i++) GameManager.inst.players[i].ready = false;
    }

    public void LoadPlayersUI(int thisPlayer, PlayerObject[] players)
    {
        for(int i = 0; i < players.Length; i++)
        {
            uiObjects[i].gameObject.SetActive(true);
            uiObjects[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = players[i].username;
            playerTokens[i].gameObject.SetActive(true);

            if (i == thisPlayer) uiObjects[thisPlayer].transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    public void SetPlayerTurn(int player)
    {
        foreach (Image a in uiObjects) a.color = normalColor;
        uiObjects[player].color = turnColor;

        turnText.transform.parent.gameObject.SetActive(true);
        turnText.text = GameManager.inst.players[player].username + "'s Turn";

        Camera.main.transform.parent.position = playerTokens[player].transform.GetChild(1).transform.position;
        Camera.main.transform.parent.rotation = Quaternion.Euler(Vector3.up * 90 * playerTokens[player].mySpace.GetComponent<BoardSpace>().direction);
    }

    public void EnableActions(bool buttons)
    {
        actionChoice.SetActive(true);
        foreach (Button b in actionChoice.GetComponentsInChildren<Button>()) b.interactable = buttons;
    }
    public void DisableActions()
    {
        actionChoice.SetActive(false);
    }

    public void RollDiceAction()
    {
        DisableActions();

        if (!GameManager.inst.myTurn) return;

        int rand = Random.Range(1, 4);
        Debug.Log("Rolled: " + rand);
        ServerManager.server?.RolledDice(rand);
        DisableActions();
    }

    public void PlayMinigameAction()
    {
        Debug.Log("minigame time!");
        if (!GameManager.inst.myTurn) return;

        ServerManager.server?.MinigameSelect();
        DisableActions();
    }

    public void ShowDiceRoll(int value)
    {
        diceScreen.SetActive(true);
        Dice.inst.PickRightNum(value);
        //Dice.inst.ResetDie();
    }

    public IEnumerator MovePlayer(float waitTime, int player, int amount)
    {
        DisableActions();
        yield return new WaitForSeconds(waitTime);

        Dice.inst.ResetDie();
        turnText.transform.parent.gameObject.SetActive(false);

        StartCoroutine(MovePlayerToken(player, amount));
    }

    IEnumerator MovePlayerToken(int player, int amount)
    {
        yield return new WaitForSeconds(0.7F);
        Debug.Log("moving player");

        if (amount > 0)
        {
            playerTokens[player].AdvanceSpace();
            StartCoroutine(MovePlayerToken(player, amount - 1));
        }
        else
        {
            EndTurn();
            GameManager.inst?.EndTurn();
        }
    }

    public void EndTurn()
    {
        DisableActions();
        foreach (Image a in uiObjects) a.color = normalColor;
    }

    public void SnapPlayersToSpaces()
    {
        for(int i = 0; i < playerTokens.Length; i++)
        {
            playerTokens[i].SetMyLocation(allSpaces[GameManager.inst.players[i].currentSpace]);
        }
    }
}
