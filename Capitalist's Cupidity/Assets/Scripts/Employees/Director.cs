using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Director : MonoBehaviour
{
    public static Director _instance;
    public Employee employeePrefab;
    public List<Employee> employees;
    public int employeePoolCount = 10;

    string[] names;

    public enum Positions { workstation, waterfountain, exit };
    Positions _pos;


    public delegate void setPos(Positions pos);
    public static event setPos updatePos;

    bool flock = false;

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
            employeePrefab.name = names[UnityEngine.Random.Range(0, 3)] + UnityEngine.Random.Range(100, 1000).ToString();
            employeePrefab.gameObject.SetActive(false);
            employees.Add(employeePrefab);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            flock = !flock;
            Debug.Log("Updating pos");
            if (!flock)
            {
                updatePos(Positions.exit);
            }
            //else
            //{
               // updatePos(Positions.waterfountain);
            //}
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


    public Vector3 flockingAlignment(Employee flocker)
    {
        if(flock)
        {
            Vector3 averageVelocity = Vector3.zero;
            flocker.velocity = Vector3.zero;
          
            foreach(Employee emp in employees)
            {
                if (emp != flocker)
                {
                    averageVelocity += emp.velocity;
                }
            }

            return averageVelocity = (averageVelocity / (employees.Count - 1)).normalized;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 flockingCohesion(Employee flocker)
    {
        if (flock)
        {
            Vector3 averagePos = Vector3.zero;

            foreach (Employee emp in employees)
            {
                if (emp != flocker)
                {
                    averagePos += emp.transform.position;
                }
            }

            return (averagePos / (employees.Count - 1)).normalized;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 flockingSeperation(Employee flocker)
    {
        if (flock)
        {
            Vector3 seperation = Vector3.zero;

            foreach (Employee emp in employees)
            {
                if(emp != flocker)
                {
                    if(Vector3.Distance(flocker.transform.position, emp.transform.position) <= 5)
                    {
                        seperation -= (emp.transform.position - flocker.transform.position);
                    }
                }
            }

            return seperation.normalized * 5;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
