using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Director : MonoBehaviour
{
    public static Director _instance;
    public Employee employeePrefab;
    public List<Employee> employees;
    public int employeePoolCount = 10;

    string[] names;

    public enum Positions { workstation, waterfountain, exit, desk, toilet, cafe};
    public Positions _pos;

    public GameObject Desk;
    public GameObject Toilet;
    public GameObject Cafe;
    public GameObject waterFountain;
    public GameObject Exit;

    public delegate void setPos(Positions pos);
    public static event setPos flockToExit;

    public delegate void affectHappiness(float value);
    public static event affectHappiness workerHappiness;

    public delegate void payWorkers();
    public static event payWorkers payTheGuys;

    bool flock = false;

    public int numberOfMonths = 0;
    int oldMonths;

    public Text months;
    private PlayerStats playerStats;

    List<Floor> floors;

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
        floors = FindObjectOfType<OfficeGenerator>().GetFloors();
        names = new string[3] {"Ed", "Lewis", "Jack" };
        employees = new List<Employee>();
        playerStats = GameObject.FindObjectOfType<PlayerStats>();

        for (int i = 0; i < employeePoolCount; i++)
        {
            employeePrefab = Instantiate(employeePrefab);
            employeePrefab.name = names[UnityEngine.Random.Range(0, names.Length)] + UnityEngine.Random.Range(100, 1000).ToString();
            employeePrefab.gameObject.SetActive(false);
            employees.Add(employeePrefab);
        }
        oldMonths = numberOfMonths;
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        months.text = "Number of months: " + numberOfMonths.ToString();
        if (Input.GetKeyDown(KeyCode.Space))
        {
              flock = !flock;
              Debug.Log("Updating pos");
              if (!flock)
              {
                  flockToExit(Positions.exit);
              }
        }
        if (workerHappiness != null)
        {
            workerHappiness(-1f);
        }

        if (numberOfMonths == oldMonths + 1)
        {
            updateFunds();
            oldMonths = numberOfMonths;
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

    public int totalActiveEmployees()
    {
        int total = 0;
        foreach(Employee emp in employees)
        {
            if(emp.gameObject.activeSelf)
            {
                total++;
            }
        }

        return total;
    }

    public int assignFloor()
    {
        return Random.Range(0, floors.Count - 1);
    }

    public GameObject assignFacilities(int floor, string facilityName, Employee emp)
    {
        for(int i = 0; i < floors[floor].facilities.Count; i++)
        {
            if (floors[floor].facilities[i].facilityInfo.facilityName == facilityName) 
            {
                return floors[floor].facilities[i].gameObject;
            }
    
        }

        return findClosestFacility(floor, facilityName, emp.Desk);
    }

    public GameObject findClosestFacility(int floor, string facilityName, GameObject assignedDesk)
    {
        GameObject best = null;
        var closeFacilities = FindObjectsOfType<Facility>();
        float closest = Mathf.Infinity;

        foreach(Facility fal in closeFacilities)
        {
            Vector3 direction = fal.transform.position - assignedDesk.transform.position;
            float squareDistance = direction.magnitude;

            if(squareDistance < closest && fal.facilityInfo.facilityName == facilityName)
            {
                closest = squareDistance;
                best = fal.gameObject;
            }
        }
        if (best != null)
        {
            return best;
        }
        else
        {
            return Exit;
        }
    }

    private void updateFunds()
    {
        float funds = 0;
        foreach (Facility facility in GameObject.FindObjectsOfType<Facility>())
        {
            funds -= facility.GetMonthlyExpense();
        }
        if (payTheGuys != null)
        {
            payTheGuys();
        }
        if (funds > 0)
        {
            playerStats.ChangePersonalFunds(funds);
        }
        playerStats.ChangeCompanyFunds(funds);
    }
}
