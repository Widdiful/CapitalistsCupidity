using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour {

    public Transform content;
    public GameObject leaderboardItemPrefab;
    public GameObject failText;

    public enum GameTypes { Time, Gold, Free };
    public GameTypes gameType;

    public enum ScoreTypes { Local, Global };
    public ScoreTypes scoreType;

    public RectTransform loadingImage;

    private void Start() {
        LoadScores();
    }

    public void GameTime() {
        gameType = GameTypes.Time;
        LoadScores();
    }

    public void GameGold() {
        gameType = GameTypes.Gold;
        LoadScores();
    }

    public void GameFree() {
        gameType = GameTypes.Free;
        LoadScores();
    }

    public void ScoreLocal() {
        scoreType = ScoreTypes.Local;
        LoadScores();
    }

    public void ScoreGlobal() {
        scoreType = ScoreTypes.Global;
        LoadScores();
    }

    public void LoadScores() {
        failText.SetActive(false);
        foreach (Transform button in content) {
            Destroy(button.gameObject);
        }

        if (scoreType == ScoreTypes.Global)
            StartCoroutine(LoadScoresGlobal());
        else
            LoadScoresLocal();
    }

    private void LoadScoresLocal() {
        List<LocalDatabase.LocalDatabaseItem> db = new List<LocalDatabase.LocalDatabaseItem>();

        switch (gameType) {
            case GameTypes.Free:
                db = LocalDatabase.instance.databaseFree;
                db.Sort(SortByScore);
                db.Reverse();
                break;
            case GameTypes.Gold:
                db = LocalDatabase.instance.databaseGold;
                db.Sort(SortByScore);
                db.Reverse();
                break;
            case GameTypes.Time:
                db = LocalDatabase.instance.databaseTime;
                db.Sort(SortByScore);
                break;
        }

        int position = 1;

        foreach (LocalDatabase.LocalDatabaseItem item in db) {
                LeaderboardItem newItem = Instantiate(leaderboardItemPrefab, content).GetComponent<LeaderboardItem>();
                newItem.position = position;
                newItem.playerName = RemoteDatabase.instance.userName;
                newItem.companyName = item.companyName;
                if (gameType == GameTypes.Time)
                    newItem.score = item.score.ToString();
                else
                    newItem.score = "$" + float.Parse(item.score).ToString("n0");
                newItem.UpdateInformation();
                position++;
        }
    }

    static int SortByScore(LocalDatabase.LocalDatabaseItem item1, LocalDatabase.LocalDatabaseItem item2) {
        return item1.score.CompareTo(item2.score);
    }

    IEnumerator LoadScoresGlobal() {

        loadingImage.gameObject.SetActive(true);
        failText.SetActive(false);
        loadingImage.rotation = Quaternion.identity;

        switch (gameType) {
            case GameTypes.Free:
                RemoteDatabase.instance.GetScoresFree();
                break;
            case GameTypes.Gold:
                RemoteDatabase.instance.GetScoresGold();
                break;
            case GameTypes.Time:
                RemoteDatabase.instance.GetScoresTime();
                break;
        }

        while (!RemoteDatabase.instance.fetchingComplete) {
            loadingImage.Rotate(0, 0, -10);
            yield return new WaitForSecondsRealtime(0.01f);
        }

        loadingImage.gameObject.SetActive(false);

        if (RemoteDatabase.instance.dbLines.Length <= 1) {
            failText.SetActive(true);
        }

        else {
            int position = 1;

            foreach (string line in RemoteDatabase.instance.dbLines) {

                if (line.Length > 0) {
                    LeaderboardItem newItem = Instantiate(leaderboardItemPrefab, content).GetComponent<LeaderboardItem>();
                    newItem.position = position;
                    newItem.playerName = RemoteDatabase.instance.GetDBLineValue(line, "player:");
                    newItem.companyName = RemoteDatabase.instance.GetDBLineValue(line, "company:");
                    if (gameType == GameTypes.Time)
                        newItem.score = RemoteDatabase.instance.GetDBLineValue(line, "score:");
                    else
                        newItem.score = "$" + int.Parse(RemoteDatabase.instance.GetDBLineValue(line, "score:")).ToString("n0");
                    newItem.UpdateInformation();
                    position++;
                }
            }
        }
    }
}
