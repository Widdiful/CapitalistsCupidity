using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteDatabase : MonoBehaviour {

    private string hostURL = "widdiful.co.uk/php/cupidity/";
    public string userID;
    public string userName;

    public string[] dbLines;
    public bool fetchingComplete;

    public static RemoteDatabase instance;

    private void Awake() {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }

    private void Start() {
        if (userID == "")
            Register();

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

    public void GetScoresFree() {
        StartCoroutine(FetchScores(HighScoreManager.GameTypes.Free));
    }

    public void GetScoresTime() {
        StartCoroutine(FetchScores(HighScoreManager.GameTypes.Time));
    }

    public void GetScoresGold() {
        StartCoroutine(FetchScores(HighScoreManager.GameTypes.Gold));
    }

    IEnumerator FetchScores(HighScoreManager.GameTypes gameType) {
        fetchingComplete = false;
        string tableName = "";
        switch (gameType) {
            case HighScoreManager.GameTypes.Free:
                tableName = "highscoresfree";
                break;
            case HighScoreManager.GameTypes.Time:
                tableName = "highscorestime";
                break;
            case HighScoreManager.GameTypes.Gold:
                tableName = "highscoresgold";
                break;

        }
        WWWForm form = new WWWForm();
        form.AddField("tableName", tableName);
        WWW www = new WWW(hostURL + "getScores.php", form);
        yield return www;

        dbLines = www.text.Split(';');
        fetchingComplete = true;
    }

    public string GetDBLineValue(string line, string index) {
        string value = line.Substring(line.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }

    public void UploadScore(string companyName, string score, HighScoreManager.GameTypes gameType) {
        StartCoroutine(UploadScoreAwait(companyName, score, gameType));
    }

    IEnumerator UploadScoreAwait(string companyName, string score, HighScoreManager.GameTypes gameType) {
        string tableName = "";
        switch (gameType) {
            case HighScoreManager.GameTypes.Free:
                tableName = "highscoresfree";
                break;
            case HighScoreManager.GameTypes.Time:
                tableName = "highscorestime";
                break;
            case HighScoreManager.GameTypes.Gold:
                tableName = "highscoresgold";
                break;

        }

        WWWForm form = new WWWForm();
        form.AddField("playerID", userID);
        form.AddField("companyName", companyName);
        form.AddField("score", score);
        form.AddField("tableName", tableName);

        WWW www = new WWW(hostURL + "uploadScore.php", form);
        yield return www;

        if (www.text == "0") {
            Debug.Log("Score not updated.");
        }
        else if (www.text == "1") {
            Debug.Log("Score updated.");
        }
    }
}
