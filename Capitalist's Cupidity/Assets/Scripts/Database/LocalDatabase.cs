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
    }
}
