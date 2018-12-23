﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenameMenu : MonoBehaviour {

    public InputField inputText;

    public void Open() {
        inputText.text = RemoteDatabase.instance.userName;
    }

	public void Rename() {
        if (inputText.text != "") {
            RemoteDatabase.instance.RenamePlayer(inputText.text);
            inputText.text = "";
            Close();
        }   
    }

    public void Close() {
        
    }
}
