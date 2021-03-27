using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyInputField : MonoBehaviour
{
    protected TMP_InputField inputField;

    protected void Start()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    protected void Update()
    {
        if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && inputField.text != "")
        {
            SubmitText();
        }
    }

    public virtual void SubmitText()
    {
        inputField.text = "";
    }

    public void ClearText()
    {
        inputField.text = "";
    }
}
