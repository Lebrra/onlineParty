using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameBoardConnector : MonoBehaviour
{
    public static GameBoardConnector inst;

    public GameObject actionChoice, diceScreen;
    public TextMeshProUGUI turnText;
    public Image[] uiObjects;

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
    }

    public void LoadPlayersUI(int thisPlayer, PlayerObject[] players)
    {
        for(int i = 0; i < players.Length; i++)
        {
            uiObjects[i].gameObject.SetActive(true);
            uiObjects[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = players[i].username;

            if (i == thisPlayer) uiObjects[thisPlayer].transform.GetChild(1).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        }
    }

    public void SetPlayerTurn(int player)
    {
        foreach (Image a in uiObjects) a.color = normalColor;
        uiObjects[player].color = turnColor;
        turnText.text = GameManager.inst.players[player].username + "'s Turn";
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
    }

    public void ShowDiceRoll(int value)
    {
        diceScreen.SetActive(true);
        Dice.inst.PickRightNum(value);
        Dice.inst.ResetDie();
    }

    public void EndTurn()
    {
        DisableActions();
        foreach (Image a in uiObjects) a.color = normalColor;
    }
}
