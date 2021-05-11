using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameLoader : MonoBehaviour
{
    public static MinigameLoader gameInst;

    // Inheritance later maybe
    [Header("How to Play")]
    public GameObject howToPlayPanel;
    public GameObject[] playerReadyUI;  // children: 0 = fill, 1 = check, 2 = name, 3 = me
    public Button readyBtn;

    [Header("Game UI")]
    public GameObject[] playerScoreContainers;
    public string defaultScore = "";

    protected void Awake()
    {
        if (gameInst) Destroy(gameInst);
        gameInst = this;
    }

    protected void Start()
    {
        howToPlayPanel.SetActive(true);
        if (GameManager.inst) 
            LoadHowToPlay(GameManager.inst.myUser, GameManager.inst.players);
        readyBtn.gameObject.SetActive(false);
        Invoke("DelayReadyBtn", 2F);
    }

    protected void DelayReadyBtn()
    {
        readyBtn.gameObject.SetActive(true);
    }

    protected void LoadHowToPlay(int me, PlayerObject[] players)
    {
        for(int i = 0; i < 6; i++)
        {
            if (i < players.Length)
            {
                playerReadyUI[i].SetActive(true);
                playerReadyUI[i].transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = players[i].username;
                if (i == me) playerReadyUI[i].transform.GetChild(3).gameObject.SetActive(true);
                if (players[i].ready) playerReadyUI[i].transform.GetChild(1).gameObject.SetActive(true);

                playerScoreContainers[i].SetActive(true);
                string extraText = "";
                if (defaultScore != "") extraText = "<#AAA>:</color>";
                playerScoreContainers[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = players[i].username + extraText;
                playerScoreContainers[i].transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = defaultScore;
            }
            else
            {
                playerReadyUI[i].SetActive(false);
                playerScoreContainers[i].SetActive(false);
            }
        }
    }

    public void SetPlayerReady(int player)
    {
        playerReadyUI[player].transform.GetChild(1).gameObject.SetActive(true);
    }

    public void SendReady()
    {
        readyBtn.interactable = false;
        ServerManager.server?.SetReady();
    }

    protected virtual void LoadGameUI(int me, PlayerObject[] players)
    {
        Debug.Log("loading ui for minigame...");

    }

    public virtual void StartGame()
    {
        Debug.Log("start minigame!");
        howToPlayPanel.SetActive(false);
    }

    public virtual void ReceiveGameData(JSONObject data)
    {
        // depending on the game, this could be an int, float, bool
        int playerIndex = -1;
        if (int.TryParse(data.GetField("player").ToString().Trim(Quote.quote), out playerIndex))
            Debug.Log("Game data received: " + data.GetField("data").ToString());
        else Debug.LogError("player index invalid.", gameObject);
    }
}
