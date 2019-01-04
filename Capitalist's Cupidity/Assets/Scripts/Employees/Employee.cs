using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Employee : MonoBehaviour
{
    AudioSource audioSource;
    public GameObject targetObject;
    public Vector3 velocity = Vector3.zero;

    float maxTurnSpeed = 20.0f;
    float maxMoveSpeed = 1.0f;

    float happiness = 100;


    Actions Work;
    Actions Leave;
    Actions goToToilet;
    Actions getFood;
    Actions drinkADrink;

    public float needToWork = 100.0f;
    public float homeTime = 0.0f;
    public float needForBathroom = 0.0f;
    public float hunger = 0.0f;
    public float thirst = 0.0f;

    float needToWorkModifier = 0.0f;
    float bladderModifier = 0.0f;
    float hungerModifier = 0.0f;
    float thirstModifier = 0.0f;

    public Dictionary<FacilityInfo.FacilityType, Facility> assignedWorkPoints = new Dictionary<FacilityInfo.FacilityType, Facility>();

    public GameObject Exit;
    int exitFloor;

    public List<Actions> actions;

    bool isBootLicker;

    public float moneyInBank = 0.0f;
    int salary;
    float moneyEarnedForCompany;

    public int assignedFloor;
    public int currentFloor;
    int targetFloor;
    
    public bool pathComplete;

    Pathfinding pathFinding;
    Vector3 followPath;
    public int currentPathPoint = 0;

    public List<GameObject> liftList;
    public Dictionary<int, Grid> getCurrentGrid;
    public Dictionary<int, GameObject> Lifts;
    public Dictionary<int, GameObject> facilities;
    public List<Floor> floors;

    float targetOffset = 1.55f;
    bool quit = false;
    Renderer rend;
    
    public static void Swap<T>(List<T> list, int index1, int index2)
    {
        if (list != null)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        assignedFloor = Director.Instance.assignFloor();
        AssignFacility(FacilityInfo.FacilityType.WorkSpace);
        AssignFacility(FacilityInfo.FacilityType.Toilets);
        AssignFacility(FacilityInfo.FacilityType.Catering);
        AssignFacility(FacilityInfo.FacilityType.WaterFountain);
        Exit = Director.Instance.Exit;


        //Create actions
        actions = new List<Actions>();

        Work = new Actions();
        Leave = new Actions();
        goToToilet = new Actions();
        getFood = new Actions();
        drinkADrink = new Actions();

        //Create delegates for the actions
        Work.empFunc += sitAtDesk;
        Work.priority = needToWork;

        Leave.empFunc += goHome;
        Leave.priority = homeTime;

        goToToilet.empFunc += goToBathroom;
        goToToilet.priority += needForBathroom;

        getFood.empFunc += eat;
        getFood.priority = hunger;

        drinkADrink.empFunc = drink;
        drinkADrink.priority = thirst;

        Work.canInterupt = true;
        Leave.canInterupt = false;
        goToToilet.canInterupt = false;
        getFood.canInterupt = true;
        drinkADrink.canInterupt = true;
        

        //Add to list
        actions.Add(Work);
        actions.Add(Leave);
        actions.Add(goToToilet);
        actions.Add(getFood);
        actions.Add(drinkADrink);

        //Create modifiers so employees are unique
        needToWorkModifier = Random.Range(0.1f, 10.0f);
        bladderModifier = Random.Range(0.1f, 0.2f); 
        hungerModifier = Random.Range(0.1f, 0.2f); 
        thirstModifier = Random.Range(0.1f, 0.2f);

        //Create chance of boot licker that cannot lose happiness

        isBootLicker = Random.Range(1, 10) == 1 ? true : false;

        if(!isBootLicker)
        {
            Director.workerHappiness += setHappiness;
        }
        //else
        //{
        //    GetComponent<Renderer>().material.color = Color.red;
        //}

        //Give random monthly salary
        salary = 1200;
        moneyEarnedForCompany = salary * 10;

        //Delegate to pay employees
        Director.payTheGuys += payWages;
        currentFloor = Director.Instance.findClosestFloor(gameObject).floorNo;
        targetFloor = assignedWorkPoints[FacilityInfo.FacilityType.WorkSpace].GetFloor().floorNo;

        exitFloor = Director.Instance.findClosestFloor(Exit).floorNo;

        liftList = Director.Instance.liftList;
        getCurrentGrid = Director.Instance.getCurrentGrid;
        Lifts = Director.Instance.Lifts;
        facilities = Director.Instance.facilities;
        floors = Director.Instance.floors;

        quit = false;
        pathComplete = true;
        pathFinding = GetComponent<Pathfinding>();
    }

    public void fireEmployee()
    {
        quit = true;
        actions[0] = Leave;
        Leave.priority = 100;
        pathFinding.foundPath = false;
        pathComplete = true;
        foreach(KeyValuePair<FacilityInfo.FacilityType, Facility> pair in assignedWorkPoints)
        {
            pair.Value.employees.Remove(this);
        }
    }

    public void updateHappiness()
    {
        if(happiness > 100)
        {
            happiness = 100;
        }

        if(happiness <= 0)
        {
            fireEmployee();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Set floats to worker priorities to see them in inspector
        needToWork = Work.priority;
        homeTime = Leave.priority;
        needForBathroom = goToToilet.priority;
        hunger = getFood.priority;
        thirst = drinkADrink.priority;

        //If current action need is greater than the current action, current action will be executed

        StartCoroutine(updateActions());
        Move();
    }

    IEnumerator updateActions()
    {
        foreach (Actions action in actions)
        {
            if(action.priority > 100)
            {
                action.priority = 100;
            }

            if (action.priority > actions[0].priority && !quit)
            {
                if (actions[0].priority <= 0 || actions[0].canInterupt == true)
                {
                    yield return new WaitForSeconds(2.0f);
                    audioSource.Stop();
                    Swap(actions, 0, actions.IndexOf(action));

                    if (actions[0] != goToToilet)
                    {
                        rend.enabled = true;
                    }
                }
            }

            actions[0].execute();
            yield return null;
        }
    }

    public float getHappiness()
    {
        return happiness;
    }

    public void setHappiness(float value)
    {
        if (!isBootLicker) happiness += value;
    }

    public void setBank(float value)
    {
        moneyInBank += value;
    }

    //Employees are happy on payday
    public void payWages()
    {
        moneyInBank += salary;
        setHappiness(10.0f);
        updateHappiness();
    }

    public float getBank()
    {
        return moneyInBank;
    }

    public void setSalary(int value)
    {
        salary += value;
    }

    public float getSalary()
    {
        return salary;
    }

    public float getEarnings() {
        return moneyEarnedForCompany;
    }

    public void changeEmployeeLayer()
    {
        gameObject.layer = Lifts[currentFloor].layer;
        if (audioSource.isPlaying && currentFloor != CameraControl.instance.selectedFloor)
            audioSource.Stop();
    }
    //Using director positions, set employee target object
    public void moveTo(Director.Positions pos)
    {
        switch (pos)
            {
            case Director.Positions.desk:
                {
                    if (assignedWorkPoints[FacilityInfo.FacilityType.WorkSpace] == null) {
                        quit = true;
                        break;
                    }
                    targetFloor = assignedWorkPoints[FacilityInfo.FacilityType.WorkSpace].GetFloor().floorNo;
                    if (currentFloor == targetFloor)
                    {
                        targetObject = assignedWorkPoints[FacilityInfo.FacilityType.WorkSpace].workPoint;
                        break;
                    }
                    else
                    {
                        targetObject = Lifts[currentFloor];
                        break;
                    }
                 
                        
                }
            case Director.Positions.cafe:
                {
                    if (assignedWorkPoints[FacilityInfo.FacilityType.Catering] == null) {
                        moveTo(Director.Positions.exit);
                        break;
                    }
                    targetFloor = assignedWorkPoints[FacilityInfo.FacilityType.Catering].GetFloor().floorNo;
                    if (currentFloor == targetFloor)
                    {
                        targetObject = assignedWorkPoints[FacilityInfo.FacilityType.Catering].workPoint;
                        break;
                    }
                    else
                    {
                        targetObject = Lifts[currentFloor];
                        break;
                    }

                   
                }
            case Director.Positions.exit:
                {
                    targetFloor = exitFloor;
                    if (currentFloor == targetFloor)
                    {
                        targetObject = Exit;
                        break;
                    }
                    else
                    {
                        targetObject = Lifts[currentFloor];
                        break;
                    }
                }
            case Director.Positions.toilet:
                {
                    if (assignedWorkPoints[FacilityInfo.FacilityType.Toilets] == null) {
                        moveTo(Director.Positions.exit);
                        break;
                    }
                    targetFloor = assignedWorkPoints[FacilityInfo.FacilityType.Toilets].GetFloor().floorNo;
                    if (currentFloor == targetFloor)
                    {
                        targetObject = assignedWorkPoints[FacilityInfo.FacilityType.Toilets].workPoint;
                        break;
                    }
                    else
                    {
                        targetObject = Lifts[currentFloor];
                        break;
                    }
                }
            case Director.Positions.waterfountain:
                {
                    if (assignedWorkPoints[FacilityInfo.FacilityType.WaterFountain] == null) {
                        moveTo(Director.Positions.exit);
                        break;
                    }
                    targetFloor = assignedWorkPoints[FacilityInfo.FacilityType.WaterFountain].GetFloor().floorNo;
                    if (currentFloor == targetFloor)
                    {
                        targetObject = assignedWorkPoints[FacilityInfo.FacilityType.WaterFountain].workPoint;
                        break;
                    }
                    else
                    {
                        targetObject = Lifts[currentFloor];
                        break;
                    }
                }
            case Director.Positions.workstation:
                {
                    if (assignedWorkPoints[FacilityInfo.FacilityType.WorkSpace] == null) {
                        quit = true;
                        break;
                    }
                    targetFloor = assignedWorkPoints[FacilityInfo.FacilityType.WorkSpace].GetFloor().floorNo;
                    if (currentFloor == targetFloor)
                    {
                        targetObject = assignedWorkPoints[FacilityInfo.FacilityType.WorkSpace].workPoint;
                        break;
                    }
                    else
                    {
                        targetObject = Lifts[currentFloor];
                        break;
                    }
                }

            default: break;
            }
    }

    private bool inBounds(int index, List<Node> List)
    {
        return (index >= 0) && (index < List.Count);
    }

    void Move()
    {
        if (pathFinding.foundPath == true)
        {
            if (pathFinding.newPath != null && followPath != pathFinding.newPath[pathFinding.newPath.Count - 1].worldPos)
            {
                followPath = pathFinding.newPath[currentPathPoint].worldPos;
                Vector3 pathPosition = new Vector3(followPath.x, transform.position.y, followPath.z);
                Vector3 targetDirection = pathPosition - transform.position;
                targetDirection = targetDirection.normalized;

                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                        Quaternion.LookRotation(targetDirection),
                                                         maxTurnSpeed * Time.deltaTime);

                transform.position += transform.forward * Time.deltaTime * maxMoveSpeed;

                if (Vector3.Distance(pathPosition, transform.position) <= 0.5f)
                {
                    currentPathPoint = (currentPathPoint + 1) % pathFinding.newPath.Count;
                }
            }
            else
            {
                pathComplete = true;
            }
        }
    }

  
    
    public bool sitAtDesk()
    {
        if(assignedWorkPoints[FacilityInfo.FacilityType.WorkSpace] != targetObject)
        {
            moveTo(Director.Positions.desk);
        }
        if (Vector3.Distance(transform.position, targetObject.transform.position) > targetOffset)
        {
            return false;
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                if (currentFloor == CameraControl.instance.selectedFloor)
                    audioSource.Play();
                else
                    audioSource.Stop();
            }
            goToToilet.priority += (Time.deltaTime * bladderModifier);
            drinkADrink.priority += (Time.deltaTime * thirstModifier);
            getFood.priority += (Time.deltaTime * hungerModifier);
            Work.priority -= (Time.deltaTime * needToWorkModifier);
            return true;
        }
    }

    public bool goHome()
    {
        if (Exit != targetObject)
        {
            moveTo(Director.Positions.exit);
         
        }

        if (Vector3.Distance(transform.position, targetObject.transform.position) > targetOffset)
        {
            return false;
        }
        else
        {
            //gameObject.SetActive(false);
            Leave.priority = 0;
            return true;
        }
    }

    public bool goToBathroom()
    {
        if (assignedWorkPoints[FacilityInfo.FacilityType.Toilets] != targetObject)
        {
            moveTo(Director.Positions.toilet);
            
        }

        if (Vector3.Distance(transform.position, targetObject.transform.position) > targetOffset)
        {
            return false;
        }
        else
        {
            if(rend.enabled)
            {
                rend.enabled = false;
            }

            Work.priority += (Time.deltaTime * needToWorkModifier);
            goToToilet.priority -= (Time.deltaTime * bladderModifier);
            return true;
        }
    }

    public bool eat()
    {
        if (assignedWorkPoints[FacilityInfo.FacilityType.Catering] != targetObject)
        {
            moveTo(Director.Positions.cafe);
            
        }

        if (Vector3.Distance(transform.position, targetObject.transform.position) > targetOffset)
        {
            return false;
        }
        else
        {
            Work.priority += (Time.deltaTime * needToWorkModifier);
            goToToilet.priority += (Time.deltaTime * bladderModifier);
            getFood.priority -= (Time.deltaTime * hungerModifier);
            return true;
        }
    }

    public bool drink()
    {
        if (assignedWorkPoints[FacilityInfo.FacilityType.WaterFountain] != targetObject)
        {
            moveTo(Director.Positions.waterfountain);
        }

        if (Vector3.Distance(transform.position, targetObject.transform.position) > targetOffset)
        {
            return false;
        }
        else
        {
            Work.priority += (Time.deltaTime * needToWorkModifier);
            goToToilet.priority += (Time.deltaTime * bladderModifier);
            drinkADrink.priority -= (Time.deltaTime * thirstModifier);
            return true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == Lifts[currentFloor] && other.gameObject == targetObject)
        {
            transform.position = Lifts[targetFloor].transform.position + Lifts[targetFloor].transform.forward - Lifts[targetFloor].transform.up;
            currentFloor = targetFloor;
            gameObject.layer = Lifts[targetFloor].gameObject.layer;
        }

        if(other.gameObject == Exit && other.gameObject == targetObject)
        {
            if (quit)
            {
                if (Director.Instance.getCurrentEmployees() == Director.Instance.getMaxEmployees())
                {
                    Director.Instance.setMaxEmployees(-1);
                }

                Director.Instance.setCurrentEmployees(-1);

                Start();
                gameObject.SetActive(false);
            }
        }
    }

    public void AssignFacility(FacilityInfo.FacilityType facilityType)
    {
        if (facilityType == FacilityInfo.FacilityType.WorkSpace)
        {
            if (assignedWorkPoints.ContainsKey(facilityType))
            {
                assignedWorkPoints[facilityType] = OfficeManager.instance.GetEmptyFacility(facilityType);
            }
            else
            {
                assignedWorkPoints.Add(FacilityInfo.FacilityType.WorkSpace, OfficeManager.instance.GetEmptyFacility(FacilityInfo.FacilityType.WorkSpace));
            }
            //OfficeManager.instance.RemoveFacility(assignedWorkPoints[FacilityInfo.FacilityType.WorkSpace]);
        }
        else
        {
            if (assignedWorkPoints.ContainsKey(facilityType))
            {
                assignedWorkPoints[facilityType] = Director.Instance.assignFacility(assignedFloor, facilityType, assignedWorkPoints[FacilityInfo.FacilityType.WorkSpace].gameObject, this);
            }
            else
            {
                assignedWorkPoints.Add(facilityType, Director.Instance.assignFacility(assignedFloor, facilityType, assignedWorkPoints[FacilityInfo.FacilityType.WorkSpace].gameObject, this));
            }
        }
        if(assignedWorkPoints[facilityType] != null)
        {
            assignedWorkPoints[facilityType].employees.Add(this);
        }
    }
}




