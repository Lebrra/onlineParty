using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameBoardConnector : MonoBehaviour
{
    public static GameBoardConnector inst;

    [Header("UI")]
    public TextMeshProUGUI turnText;
    public GameObject actionChoice, diceScreen;
    public Image[] uiObjects;

    public GameObject endScreen;
    public TextMeshProUGUI endText;

    public GameObject pauseScreen;

    public Transform[] minimapTokens;
    public MinimapSpacer[] minimapSpaces; // - 1 for any references because Start space is not included!

    public GameObject minigameScreen;

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
        pauseScreen.SetActive(false);
        endScreen.SetActive(false);

        StartCoroutine(LookForBoardData());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ShowPauseScreen();
    }

    IEnumerator LookForBoardData()
    {
        yield return new WaitForSecondsRealtime(0.1F);

        Debug.LogWarning("looking for game board data to load...");

        if (GameManager.inst.initialLoad && !uiObjects[0].gameObject.activeInHierarchy)
        {
            LoadPlayersUI(GameManager.inst.myUser, GameManager.inst.players);
            SnapPlayersToSpaces();
        }

        for (int i = 0; i < GameManager.inst.players.Length; i++) GameManager.inst.players[i].ready = false;
        ServerManager.server?.SetReady();
    }

    public void LoadPlayersUI(int thisPlayer, PlayerObject[] players)
    {
        Debug.LogWarning("player list length:" + players.Length);
        Debug.LogWarning("player index: " + thisPlayer);

        for (int i = 0; i < players.Length; i++)
        {
            uiObjects[i].gameObject.SetActive(true);
            uiObjects[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = players[i].username;
            playerTokens[i].gameObject.SetActive(true);
            minimapTokens[i].gameObject.SetActive(true);

            if (i == thisPlayer) uiObjects[thisPlayer].transform.GetChild(2).gameObject.SetActive(true);
            if (players[i].disconnected) DisconnectedUI(i);

            Debug.Log("loaded player ui for " + players[i].username);
        }

        Debug.Log("finished loading player ui");
    }

    public void SetPlayerTurn(int player)
    {
        foreach (Image a in uiObjects) a.color = normalColor;
        uiObjects[player].color = turnColor;

        turnText.transform.parent.gameObject.SetActive(true);
        turnText.text = GameManager.inst.players[player].username + "'s Turn";

        SetCameraPos(player);
    }

    public void SetCameraPos(int player)
    {
        Camera.main.transform.parent.position = playerTokens[player].transform.GetChild(1).transform.position;
        Camera.main.transform.parent.rotation = Quaternion.Euler(Vector3.up * 90 * playerTokens[player].mySpace.GetComponent<BoardSpace>().direction);
    }

    public void EnableActions(bool buttons)
    {
        actionChoice.SetActive(true);
        foreach (Button b in actionChoice.GetComponentsInChildren<Button>()) b.interactable = buttons;

        // set transperency on panel if not my turn
        Color32 panelColor = actionChoice.GetComponent<Image>().color;
        if (buttons) actionChoice.GetComponent<Image>().color = new Color32(panelColor.r, panelColor.g, panelColor.b, 255);
        else actionChoice.GetComponent<Image>().color = new Color32(panelColor.r, panelColor.g, panelColor.b, 123);
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

        int originalLoc = GameManager.inst.players[player].currentSpace - amount;

        StartCoroutine(MovePlayerToken(player, amount, originalLoc));
    }

    IEnumerator MovePlayerToken(int player, int amount, int prev)
    {
        yield return new WaitForSeconds(0.7F);
        Debug.Log("moving player");

        if (amount > 0)
        {
            playerTokens[player].AdvanceSpace();
            SetMinimapToken(player, prev + 1, prev);
            StartCoroutine(MovePlayerToken(player, amount - 1, prev + 1));
        }
        else
        {
            GameManager.inst?.CheckForWin(player);
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
        for(int i = 0; i < GameManager.inst.players.Length; i++)
        {
            Debug.Log("setting player location: " + GameManager.inst.players[i].username + " at index " + GameManager.inst.players[i].currentSpace);
            playerTokens[i].SetMyLocation(allSpaces[GameManager.inst.players[i].currentSpace]);
            SetMinimapToken(i, GameManager.inst.players[i].currentSpace, 0);
        }
    }

    public void SetMinimapToken(int player, int current, int previous)
    {
        if (previous > 0) minimapSpaces[previous - 1].RemoveFromLayout();
        if (current > 0) minimapSpaces[current - 1].AddToLayout(minimapTokens[player]);
    }

    public void SetRewardsText(int player)
    {
        turnText.transform.parent.gameObject.SetActive(true);
        turnText.text = GameManager.inst.players[player].username + "'s Winning Roll!";
    }

    public void ShowEndScreen(string winner)
    {
        endScreen.SetActive(true);
        endText.text = "<#C12089>" + winner + "</color> has Won!";
    }

    public void ShowPauseScreen()
    {
        pauseScreen.SetActive(!pauseScreen.activeInHierarchy);
    }

    public void QuitGame()
    {
        ServerManager.server?.LeaveRoom();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void DisconnectedUI(int player)
    {
        uiObjects[player].transform.GetChild(1).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
    }

    public void EnableMinigameScreen(Sprite img)
    {
        if (img != null)
        {
            minigameScreen.SetActive(true);
            minigameScreen.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = img;
        }
        else
        {
            minigameScreen.SetActive(false);
        }
    }
}
