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
    float maxSeeAheadDistance = 10.0f;

    Vector3 avoidanceForce;
    float maxAvoidanceForce = 5.0f;



    float happiness = 100;


    private void Start()
    {
        Director.updatePos += moveTo;
    }

    // Update is called once per frame
    void Update()
    {
        Steer(targetPos);

       /* if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                targetPos = new Vector3(hit.point.x, 2.0f, hit.point.z);
            }
        }*/
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

        transform.position += velocity * Time.deltaTime + avoidCollision();

        rotate(targetPos);

    }

    void rotate(Vector3 targetPos)
    {
        transform.rotation = Quaternion.LookRotation(targetPos);
    }

    Vector3 avoidCollision()
    {
        RaycastHit hit;

        Debug.DrawLine(transform.position, transform.position + (transform.forward * maxSeeAheadDistance), Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxSeeAheadDistance))
        {

            seeAhead = transform.position + (velocity.normalized * maxSeeAheadDistance);
            seeAheadNear = transform.position + (velocity.normalized * (maxSeeAheadDistance * 0.5f));
            Debug.Log(hit.collider.name);
            if(Vector3.Distance(hit.collider.bounds.center, seeAhead) <= hit.collider.bounds.extents.x || Vector3.Distance(hit.collider.bounds.center, seeAhead) <= hit.collider.bounds.extents.y
                || Vector3.Distance(hit.collider.bounds.center, seeAhead) <= hit.collider.bounds.extents.z)
            {
                avoidanceForce = seeAhead - hit.collider.bounds.center;
                avoidanceForce = avoidanceForce.normalized * maxAvoidanceForce;

                return avoidanceForce;
            }
        }

        return Vector3.zero;

    }
}


