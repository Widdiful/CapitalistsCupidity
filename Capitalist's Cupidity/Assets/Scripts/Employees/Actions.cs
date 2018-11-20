using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions
{
    float expiryTime = 0;
    public float priority = 0;
    bool canInterupt = false;
    bool isComplete = false;

    public delegate bool employeeFunc();
    public employeeFunc empFunc;

    public void execute()
    {
        isComplete = empFunc.Invoke();
    }

}
