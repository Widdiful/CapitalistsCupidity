using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Director : MonoBehaviour
{
    public static Director _instance;
    public Employee employeePrefab;
    List<Employee> employees;
    public int employeePoolCount = 10;

    string[] names;

    public enum Positions { workstation, waterfountain, exit };
    Positions _pos;


    public delegate void setPos(Positions pos);
    public static event setPos updatePos;


    public static Director Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new Director();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        names = new string[3] {"Ed", "Lewis", "Jack" };

        
        employees = new List<Employee>();

        for(int i = 0; i < employeePoolCount; i++)
        {
            employeePrefab = Instantiate(employeePrefab);
            employeePrefab.name = names[UnityEngine.Random.Range(0, 3)] + UnityEngine.Random.Range(0, 100).ToString();
            employeePrefab.gameObject.SetActive(false);
            employees.Add(employeePrefab);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Updating pos");
            updatePos(Positions.exit);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach(Employee employee in employees)
            {
                if(!employee.gameObject.activeSelf)
                {
                    employee.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }


    void flockingManager()
    { }
}
