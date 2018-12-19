using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour {

    public Transform content;
    public GameObject leaderboardItemPrefab;

    public enum GameTypes { Time, Gold, Free };
    public GameTypes gameType;

    public enum ScoreTypes { Local, Global };
    public ScoreTypes scoreType;

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
        foreach (Transform button in content) {
            Destroy(button.gameObject);
        }

        StartCoroutine(LoadScoresAwait());
    }

    IEnumerator LoadScoresAwait() {

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
            yield return new WaitForSecondsRealtime(0.1f);
        }

        int position = 1;

        foreach (string line in RemoteDatabase.instance.dbLines) {

            if (line.Length > 0) {
                LeaderboardItem newItem = Instantiate(leaderboardItemPrefab, content).GetComponent<LeaderboardItem>();
                newItem.position = position;
                newItem.playerName = RemoteDatabase.instance.GetDBLineValue(line, "player:");
                newItem.companyName = RemoteDatabase.instance.GetDBLineValue(line, "company:");
                newItem.score = RemoteDatabase.instance.GetDBLineValue(line, "score:");
                newItem.UpdateInformation();
                position++;
            }
        }
    }
}
