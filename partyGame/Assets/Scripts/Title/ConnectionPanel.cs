using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPanel : MonoBehaviour
{
    public static ConnectionPanel inst;

    private void Awake()
    {
        inst = this;
    }

    public void DisablePanel()
    {
        inst = null;
        gameObject.SetActive(false);
    }
}
