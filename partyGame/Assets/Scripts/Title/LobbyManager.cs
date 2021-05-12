using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using SocketIO;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    public GameObject mainPanel;
    public GameObject lobbyPanel;
    public GameObject roomEnterPanel;

    [Header("Join Objects")]
    public Animator errorRoomAnim;
    public GameObject joinInputField;

    [Header("Room Objects")]
    public TextMeshProUGUI roomName;
    public GameObject namePrefab;
    public GameObject namesContainer;
    Dictionary<string, GameObject> names;
    public UnityEngine.UI.Button playButton;

    public Animator copyAnim;

    void Awake()
    {
        instance = this;
        mainPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        roomEnterPanel.SetActive(false);

        names = new Dictionary<string, GameObject>();
    }

        // TEMP UPDATE FUNCTION
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void CreateGame()
    {
        ServerManager.server.CreateNewLobby();
        mainPanel.SetActive(false);
    }

    public void JoinGame()
    {
        mainPanel.SetActive(false);
        roomEnterPanel.SetActive(true);
        //StartCoroutine(DelayInputSelect());
    }

    public void JoinRoom(string roomName)
    {
        roomEnterPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        this.roomName.text = roomName;
    }

    public void ShowRoomError()
    {
        errorRoomAnim.SetTrigger("Flash");
        roomEnterPanel.SetActive(true);
        //StartCoroutine(DelayInputSelect());
    }

    IEnumerator DelayInputSelect()
    {
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(joinInputField, null);
    }

    public void UpdateRoomList(SocketIOEvent roomNames)
    {
        //check for someone left
        List<string> ids = new List<string>();

        for (int i = 0; i < roomNames.data.Count; i++)
        {
            JSONObject jsonData = roomNames.data.GetField(i.ToString());
            string id = jsonData.GetField("id").ToString().Trim('"');
            string username = jsonData.GetField("username").ToString().Trim('"');

            Debug.Log(username);
            ids.Add(id);

            if (names.ContainsKey(id))
            {
                //update name
                names[id].GetComponent<TextMeshProUGUI>().text = username;
            }
            else
            {
                //add new object
                GameObject newPlayer = Instantiate(namePrefab, namesContainer.transform);
                newPlayer.GetComponent<TextMeshProUGUI>().text = username;
                names.Add(id, newPlayer);
            }

            if (ServerManager.server.GetSocket() == id) names[id].GetComponent<TextMeshProUGUI>().color = Color.green;
        }

        if(names.Count > ids.Count)
        {
            Debug.Log("A user has left the lobby, removing them");
            foreach(var a in names.Keys)
            {
                if (!ids.Contains(a))
                {
                    //this user has left, remove them
                    Debug.Log("removed " + a);
                    GameObject temp = names[a];
                    names.Remove(a);
                    Destroy(temp);
                    //temp.SetActive(false);
                    break;
                }
            }
        }

        Debug.Log("member count: " + names.Count);
        if(names.Count >= 2)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }

    public void CloseRoom()
    {
        playButton.interactable = false;
        ServerManager.server.LeaveRoom();
        foreach (var a in names.Values) Destroy(a);
        names = new Dictionary<string, GameObject>();
        lobbyPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void CloseJoin()
    {
        roomEnterPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void PlayGame()
    {
        ServerManager.server.StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CopyRoomCode()
    {
        GUIUtility.systemCopyBuffer = roomName.text;
        copyAnim.SetTrigger("Flash");
    }
}
