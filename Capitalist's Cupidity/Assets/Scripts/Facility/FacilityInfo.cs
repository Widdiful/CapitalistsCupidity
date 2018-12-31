using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FacilityInfo
{
    //Public
    public enum FacilityType { Catering, WaterFountain, Toilets,  Security, WorkSpace, Gym, Relax, Empty, Copy }
    public float costToBuy;
    public float baseMonthlyExpenses;
    public string facilityName;
    public GameObject child;
    public FacilityType facilityType = FacilityType.Empty;
    public int height = 1;
    public int width = 1;
    public float sabotageCost;
    public string sabotageMessageTitle;
    public string sabotageMessageContent;
}
