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
}

public class savingAndLoading : MonoBehaviour
{
    Director directorScript;
    PlayerStats playerStatScript;
    SaveData saveData;

    private void Start()
    {
        saveData = new SaveData();
        directorScript = GameObject.Find("Director").GetComponent<Director>();
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

    public void readData()
    {
        saveData.employees = directorScript.employees;
        saveData.numberOfMonths = directorScript.numberOfMonths;
    }

    public void writeData()
    {
        directorScript.employees = saveData.employees;
        directorScript.numberOfMonths = saveData.numberOfMonths;
    }
}
