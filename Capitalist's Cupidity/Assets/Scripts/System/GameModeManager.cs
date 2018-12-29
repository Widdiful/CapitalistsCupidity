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

    private void FixedUpdate() {
        timeSpent += Time.fixedDeltaTime;
        timeSpentUnscaled += Time.fixedUnscaledDeltaTime;
        timeSpentString = Mathf.Floor(timeSpent / 3600.0f).ToString("00") + ":" + Mathf.Floor((timeSpent / 60.0f) % 60).ToString("00") + ":" + (timeSpent % 60).ToString("00");
        timeSpentUnscaledString = Mathf.Floor(timeSpentUnscaled / 3600.0f).ToString("00") + ":" + Mathf.Floor((timeSpentUnscaled / 60.0f) % 60).ToString("00") + ":" + (timeSpentUnscaled % 60).ToString("00");

        if (gameMode == GameModes.Time) {
            if (Director.Instance.numberOfMonths >= monthLimit) {
                FinishGame();
            }
        }
        else if (gameMode == GameModes.Gold) {
            if (PlayerStats.instance.GetPersonalFunds() >= moneyGoal) {
                FinishGame();
            }
        }
    }

    private void FinishGame() {
        Debug.Break();
    }
}
