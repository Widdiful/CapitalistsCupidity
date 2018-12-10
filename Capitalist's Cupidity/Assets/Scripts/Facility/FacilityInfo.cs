using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FacilityInfo
{
    //Public
    public enum FacilityType { Catering, WaterFountain, Toilets,  Security, WorkSpace, Gym, Relax, Empty }
    public float costToBuy;
    public float baseMonthlyExpenses;
    public string facilityName;
    public GameObject child;
    public FacilityType facilityType = FacilityType.Empty;
}
