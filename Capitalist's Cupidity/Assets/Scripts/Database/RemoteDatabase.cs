using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteDatabase : MonoBehaviour {

    private string hostURL = "http://localhost/cupidity/";
    public string userID;
    public string userName;

    private void Update() {
        if (Input.GetKeyDown("s"))
            Register();

        if (Input.GetKeyDown("d"))
            RenamePlayer("test");
    }

    public void Register() {
        StartCoroutine(Registration());
    }

    public void RenamePlayer(string newName) {
        StartCoroutine(Rename(newName));
    }

    IEnumerator Rename(string newName) {
        WWWForm form = new WWWForm();
        form.AddField("playerID", userID);
        form.AddField("newName", newName);

        WWW www = new WWW(hostURL + "rename.php", form);
        yield return www;

        if (www.text == "0") { // SUCCESSFUL RENAME
            Debug.Log("Rename successful");
            userName = newName;
        }
        else {
            Debug.Log("Rename failed. Error " + www.text);
        }
    }

    IEnumerator Registration() {
        WWW www = new WWW(hostURL + "register.php");
        yield return www;
        
        if (www.text.StartsWith("ID")) { // SUCCESSFUL REGISTRATION
            Debug.Log("Registration successful");
            userID = www.text.Replace("ID", "");
            userName = "New Player";
            Debug.Log(www.text);
            Debug.Log(userID);
        }
        else {
            Debug.Log("Registration failed. Error " + www.text);
        }
    }
}
