using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions
{
    public float priority = 0;
    public bool canInterupt = false;
    public bool isComplete = false;

    public delegate bool employeeFunc();
    public employeeFunc empFunc;

    public void execute()
    {
        isComplete = empFunc.Invoke();
    }

}
