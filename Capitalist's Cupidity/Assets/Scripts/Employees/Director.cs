using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Director : MonoBehaviour
{
    
    public static Director _instance;
    public Employee employeePrefab;
    public List<Employee> employees;
    public int employeePoolCount = 45;
    int currentEmployees = 0;
    int maxEmployees = 5;

    public List<GameObject> liftList;
    public Dictionary<int, Grid> getCurrentGrid;
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



    public delegate void affectHappiness(float value);
    public static event affectHappiness workerHappiness;

    public delegate void payWorkers();
    public static event payWorkers payTheGuys;

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
        transform.position = Exit.transform.position - Exit.transform.up / 2 + (Exit.transform.forward / 1.2f);

        names = new string[] { "Ed", "Lewis", "Jack", "Kostas", "Nick", "Linda", "Sandra", "Boris", "Derek", "Darren", "Derren", "Ross", "James", "Phil", "Oliver", "Harry", "Jacob", "Sally", "Charlie", "Thomas", "George", "Cam", "Oscar", "William", "Dave", "Olly", "Jake", "Connor", "Jacob", "Kyle", "Joe", "Reece", "Rhys", "Richard", "Damian", "Peter", "Beth", "Margaret", "Sophie", "Isabelle", "Snake", "Amelia", "Olivia", "Isla", "Emily", "Poppy", "Ava", "Jessica", "Susan", "Samantha", "Joanne", "Megan", "Victoria", "Lauren", "Michelle", "Tracy", "Emma", "Char", "Amuro", "Reccoa", "Kamille", "Kats", "Bright", "Mia", "Abigail", "Madison", "Charlotte", "Mary", "Patricia", "Four", "Takumi", "Barbara", "Susan" };
        employees = new List<Employee>();
        playerStats = PlayerStats.instance;

        for (int i = 0; i < employeePoolCount; i++)
        {
            employeePrefab = Instantiate(employeePrefab, transform);
            employeePrefab.name = names[UnityEngine.Random.Range(0, names.Length)] + " " + UnityEngine.Random.Range(100, 1000).ToString();
            if (employeePrefab.name.EndsWith("555")) {
                if (Random.Range(0, 555) == 0)
                    employeePrefab.name = "Kamen Rider 555";
            }
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

        for (int i = 0; i < floors.Count; i++)
        {
            Grid grid = floors[i].transform.GetChild(0).GetComponent<Grid>();
            getCurrentGrid.Add(i, grid);
        }
    }

    public int getCurrentEmployees()
    {
        return currentEmployees;
    }

    public void setCurrentEmployees(int value)
    { 
        currentEmployees += value;

        if(currentEmployees > maxEmployees)
        {
            currentEmployees = maxEmployees;
        }
        
    }

    public int getMaxEmployees()
    {
        return maxEmployees;
    }

    public void setMaxEmployees(int value)
    {
       
        maxEmployees += value;

        if(maxEmployees > employeePoolCount)
        {
            maxEmployees = employeePoolCount;
        }
        
    }

    // Update is called once per frame
    void Update ()
    {
        months.text = "Month: " + numberOfMonths.ToString();

        if (numberOfMonths == oldMonths + 1)
        {
            updateFunds();
            oldMonths = numberOfMonths;
        }

        StartCoroutine(spawnEmployee());
    }

    public IEnumerator spawnEmployee()
    {
        WaitForSeconds wait = new WaitForSeconds(2.0f);
        foreach (Employee employee in employees)
        {
            if (!employee.gameObject.activeSelf && currentEmployees < maxEmployees)
            {
                employee.gameObject.SetActive(true);
                currentEmployees++;
                break;
                
            }
            yield return wait;
        }
    }

    public void HireEmployee() {
        foreach (Employee employee in employees) {
            if (!employee.gameObject.activeSelf) {
                employee.gameObject.SetActive(true);
                currentEmployees++;
                break;
            }
        }
    }

    public float getGlobalHappiness()
    {
        float happiness = 0;
        int numOfActive = 0;
        foreach (Employee emp in employees)
        {
            if (emp.gameObject.activeSelf)
            {
                happiness += emp.getHappiness();
                numOfActive++;
            }
        }

        return happiness / numOfActive;
    }


    public int totalActiveEmployees()
    {
        return currentEmployees;
    }

    public int assignFloor()
    {
        return Random.Range(0, floors.Count - 1);
    }

    public Facility assignFacility(int floor, FacilityInfo.FacilityType facilityType, GameObject assignedDesk, Employee emp)
    {
        for(int i = 0; i < floors[floor].facilities.Count; i++)
        {
            if (floors[floor].facilities[i].facilityInfo.facilityType == facilityType) 
            {
                if (floors[floor].facilities[i].workPoint)
                {
                    if(floors[floor].facilities[i].facilityInfo.facilityType != FacilityInfo.FacilityType.WorkSpace)
                    {
                        return floors[floor].facilities[i];
                    }
                    else
                    {
                        foreach (Facility fal in floors[floor].facilities)
                        {
                            if(fal.facilityInfo.facilityType == FacilityInfo.FacilityType.WorkSpace && fal.employees.Count < 1)
                            {
                                return fal;
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("Shit broke fam");
                    return null;
                }
            }
    
        }
        return findClosestFacility(facilityType, assignedDesk, emp);

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

    public Facility findClosestFacility(FacilityInfo.FacilityType facilityType, GameObject assignedDesk, Employee emp)
    {
        Facility best = null;
        float closest = Mathf.Infinity;

        foreach(Facility fal in OfficeGenerator.instance.getFacilities())
        {
            Vector3 direction = fal.transform.position - assignedDesk.transform.position;
            float squareDistance = direction.magnitude;

            if(squareDistance < closest && fal.facilityInfo.facilityType == facilityType)
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
            return null;
        }
    }

    public Floor findClosestFloor(GameObject Obj)
    {
        Floor best = null;
        float closest = Mathf.Infinity;

        foreach (Floor fal in floors)
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
        foreach (Facility facility in OfficeGenerator.instance.getFacilities())
        {
            funds -= facility.GetMonthlyExpense();
        }
        if (payTheGuys != null)
        {
            payTheGuys();
        }
        foreach(Employee emp in employees) {
            funds += emp.getEarnings();
        }
        if (funds > 0)
        {
            playerStats.ChangePersonalFunds(funds);
        }
        playerStats.ChangeCompanyFunds(funds);
    }
}
