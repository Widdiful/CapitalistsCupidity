using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalDatabase : MonoBehaviour {

    [System.Serializable]
	public class LocalDatabaseItem {
        public string companyName;
        public string score;
    }

    public List<LocalDatabaseItem> databaseFree = new List<LocalDatabaseItem>();
    public List<LocalDatabaseItem> databaseGold = new List<LocalDatabaseItem>();
    public List<LocalDatabaseItem> databaseTime = new List<LocalDatabaseItem>();

    public static LocalDatabase instance;

    private void Awake() {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        savingAndLoading.instance.loadLeaderboards();
        //UpdateDatabase();
    }

    public void UpdateDatabase() {
        if (databaseFree.Count > 0) {
            databaseFree.Sort(SortByScore);
            databaseFree.Reverse();
            RemoteDatabase.instance.UploadScore(databaseFree[0].companyName, databaseFree[0].score, HighScoreManager.GameTypes.Free);
        }

        if (databaseGold.Count > 0) {
            databaseGold.Sort(SortByScore);
            databaseGold.Reverse();
            RemoteDatabase.instance.UploadScore(databaseGold[0].companyName, databaseGold[0].score, HighScoreManager.GameTypes.Gold);
        }

        if (databaseTime.Count > 0) {
            databaseTime.Sort(SortByScore);
            RemoteDatabase.instance.UploadScore(databaseTime[0].companyName, databaseTime[0].score, HighScoreManager.GameTypes.Time);
        }
    }

    static int SortByScore(LocalDatabaseItem item1, LocalDatabaseItem item2) {
        return item1.score.CompareTo(item2.score);
    }
}
