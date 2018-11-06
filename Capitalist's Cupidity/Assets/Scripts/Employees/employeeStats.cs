using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class employeeStats
{
    [SerializeField]
    private float baseValue;

    public float GetValue()
    {
        return baseValue;
    }
}
