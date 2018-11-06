using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{

    public enum Positions { workstation, waterfountain, exit };
    public Positions _pos;
    Vector3 targetPos;


    Vector3 velocity = Vector3.zero;
    float orientation;
    float maxTurnSpeed;
    float maxMoveSpeed;

    Vector3 seeAhead = Vector3.zero;
    Vector3 seeAheedNear = Vector3.zero;
    float maxSeeAheedDistance = 10.0f;

    float happiness = 100;


    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float getHappiness()
    {
        return happiness;
    }

    public void setHappiness(float value)
    {
        happiness += value;
    }

    void moveTo(Positions pos)
    {
        _pos = pos;

        switch(_pos)
        {
            case Positions.workstation: break;
            case Positions.waterfountain: break;
            case Positions.exit: break;
        }
    }

    void Steer(Vector3 targetPos)
    {
        targetPos = targetPos - transform.position;

        velocity = targetPos.normalized * maxMoveSpeed;

        transform.position += velocity * Time.deltaTime;

        rotate(targetPos);

    }

    void rotate(Vector3 targetPos)
    {
        transform.rotation = Quaternion.LookRotation(targetPos);
    }

    void avoidCollision()
    {
        seeAhead = transform.position + (velocity.normalized * maxSeeAheedDistance);
        seeAheedNear = transform.position + (velocity.normalized * (maxSeeAheedDistance * 0.5f));

    }


}


