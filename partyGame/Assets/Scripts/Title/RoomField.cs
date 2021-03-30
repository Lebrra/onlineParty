using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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

            ServerManager.server.JoinRoom(code);
            base.SubmitText();

            LobbyManager.instance.roomEnterPanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject, null);
        inputField?.OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
