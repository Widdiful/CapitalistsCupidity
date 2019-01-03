using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;


public class SaveData
{
    public List<Employee> employees;
    public int numberOfMonths;

    public float personalFunds;
    public float companyFunds;

    public List<FacilityInfo> facilityList;

    public List<LocalDatabase.LocalDatabaseItem> dataBaseFree;
    public List<LocalDatabase.LocalDatabaseItem> dataBaseGold;
    public List<LocalDatabase.LocalDatabaseItem> dataBaseTime;

}

[System.Serializable]
public class ScoreData
{
    public List<LocalDatabase.LocalDatabaseItem> dataBaseFree = new List<LocalDatabase.LocalDatabaseItem>();
    public List<LocalDatabase.LocalDatabaseItem> dataBaseGold = new List<LocalDatabase.LocalDatabaseItem>();
    public List<LocalDatabase.LocalDatabaseItem> dataBaseTime = new List<LocalDatabase.LocalDatabaseItem>();
}

[System.Serializable]
public class ProfileData
{
    public string userID;
    public int fiveCoins;
}

public class savingAndLoading : MonoBehaviour
{
    Director directorScript;
    PlayerStats playerStatScript;
    SaveData saveData;
    FacilityList facilityScript;
    LocalDatabase localDatabaseScript;
    ScoreData scoreData = new ScoreData();
    public ProfileData profileData = new ProfileData();

    public static savingAndLoading instance;

    private void Awake() {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }

    private void Start()
    {
        saveData = new SaveData();
        directorScript = Director.Instance;
        playerStatScript = PlayerStats.instance;
        facilityScript = FacilityList.instance;
        localDatabaseScript = LocalDatabase.instance;
    }



    public void save()
    {
        readData();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveFile.dat");
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void load()
    {
        if (File.Exists(Application.persistentDataPath + "/saveFile.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveFile.dat", FileMode.Open);
            saveData = (SaveData)bf.Deserialize(file);
            file.Close();
            writeData();
            Debug.Log("Save loaded!");
        }
        else
        {
            Debug.Log("No save found!");
        }
    }

    void readData()
    {
        saveData.employees = directorScript.employees;
        saveData.numberOfMonths = directorScript.numberOfMonths;

        saveData.personalFunds = playerStatScript.GetPersonalFunds();
        saveData.companyFunds = playerStatScript.GetCompanyFunds();

        saveData.facilityList = facilityScript.facilityList;

        saveData.dataBaseFree = LocalDatabase.instance.databaseFree;
        saveData.dataBaseGold = LocalDatabase.instance.databaseGold;
        saveData.dataBaseTime = LocalDatabase.instance.databaseTime;
    }

    void writeData()
    {
        directorScript.employees = saveData.employees;
        directorScript.numberOfMonths = saveData.numberOfMonths;

        facilityScript.facilityList = saveData.facilityList;

        playerStatScript.SetPersonalFunds(saveData.personalFunds);
        playerStatScript.SetCompanyFunds(saveData.companyFunds);

        LocalDatabase.instance.databaseFree = saveData.dataBaseFree;
        LocalDatabase.instance.databaseGold = saveData.dataBaseGold;
        LocalDatabase.instance.databaseTime = saveData.dataBaseTime;
    }

    public void saveLeaderboards() {
        scoreData.dataBaseFree = LocalDatabase.instance.databaseFree;
        scoreData.dataBaseGold = LocalDatabase.instance.databaseGold;
        scoreData.dataBaseTime = LocalDatabase.instance.databaseTime;

        // save scores to file
        FileStream fs = new FileStream("scores.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, scoreData);
        fs.Close();
    }

    public void loadLeaderboards() {
        if (File.Exists("scores.dat")) {
            using (Stream stream = File.Open("scores.dat", FileMode.Open)) {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                scoreData = (ScoreData)bformatter.Deserialize(stream);
            }

            LocalDatabase.instance.databaseFree = scoreData.dataBaseFree;
            LocalDatabase.instance.databaseGold = scoreData.dataBaseGold;
            LocalDatabase.instance.databaseTime = scoreData.dataBaseTime;

            LocalDatabase.instance.UpdateDatabase();
        }
    }

    public void saveProfileData() {
        profileData.userID = RemoteDatabase.instance.userID;
        profileData.fiveCoins = MainMenuManager.instance.fiveCoins;

        // save data to file
        FileStream fs = new FileStream("profile.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, profileData);
        fs.Close();
    }

    public void loadProfileData() {
        if (File.Exists("profile.dat")) {
            using (Stream stream = File.Open("profile.dat", FileMode.Open)) {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                profileData = (ProfileData)bformatter.Deserialize(stream);
            }

            RemoteDatabase.instance.userID = profileData.userID;
            MainMenuManager.instance.fiveCoins = profileData.fiveCoins;
            MainMenuManager.instance.coinText.text = "Owned 5Coins: " + MainMenuManager.instance.fiveCoins.ToString("n0");
        }
    }
}
