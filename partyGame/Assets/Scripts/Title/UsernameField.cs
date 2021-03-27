using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UsernameField : MyInputField
{
    new void Start()
    {
        base.Start();

        if (PlayerPrefs.HasKey("username"))
        {
            inputField.placeholder.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("username");
        }
    }

    public override void SubmitText()
    {
        if (inputField.text != "")
        {
            string newName = inputField.text.Replace(" ", "");
            if (newName.Length > 25) newName = newName.Substring(0, 25);

            PlayerPrefs.SetString("username", newName);
            ServerManager.server.ChangeUsername(newName);
            inputField.placeholder.GetComponent<TextMeshProUGUI>().text = newName;

            base.SubmitText();
        }
    }
}
