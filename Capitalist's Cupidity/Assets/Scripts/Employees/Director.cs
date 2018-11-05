using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public Employee employeePrefab;
    List<Employee> employees;
    public int employeePoolCount = 20;

	// Use this for initialization
	void Start ()
    {
        employees = new List<Employee>();

        for(int i = 0; i < employeePoolCount; i++)
        {
            employeePrefab = Instantiate(employeePrefab);
            employeePrefab.gameObject.SetActive(false);
            employees.Add(employeePrefab);
            employeePoolCount++;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void setEmployeeTargetPos()
    {

    }

    void flockingManager()
    { }
}
