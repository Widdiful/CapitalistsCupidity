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

        LeaderboardItem newItem = Instantiate(leaderboardItemPrefab, content).GetComponent<LeaderboardItem>();
        if (newItem) {
            newItem.position = 1;
            newItem.playerName = gameType.ToString();
            newItem.companyName = scoreType.ToString();
            newItem.score = "$123,456,789";
            newItem.UpdateInformation();
        }
    }
}
