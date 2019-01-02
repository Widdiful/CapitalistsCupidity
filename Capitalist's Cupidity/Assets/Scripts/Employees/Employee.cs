using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Employee : MonoBehaviour
{
    public Vector3 targetPos;
    public GameObject targetObject;
    public Vector3 velocity = Vector3.zero;
    float orientation;

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


    public GameObject Desk;
    public GameObject Toilet;
    public GameObject Cafe;
    public GameObject waterFountain;
    public GameObject Exit;

    int deskFloor;
    int toiletFloor;
    int cafeFloor;
    int waterFountainFloor;
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
        assignedFloor = Director.Instance.assignFloor();
        Desk = Director.Instance.assignFacilities(assignedFloor, "Work Space", Desk, this);
        Toilet = Director.Instance.assignFacilities(assignedFloor, "Toilets", Desk, this);
        Cafe = Director.Instance.assignFacilities(assignedFloor, "Cafeteria", Desk, this);
        waterFountain = Director.Instance.assignFacilities(assignedFloor, "Water Fountain", Desk, this);
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

        Work.canInterupt = false;
        Leave.canInterupt = false;
        goToToilet.canInterupt = true;
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

        isBootLicker = Random.Range(1, 3) > 1 ? true : false;

        if(!isBootLicker)
        {
            Director.workerHappiness += setHappiness;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
        }

        //Give random monthly salary
        salary = 1200;
        moneyEarnedForCompany = salary * 10;

        //Delegate to pay employees
        Director.payTheGuys += payWages;
        currentFloor = Director.Instance.findClosestFloor(gameObject).floorNo;
        targetFloor = Director.Instance.findClosestFloor(Desk).floorNo;

        deskFloor = Director.Instance.findClosestFloor(Desk).floorNo;
        toiletFloor = Director.Instance.findClosestFloor(Toilet).floorNo;
        cafeFloor = Director.Instance.findClosestFloor(Cafe).floorNo;
        waterFountainFloor = Director.Instance.findClosestFloor(waterFountain).floorNo;
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
            if (action.priority > actions[0].priority && actions[0].priority <= 0)
            {
                Swap(actions, 0, actions.IndexOf(action));
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
        happiness += value;
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
    }
    //Using director positions, set employee target pos
    public void moveTo(Director.Positions pos)
    {
        switch (pos)
            {
            case Director.Positions.desk:
                {
                    targetFloor = deskFloor;
                    if (currentFloor == targetFloor)
                    {
                        targetObject = Desk;
                        targetPos = Desk.transform.position;
                    }
                    else
                    {
                        targetObject = Lifts[currentFloor];
                        targetPos = Lifts[currentFloor].transform.position;
                    }
                 
                        break;
                    }
            case Director.Positions.cafe:
                {
                    targetFloor = cafeFloor;
                    if (currentFloor == targetFloor)
                    {
                        targetPos = Cafe.transform.position;
                        targetObject = Cafe;
                    }
                    else
                    {
                        targetPos = Lifts[currentFloor].transform.position;
                        targetObject = Lifts[currentFloor];
                    }

                    break;
                }
            case Director.Positions.exit:
                {
                    targetFloor = exitFloor;
                    if (currentFloor == targetFloor)
                    {
                        targetPos = Exit.transform.position;
                        targetObject = Exit;
                    }
                    else
                    {
                        targetPos = Lifts[currentFloor].transform.position;
                        targetObject = Lifts[currentFloor];
                    }

                    break;
                }
            case Director.Positions.toilet:
                {
                    targetFloor = toiletFloor;
                    if (currentFloor == targetFloor)
                    {
                        targetPos = Toilet.transform.position;
                        targetObject = Toilet;
                    }
                    else
                    {
                        targetPos = Lifts[currentFloor].transform.position;
                        targetObject = Lifts[currentFloor];
                    }

                    break;
                }
            case Director.Positions.waterfountain:
                {
                    targetFloor = waterFountainFloor;
                    if (currentFloor == targetFloor)
                    {
                        targetPos = waterFountain.transform.position;
                        targetObject = waterFountain;
                    }
                    else
                    {
                        targetPos = Lifts[currentFloor].transform.position;
                        targetObject = Lifts[currentFloor];
                    }

                    break;
                }
            case Director.Positions.workstation:
                {
                    targetFloor = deskFloor;
                    if (currentFloor == targetFloor)
                    {
                        targetPos = Desk.transform.position;
                        targetObject = Desk;
                    }
                    else
                    {
                        targetPos = Lifts[currentFloor].transform.position;
                        targetObject = Lifts[currentFloor];
                    }

                    break;
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
        if(Desk.transform.position != targetPos)
        {
            moveTo(Director.Positions.desk);
        }
        if (Vector3.Distance(transform.position, targetPos) > targetOffset)
        {
            return false;
        }
        else
        {
            goToToilet.priority += (Time.deltaTime * bladderModifier);
            drinkADrink.priority += (Time.deltaTime * thirstModifier);
            getFood.priority += (Time.deltaTime * hungerModifier);
            Work.priority -= (Time.deltaTime * needToWorkModifier);
            return true;
        }
    }

    public bool goHome()
    {
        if (Exit.transform.position != targetPos)
        {
            moveTo(Director.Positions.exit);
         
        }

        if (Vector3.Distance(transform.position, targetPos) > targetOffset)
        {
            return false;
        }
        else
        {
            //gameObject.SetActive(false);
            Leave.priority = 0;
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
            return true;
        }
    }

    public bool goToBathroom()
    {
        if (Toilet.transform.position != targetPos)
        {
            moveTo(Director.Positions.toilet);
            
        }

        if (Vector3.Distance(transform.position, targetPos) > targetOffset)
        {
            return false;
        }
        else
        {
            Work.priority += (Time.deltaTime * needToWorkModifier);
            goToToilet.priority -= (Time.deltaTime * bladderModifier);
            return true;
        }
    }

    public bool eat()
    {
        if (Cafe.transform.position != targetPos)
        {
            moveTo(Director.Positions.cafe);
            
        }

        if (Vector3.Distance(transform.position, targetPos) > targetOffset)
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
        if (waterFountain.transform.position != targetPos)
        {
            moveTo(Director.Positions.waterfountain);
        }

        if (Vector3.Distance(transform.position, targetPos) > targetOffset)
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Lifts[currentFloor] && other.gameObject == targetObject)
        {
            transform.position = Lifts[targetFloor].transform.position + Lifts[targetFloor].transform.forward - Lifts[targetFloor].transform.up;
            currentFloor = targetFloor;
            gameObject.layer = Lifts[targetFloor].gameObject.layer;
        }
    }
}




