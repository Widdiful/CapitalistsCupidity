using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour {

	public enum GameModes { Free, Time, Gold };
    public GameModes gameMode;

    public float timeSpent = 0;             // Time spent in in-game speed
    public float timeSpentUnscaled = 0;     // Time spent in real-time speed
    public int monthLimit;
    public float moneyGoal;
    public string timeSpentString;
    public string timeSpentUnscaledString;

    private bool completed;

    public static GameModeManager instance;

    void Awake() {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }

    private void Start() {
        if (MainMenuManager.instance) {
            OfficeGenerator.instance.officeName = MainMenuManager.instance.companyName;
            PlayerStats.instance.SetFiveCoins(MainMenuManager.instance.fiveCoins);
            gameMode = (GameModes)MainMenuManager.instance.gameType;
            Destroy(MainMenuManager.instance.gameObject);
        }
    }

    private void FixedUpdate() {
        if (!completed) {
            timeSpent += Time.fixedDeltaTime;
            timeSpentUnscaled += Time.fixedUnscaledDeltaTime;
            timeSpentString = Mathf.Floor(timeSpent / 3600.0f).ToString("00") + ":" + Mathf.Floor((timeSpent / 60.0f) % 60).ToString("00") + ":" + (timeSpent % 60).ToString("00");
            timeSpentUnscaledString = Mathf.Floor(timeSpentUnscaled / 3600.0f).ToString("00") + ":" + Mathf.Floor((timeSpentUnscaled / 60.0f) % 60).ToString("00") + ":" + (timeSpentUnscaled % 60).ToString("00");

            if (gameMode == GameModes.Gold) {
                if (Director.Instance.numberOfMonths >= monthLimit) {
                    FinishGame();
                }
            }
            else if (gameMode == GameModes.Time) {
                if (PlayerStats.instance.GetPersonalFunds() >= moneyGoal) {
                    FinishGame();
                }
            }
        }
    }

    private void FinishGame() {
        completed = true;
        Time.timeScale = 0;
        Destroy(FindObjectOfType<TimeControls>());

        LocalDatabase.LocalDatabaseItem newScore = new LocalDatabase.LocalDatabaseItem();
        newScore.companyName = OfficeGenerator.instance.officeName;
        switch (gameMode) {
            case GameModes.Free:
                newScore.score = PlayerStats.instance.GetPersonalFunds().ToString("#0.00");
                LocalDatabase.instance.databaseFree.Add(newScore);
                break;
            case GameModes.Time:
                newScore.score = timeSpentString;
                LocalDatabase.instance.databaseTime.Add(newScore);
                break;
            case GameModes.Gold:
                newScore.score = PlayerStats.instance.GetPersonalFunds().ToString("#0.00");
                LocalDatabase.instance.databaseGold.Add(newScore);
                break;
        }
        UIManager.instance.OpenGameOverWindow();
        savingAndLoading.instance.save();
    }
}
