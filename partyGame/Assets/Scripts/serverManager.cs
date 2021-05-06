using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class ServerManager : MonoBehaviour
{
    public static ServerManager server;

    [TextArea(3, 10)]
    public string serverQuickRef;

    [TextArea(3, 10)]
    public string localFileLoc;

    SocketIOComponent socket;

    char quote = '"';

    private void Awake()
    {
        if (server) Destroy(gameObject);
        else server = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        socket = GetComponent<SocketIOComponent>();

        //playerManager = GameObject.Find("Player Manager").GetComponent<playerManager>();

        // The lines below setup 'listener' functions

            // Connection/Room Functions
        socket.On("connectionmessage", onConnectionEstabilished);
        socket.On("playerConnected", OnPlayerConnected);
        socket.On("roomUsers", LoadRoomUsers);
        socket.On("joinedRoom", JoiningRoom);
        socket.On("roomNotFound", RoomError);
        socket.On("ping", Ping);

            // Game Functions
        socket.On("loadGame", LoadGame);
        socket.On("loadTurnOrder", LoadTurnOrder);
        socket.On("declareTurn", DeclareTurn);
        socket.On("diceRoll", DiceRolled);
    }

    public string GetSocket()
    {
        return socket.sid;
    }

    #region Connection/Room Functions

    // This is the listener function definition
    void onConnectionEstabilished(SocketIOEvent evt)
    {
        Debug.Log("Player is connected: " + evt.data.GetField("id"));
        Debug.Log("Temp name: " + evt.data.GetField("name"));

        if (PlayerPrefs.HasKey("username"))
        {
            // send username to server to update
            string playerName = PlayerPrefs.GetString("username");
            Debug.Log("player " + playerName + " has connected");
            ChangeUsername(playerName);
        }
        else
        {
            Debug.Log("this player does not have a username");
            OnPlayerConnected(null);
        }
    }

    void OnPlayerConnected(SocketIOEvent evt)
    {
        //disable connector screen
        ConnectionPanel.inst?.DisablePanel();
    }

    public void ChangeUsername(string name)
    {
        socket.Emit("updateUsername", new JSONObject(quote + name + quote));
    }

    public void CreateNewLobby()
    {
        socket.Emit("createRoom");
    }

    public void JoinRoom(string roomName)
    {
        socket.Emit("joinRoom", new JSONObject(quote + roomName + quote));
    }

    public void LeaveRoom()
    {
        socket.Emit("leaveRoom");
    }

    void JoiningRoom(SocketIOEvent evt)
    {
        Debug.Log("Joined room: " + evt.data.GetField("name"));
        LobbyManager.instance.JoinRoom(evt.data.GetField("name").ToString().Trim('"'));
    }

    void LoadRoomUsers(SocketIOEvent evt)
    {
        Debug.Log("loading room usernames...");
        LobbyManager.instance.UpdateRoomList(evt);
    }

    void RoomError(SocketIOEvent evt)
    {
        LobbyManager.instance.ShowRoomError();
    }

    public void Ping(SocketIOEvent socketIOEvent)
    {
        //Debug.Log("Ping");
        //ConnectionIndicator.instance?.Ping();
    }

    #endregion


    #region Game Functions
    public void StartGame()
    {
        // tells server to start game
        socket.Emit("startGame");
    }

    void LoadGame(SocketIOEvent evt)
    {
        Debug.Log("The game has been started!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void DestroyGameData()
    {
        socket.Emit("deleteGame");
    }

    public void SetReady()
    {
        socket.Emit("setReady");
    }

    void LoadTurnOrder(SocketIOEvent evt)
    {
        PlayerObject[] players = new PlayerObject[evt.data.Count];
        int myIndex = -1;
        Debug.Log("Player Order List: ");

        for (int i = 0; i < evt.data.Count; i++)
        {
            JSONObject jsonData = evt.data.GetField(i.ToString());
            string id = jsonData.GetField("id").ToString().Trim('"');
            string username = jsonData.GetField("username").ToString().Trim('"');
            int index = -1;
            int.TryParse(jsonData.GetField("index").ToString().Trim('"'), out index);

            if (index != -1)
            {

                players[index] = new PlayerObject();
                players[index].id = id;
                players[index].username = username;
            }
            if (socket.sid == id) myIndex = index;
        }

        foreach (PlayerObject p in players) Debug.Log(p.username);
        GameManager.inst.LoadPlayerList(myIndex, players);
    }

    void DeclareTurn(SocketIOEvent evt)
    {
        // Server telling client it is [index] turn
        int turn = -1;
        if (int.TryParse(evt.data.GetField("turn").ToString().Trim('"'), out turn))
        {
            GameManager.inst.SetPlayerTurn(turn);
        }
        else Debug.LogError("turn order invalid.");
    }

    public void AdvanceTurn()
    {
        socket.Emit("advanceTurn");
    }

    public void RolledDice(int roll)
    {
        socket.Emit("diceRoll", new JSONObject(quote + roll.ToString() + quote));
    }

    void DiceRolled(SocketIOEvent evt)
    {
        int index = -1;
        int.TryParse(evt.data.GetField("index").ToString().Trim('"'), out index);

        int roll = -1;
        int.TryParse(evt.data.GetField("roll").ToString().Trim('"'), out roll);

        GameManager.inst?.PlayerRoll(index, roll);
    }

    #endregion
}