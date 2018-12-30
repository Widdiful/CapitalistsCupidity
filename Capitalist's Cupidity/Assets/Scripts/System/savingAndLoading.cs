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
    public int fiveCoin;
    public List<FacilityInfo> facilityList;


}

public class savingAndLoading : MonoBehaviour
{
    Director directorScript;
    PlayerStats playerStatScript;
    SaveData saveData;
    FacilityList facilityScript;

    private void Start()
    {
        saveData = new SaveData();
        directorScript = Director.Instance;
        playerStatScript = PlayerStats.instance;
        facilityScript = FacilityList.instance;
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
        saveData.fiveCoin = playerStatScript.GetFiveCoin();
        saveData.facilityList = facilityScript.facilityList;
    }

    void writeData()
    {
        directorScript.employees = saveData.employees;
        directorScript.numberOfMonths = saveData.numberOfMonths;
        facilityScript.facilityList = saveData.facilityList;
        playerStatScript.SetPersonalFunds(saveData.personalFunds);
        playerStatScript.SetCompanyFunds(saveData.companyFunds);
        playerStatScript.SetFiveCoin(saveData.fiveCoin);
    }
}
