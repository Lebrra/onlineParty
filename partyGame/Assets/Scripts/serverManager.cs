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

    SocketIOComponent socket;

    char quote = '"';

    //public playerManager playerManager;

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
        socket.On("connectionmessage", onConnectionEstabilished);
        socket.On("playerConnected", OnPlayerConnected);
        socket.On("roomUsers", LoadRoomUsers);
        //socket.On("removeUser", removeUser);
        socket.On("joinedRoom", JoiningRoom);
        socket.On("roomNotFound", RoomError);
        socket.On("loadGame", LoadGame);

        socket.On("ping", Ping);

    }

    #region Connection/Room Functions

    // This is the listener function definition
    void onConnectionEstabilished(SocketIOEvent evt)
    {
        Debug.Log("Player is connected: " + evt.data.GetField("id"));

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

    public void SetReady()
    {
        socket.Emit("setReady");
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

    #endregion
}