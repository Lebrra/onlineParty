using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomField : MyInputField
{
    public override void SubmitText()
    {
        if (inputField.text != "")
        {
            //check for typos
            string firstLetter = inputField.text[0].ToString();
            string code = inputField.text.ToLower();
            code = firstLetter.ToUpper() + code.Substring(1);
            Debug.Log("Room code received: " + code);

            ServerManager.server.JoinRoom(inputField.text);
            base.SubmitText();
        }
    }
}
