using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{
    
    public Vector3 targetPos;
    public Vector3 velocity = Vector3.zero;
    float orientation;

    float maxTurnSpeed = 5.0f;
    float maxMoveSpeed = 5.0f;

    Vector3 seeAhead = Vector3.zero;
    Vector3 seeAheadNear = Vector3.zero;
    float maxSeeAheadDistance = 3.0f;

    Vector3 avoidanceForce;
    float maxAvoidanceForce = 3f;

    //Actions.employeeFunc(this)

    float happiness = 100;

    Actions Work;
    Actions Leave;
    Actions goToToilet;
    Actions getFood;
    Actions drinkADrink;

    public float needToWork = 10f;
    public float homeTime = 0.0f;
    public float needForBathroom = 0.0f;
    public float hunger = 0.0f;
    public float thirst = 0.0f;

    public GameObject Desk;
    public GameObject Toilet;
    public GameObject Cafe;
    public GameObject waterFountain;
    public GameObject Exit;


    public List<Actions> actions;
    

    private void Start()
    {

        Director.updatePos += moveTo;

        actions = new List<Actions>();

        Work = new Actions();
        Leave = new Actions();
        goToToilet = new Actions();
        getFood = new Actions();
        drinkADrink = new Actions();

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

        actions.Add(Work);
        actions.Add(Leave);
        actions.Add(goToToilet);
        actions.Add(getFood);
        actions.Add(drinkADrink);

        /*Desk = GameObject.Find("Desk");
        Toilet = GameObject.Find("Toilet");
        Cafe = GameObject.Find("Cafe");
        waterFountain = GameObject.Find("waterFountain");
        Exit = GameObject.Find("Exit");*/
    }

    // Update is called once per frame
    void Update()
    {
        Steer(targetPos);

        /*if (Input.GetMouseButtonDown(0))
         {
             RaycastHit hit;
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             if (Physics.Raycast(ray, out hit))
             {
                 targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
             }
         }*/

        Work.priority += (Time.deltaTime / 10);
        Leave.priority += (Time.deltaTime / 1000);
        goToToilet.priority += (Time.deltaTime / 10);
        getFood.priority += (Time.deltaTime / 10);
        drinkADrink.priority += (Time.deltaTime / 10);

        needToWork = Work.priority;
        homeTime = Leave.priority;
        needForBathroom = goToToilet.priority;
        hunger = getFood.priority;
        thirst = drinkADrink.priority;

        foreach (Actions action in actions)
        {
            if(action.priority > actions[0].priority)
            {
                actions[0] = action;
            }

            actions[0].execute();
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

    public void moveTo(Director.Positions pos)
    {
        Debug.Log("Changing pos");

        switch (pos)
        {

            case Director.Positions.desk:
                {
                    targetPos = new Vector3(Desk.transform.position.x, transform.position.y, Desk.transform.position.z);
                    break;
                }
            case Director.Positions.waterfountain:
                {
                    targetPos = new Vector3(waterFountain.transform.position.x, transform.position.y, waterFountain.transform.position.z);
                    //targetPos = new Vector3(100, transform.position.y,
                    //100);
                    break;
                }
            case Director.Positions.cafe:
                {
                    targetPos = new Vector3(Cafe.transform.position.x, transform.position.y, Cafe.transform.position.z);
                    break;
                }
            case Director.Positions.exit:
                {
                    targetPos = new Vector3(Exit.transform.position.x, transform.position.y, Exit.transform.position.z);
                    //targetPos = new Vector3(Random.Range(0, 100), transform.position.y,
                    //Random.Range(0, 100));
                    break;
                }
            case Director.Positions.toilet:
                {
                    targetPos = new Vector3(Toilet.transform.position.x, transform.position.y, Toilet.transform.position.z);
                    break;
                }
            default: break;
        }
    }

    void Steer(Vector3 targetPos)
    {
        targetPos = targetPos - transform.position;
        if (avoidCollision() == Vector3.zero)
        {
            velocity = (targetPos.normalized * maxMoveSpeed) + (Director.Instance.flockingCohesion(this) + Director.Instance.flockingAlignment(this) + Director.Instance.flockingSeperation(this));
        }
        else
        {
            velocity = (targetPos.normalized * maxMoveSpeed) + avoidCollision();
        }
        transform.position +=  Vector3.ClampMagnitude(new Vector3(velocity.x, 0, velocity.z), 800) * Time.deltaTime;
        rotate(targetPos);
    }

    void rotate(Vector3 targetPos)
    {
        if(velocity != Vector3.zero)
        {
            Vector3 delta = targetPos - transform.position;
            delta.y = transform.position.y;
            Quaternion rotation = Quaternion.LookRotation(delta);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * maxTurnSpeed);
        }
    }

    Vector3 avoidCollision()
    {
        RaycastHit hit;
        RaycastHit hit2;
        RaycastHit hit3;

        //Debug.DrawLine(transform.position, transform.position + (transform.forward * maxSeeAheadDistance), Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxSeeAheadDistance))
        {
            Collider obj = hit.collider;

            seeAhead = transform.position + (velocity.normalized * maxSeeAheadDistance);
            seeAheadNear = transform.position + (velocity.normalized * (maxSeeAheadDistance * 0.5f));

            return checkCollisionBounds(obj, seeAhead, seeAheadNear);
        }
        if(Physics.Raycast(transform.position, (transform.forward + transform.right).normalized, out hit2, maxSeeAheadDistance))
        {
            Collider obj = hit2.collider;

            seeAhead = transform.position + ((velocity.normalized) * maxSeeAheadDistance);
            seeAheadNear = transform.position + (velocity.normalized) * (maxSeeAheadDistance * 0.5f);

            return checkCollisionBounds(obj, seeAhead, seeAheadNear);
        }
        if(Physics.Raycast(transform.position, (transform.forward - transform.right).normalized, out hit3, maxSeeAheadDistance))
        {
            Collider obj = hit3.collider;

            seeAhead = transform.position + ((velocity.normalized) * maxSeeAheadDistance);
            seeAheadNear = transform.position + (velocity.normalized) * (maxSeeAheadDistance * 0.5f);

            return checkCollisionBounds(obj, seeAhead, seeAheadNear);
        }

        return Vector3.zero;
    }
    
    Vector3 checkCollisionBounds(Collider obj, Vector3 seeAhead, Vector3 seeAheadNear)
    {
        

        if (obj.bounds.Contains(seeAhead))
        {
            avoidanceForce = seeAhead - obj.transform.position;
            avoidanceForce = avoidanceForce.normalized * (maxAvoidanceForce * 2);

            return avoidanceForce;
        }
        else if (obj.bounds.Contains(seeAheadNear))
        {
            avoidanceForce = seeAheadNear - obj.transform.position;
            avoidanceForce = avoidanceForce.normalized * (maxAvoidanceForce * 4);

            return avoidanceForce;
        }

        return Vector3.zero;
    }


    public bool sitAtDesk()
    {
        moveTo(Director.Positions.desk);

        if (Vector3.Distance(transform.position, targetPos) > 5)
        {
            return false;
        }
        else
        {
            Work.priority -= (Time.deltaTime / 5);
            return true;
        }
    }

    public bool goHome()
    {
        moveTo(Director.Positions.exit);

        if (Vector3.Distance(transform.position, targetPos) > 5)
        {
            return false;
        }
        else
        {
            gameObject.SetActive(false);
            Leave.priority = 0;
            return true;
        }
    }

    public bool goToBathroom()
    {
        moveTo(Director.Positions.toilet);

        if (Vector3.Distance(transform.position, targetPos) > 5)
        {
            return false;
        }
        else
        {
            goToToilet.priority -= (Time.deltaTime / 5);
            return true;
        }
    }

    public bool eat()
    {
        moveTo(Director.Positions.cafe);

        if (Vector3.Distance(transform.position, targetPos) > 5)
        {
            return false;
        }
        else
        {
            getFood.priority -= (Time.deltaTime / 5);
            return true;
        }
    }

    public bool drink()
    {
        moveTo(Director.Positions.waterfountain);

        if (Vector3.Distance(transform.position, targetPos) > 5)
        {
            return false;
        }
        else
        {
            drinkADrink.priority -= (Time.deltaTime / 5);
            return true;
        }
    }
}




