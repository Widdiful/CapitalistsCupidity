using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{


    public Vector3 targetPos;


    Vector3 velocity = Vector3.zero;
    float orientation;
    float maxTurnSpeed = 5.0f;
    float maxMoveSpeed = 5.0f;

    Vector3 seeAhead = Vector3.zero;
    Vector3 seeAheadNear = Vector3.zero;
    float maxSeeAheadDistance = 1.0f;

    Vector3 avoidanceForce;
    float maxAvoidanceForce = 8.0f;



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
            case Director.Positions.waterfountain: break;
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

        velocity = targetPos.normalized * maxMoveSpeed;

        transform.position += (velocity + avoidCollision()) * Time.deltaTime;

        rotate(targetPos);

    }

    void rotate(Vector3 targetPos)
    {
       
            transform.rotation = Quaternion.LookRotation(targetPos);
        
    }

    Vector3 avoidCollision()
    {
        RaycastHit hit;

        //Debug.DrawLine(transform.position, transform.position + (transform.forward * maxSeeAheadDistance), Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxSeeAheadDistance))
        {

            seeAhead = transform.position + (velocity.normalized * maxSeeAheadDistance);
            seeAheadNear = transform.position + (velocity.normalized * (maxSeeAheadDistance * 0.5f));
            Debug.Log(hit.collider.name);
            if(hit.collider.GetComponent<Collider>().bounds.Contains(seeAhead))
            {
                avoidanceForce = seeAhead - hit.collider.transform.position;
                avoidanceForce = avoidanceForce.normalized * maxAvoidanceForce;

                Debug.Log(avoidanceForce);
                return avoidanceForce;
            }
            else if(hit.collider.GetComponent<Collider>().bounds.Contains(seeAheadNear))
            {
                avoidanceForce = seeAheadNear - hit.collider.transform.position;
                avoidanceForce = avoidanceForce.normalized * maxAvoidanceForce;

                Debug.Log(avoidanceForce);
                return avoidanceForce;
            }
        }

        return Vector3.zero;

    }
}


