using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class nh_network : MonoBehaviour
{
    public static nh_network server;

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
        socket.On("roomUsers", loadRoomUsers);
        socket.On("removeUser", removeUser);
        socket.On("createdRoom", createdRoom);
        socket.On("roomNotFound", roomError);
        socket.On("loadGame", loadGame);

        socket.On("ping", ping);

    }

    #region Connection/Room Functions

    // This is the listener function definition
    void onConnectionEstabilished(SocketIOEvent evt)
    {
        Debug.Log("Player is connected: " + evt.data.GetField("id"));
    }

    public void createNewLobby()
    {
        //socket.Emit("createNewLobby");
        socket.Emit("createRoom");
    }

    public void joinRoom(string roomName)
    {
        //socket.Emit("joinLobby");
        socket.Emit("joinRoom", new JSONObject(quote + roomName + quote));
    }

    public void leaveRoom()
    {
        socket.Emit("leaveRoom");
    }

    void createdRoom(SocketIOEvent evt)
    {
        Debug.Log("Created new room: " + evt.data.GetField("name"));
        //LobbyFunctions.inst.enterRoom(evt.data.GetField("name").ToString().Trim('"'));
    }

    void roomError(SocketIOEvent evt)
    {
        //LobbyFunctions.inst.showRoomError();
    }

    public void ping(SocketIOEvent socketIOEvent)
    {
        //Debug.Log("Ping");
        //ConnectionIndicator.instance?.Ping();
    }

    #endregion
    #region Username Functions

    public void newUsername(string name)
    {
        name = quote + name + quote;

        JSONObject test = new JSONObject(name);
        socket.Emit("updateUsername", test);
    }

    void loadRoomUsers(SocketIOEvent evt)
    {
        Debug.Log("loading room usernames...");
        //ua.removeAllUsernames();

        for (int i = 0; i < evt.data.Count; i++)
        {
            JSONObject jsonData = evt.data.GetField(i.ToString());

            Debug.Log(jsonData.GetField("username"));
            //ua.addUsername(jsonData.GetField("id").ToString().Trim('"'), jsonData.GetField("username").ToString().Trim('"'));
        }
    }


    void removeUser(SocketIOEvent evt)
    {
        //ua.removeUsername(evt.data.GetField("id").ToString().Trim('"'));
    }
    #endregion
    #region Game Functions
    public void startGame()
    {
        // tells server to start game
        socket.Emit("startGame");
    }

    public void setReady()
    {
        socket.Emit("setReady");
    }

    void loadGame(SocketIOEvent evt)
    {
        Debug.Log("The game has been started!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void destroyGameData()
    {
        socket.Emit("deleteGame");
    }

    #endregion
}