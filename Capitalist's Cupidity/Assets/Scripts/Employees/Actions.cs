using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions
{
    float expiryTime = 0;
    public float priority = 0;
    bool canInterupt = false;
    bool isComplete = false;

    public delegate bool employeeFunc(Employee emp);
    public employeeFunc empFunc;

    public static bool sitAtDesk(Employee emp)
    {
        emp.moveTo(Director.Positions.desk);

        if (emp.transform.position != emp.targetPos)
        {
            return false;
        }

        return true;
    }

    public static bool goHome(Employee emp)
    {
        emp.moveTo(Director.Positions.exit);

        if (emp.transform.position != emp.targetPos)
        {
            return false;
        }

        return true;
    }

    public static bool goToBathroom(Employee emp)
    {
        emp.moveTo(Director.Positions.toilet);

        if (emp.transform.position != emp.targetPos)
        {
            return false;
        }

        return true;
    }

    public static bool eat(Employee emp)
    {
        emp.moveTo(Director.Positions.cafe);

        if (emp.transform.position != emp.targetPos)
        {
            return false;
        }

        return true;
    }

    public static bool drink(Employee emp)
    {
        emp.moveTo(Director.Positions.waterfountain);

        if (emp.transform.position != emp.targetPos)
        {
            return false;
        }

        return true;
    }

    public void execute(Employee emp)
    {
        isComplete = empFunc.Invoke(emp);
    }

}
