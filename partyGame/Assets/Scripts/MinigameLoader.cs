using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("End Game")]
    public TextMeshProUGUI winnerText;
    public GameObject endPanel;

    int waitingTime = 60;
    bool waiting = false;

    [Header("Minigame Properties")]
    public bool minigameState = false;

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
                playerReadyUI[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = players[i].username;
                if (i == me) playerReadyUI[i].transform.GetChild(3).gameObject.SetActive(true);
                if (players[i].ready) playerReadyUI[i].transform.GetChild(1).gameObject.SetActive(true);

                playerScoreContainers[i].SetActive(true);
                string extraText = "";
                if (defaultScore != "") extraText = "<#AAA>:</color>";
                playerScoreContainers[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = players[i].username + extraText;
                playerScoreContainers[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = defaultScore;
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
        {
            Debug.Log("Game data received: " + data.GetField("data").ToString());
            playerScoreContainers[playerIndex].transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = data.GetField("data").ToString().Trim(Quote.quote);
        }
        else Debug.LogError("player index invalid.", gameObject);
    }

    public void SendFinalScore(string score)
    {
        ServerManager.server?.SendMinigameData(score);
        ServerManager.server?.SetReady();
    }

    public void GetWinner(int index)
    {
        winnerText.gameObject.SetActive(true);

        if (index < 0)
        {
            Debug.Log("Nobody won!");
            winnerText.text = "Nobody won...";
        }
        else
        {
            Debug.Log(GameManager.inst.players[index].username + " has won the minigame!");
            string playerColor = "<#" + ColorUtility.ToHtmlStringRGB(playerScoreContainers[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color) + ">";
            winnerText.text = playerColor + GameManager.inst?.players[index].username + "</color> has won!";

            playerScoreContainers[index].transform.GetChild(2).gameObject.SetActive(true);
        }

        // start coroutine to enable readycheck screen
        StartCoroutine(EnableEndBtn());
    }

    IEnumerator EnableEndBtn()
    {
        yield return new WaitForSeconds(3F);
        endPanel.SetActive(true);

        StartCoroutine(ConnectionTimer());
    }

    IEnumerator ConnectionTimer()
    {
        yield return new WaitForSeconds(1F);
        waitingTime--;

        if(waitingTime == 0 && !waiting)
        {
            ReturnReadyCheck();
        }
        //StartCoroutine(ConnectionTimer());
        if (waitingTime > -1)
        {
            endPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = waitingTime.ToString();
            StartCoroutine(ConnectionTimer());
        }
    }

    public void ReturnReadyCheck()
    {
        ServerManager.server?.SetReady();
        endPanel.GetComponent<Button>().enabled = false;
        endPanel.GetComponent<Image>().color = new Color32(0, 0, 0, 100);

        GameObject temp = endPanel.transform.GetChild(0).gameObject;
        temp.GetComponent<LoadingAnim>().enabled = false;
        temp.GetComponent<TextMeshProUGUI>().text = "Waiting for players";
        temp.GetComponent<LoadingAnim>().enabled = true;
        waiting = true;
    }

    public void DisconnectedUI(int player)
    {
        playerReadyUI[player].transform.GetChild(2).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
        playerScoreContainers[player].transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
        playerScoreContainers[player].transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "-1000";
    }
}
