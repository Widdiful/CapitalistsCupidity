using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FacilityInfo
{
    //Public
    public enum FacilityType { Catering, CarPark, Toilets,  Security, WorkSpace, Gym, Relax, Empty }
    public float costToBuy;
    public float baseMonthlyExpenses;
    public string facilityName;
    public FacilityType facilityType;
}
