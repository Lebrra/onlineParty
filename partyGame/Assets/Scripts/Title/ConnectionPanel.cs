using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPanel : MonoBehaviour
{
    public static ConnectionPanel inst;

    private void Awake()
    {
        inst = this;
        if (ServerManager.server) if (ServerManager.server.connected) DisablePanel();
    }

    public void DisablePanel()
    {
        //inst = null;
        gameObject.SetActive(false);
    }

    public void EnablePanel()
    {
        gameObject.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
