using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SocketIO;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    public GameObject mainPanel;
    public GameObject lobbyPanel;
    public GameObject roomEnterPanel;

    public Animator errorRoomAnim;

    [Header("Room Objects")]
    public TextMeshProUGUI roomName;
    public GameObject namePrefab;
    public GameObject namesContainer;
    Dictionary<string, GameObject> names;

    void Awake()
    {
        instance = this;
        mainPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        roomEnterPanel.SetActive(false);

        names = new Dictionary<string, GameObject>();
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
        }

        if(names.Count > ids.Count)
        {
            Debug.Log("A user has left the lobby, removing them");
            foreach(var a in names.Keys)
            {
                if (!ids.Contains(a))
                {
                    //this user has left, remove them
                    Destroy(names[a]);
                    names.Remove(a);
                }
            }
        }
    }

    public void CloseRoom()
    {
        foreach (var a in names.Values) Destroy(a);
        names = new Dictionary<string, GameObject>();
        lobbyPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
