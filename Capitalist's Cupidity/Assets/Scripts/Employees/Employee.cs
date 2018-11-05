using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{

    public enum Positions { workstation, waterfountain, exit };
    public Positions _pos;
    Vector3 targetPos;

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

    void Steering()
    {

    }

    void avoidCollision()
    {

    }
}


