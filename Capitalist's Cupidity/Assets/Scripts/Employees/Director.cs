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

    public List<GameObject> liftList;
    public Dictionary<int, Grid> getCurrentGrid;
    public List<Grid> gridList;
    public Dictionary<int, GameObject> Lifts;
    public Dictionary<int, GameObject> facilities;
    public List<Floor> floors;


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
        Exit = GameObject.Find("Entrance");
        transform.position = Exit.transform.position + (Exit.transform.forward / 1.2f);

        names = new string[3] { "Ed", "Lewis", "Jack" };
        employees = new List<Employee>();
        playerStats = PlayerStats.instance;

        for (int i = 0; i < employeePoolCount; i++)
        {
            employeePrefab = Instantiate(employeePrefab, transform);
            employeePrefab.name = names[UnityEngine.Random.Range(0, names.Length)] + UnityEngine.Random.Range(100, 1000).ToString();
            employeePrefab.gameObject.SetActive(false);
            employees.Add(employeePrefab);
        }
        oldMonths = numberOfMonths;


        liftList = OfficeGenerator.instance.lifts;
        floors = OfficeGenerator.instance.GetFloors();

        Lifts = new Dictionary<int, GameObject>();

        for (int i = 0; i < liftList.Count; i++)
        {
            GameObject obj = findObjectOnTargetLevel(liftList, i);
            Lifts.Add(i, obj);
        }

        getCurrentGrid = new Dictionary<int, Grid>();
        gridList = new List<Grid>();

        for (int i = 0; i < floors.Count; i++)
        {
            Grid grid = floors[i].GetComponentInChildren<Grid>();
            gridList.Add(grid);
        }

        for (int i = 0; i < gridList.Count; i++)
        {
            Grid grid = gridList[i];
            getCurrentGrid.Add(i, grid);
        }

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

    public GameObject assignFacilities(int floor, string facilityName, GameObject obj)
    {
        for(int i = 0; i < floors[floor].facilities.Count; i++)
        {
            if (floors[floor].facilities[i].facilityInfo.facilityName == facilityName) 
            {
                return floors[floor].facilities[i].gameObject.transform.GetChild(0).gameObject;
            }
    
        }

        return findClosestFacility(facilityName, obj);
        
    }

    public GameObject findObjectOnTargetLevel(List<GameObject> objects, int target)
    {
        foreach (GameObject obj in objects)
        {
            if (findClosestFloor(obj).floorNo == target)
            {
                return obj;
            }
        }

        return null;
    }

    public GameObject findClosestFacility(string facilityName, GameObject assignedDesk)
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

    public Floor findClosestFloor(GameObject Obj)
    {
        Floor best = null;
        var closeFacilities = FindObjectsOfType<Floor>();
        float closest = Mathf.Infinity;

        foreach (Floor fal in closeFacilities)
        {
            Vector3 direction = fal.transform.position - Obj.transform.position;
            float squareDistance = direction.magnitude;

            if (squareDistance < closest)
            {
                closest = squareDistance;
                best = fal;
            }
        }
        if (best != null)
        {
            return best;
        }
        else
        {
            Debug.Log("Could not find floor");
            return null;
        }
    }

    public GameObject findClosestGObj(List<GameObject> objects)
    {
        GameObject best = null;
        float closest = Mathf.Infinity;

        foreach (GameObject obj in objects)
        {
            Vector3 direction = obj.transform.position - transform.position;
            float squareDistance = direction.magnitude;

            if (squareDistance < closest)
            {
                closest = squareDistance;
                best = obj.gameObject;
            }
        }

        return best;
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
