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



    float happiness = 100;


    private void Start()
    {
        Director.updatePos += moveTo;
    }

    // Update is called once per frame
    void Update()
    {
        Steer(targetPos);

       if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }
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

    void moveTo(Director.Positions pos)
    {
        Debug.Log("Changing pos");

        switch(pos)
        {
            case Director.Positions.workstation: break;
            case Director.Positions.waterfountain:
                //targetPos = new Vector3(100, transform.position.y,
                   //100);
                break;

            case Director.Positions.exit:
                targetPos = new Vector3(Random.Range(0, 100), transform.position.y,
                    Random.Range(0, 100));

                break;
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
        transform.position +=  Vector3.ClampMagnitude(velocity, 800) * Time.deltaTime;
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
            avoidanceForce = avoidanceForce.normalized * maxAvoidanceForce;

            return avoidanceForce;
        }
        else if (obj.bounds.Contains(seeAheadNear))
        {
            avoidanceForce = seeAheadNear - obj.transform.position;
            avoidanceForce = avoidanceForce.normalized * maxAvoidanceForce;

            
            return avoidanceForce;
        }

        return Vector3.zero;
    }
}




