using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomField : MyInputField
{
    public override void SubmitText()
    {
        ServerManager.server.JoinRoom(inputField.text);

        base.SubmitText();
    }
}
