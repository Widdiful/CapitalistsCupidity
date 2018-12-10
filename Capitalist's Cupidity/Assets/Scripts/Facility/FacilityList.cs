using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityList : MonoBehaviour {

    public List<FacilityInfo> facilityList = new List<FacilityInfo>();

    public static FacilityList instance;

    void Awake() {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }

    public FacilityInfo GetFacilityByName(string name)
    {
        foreach(FacilityInfo facility in facilityList)
        {
            if(facility.facilityName == name)
            {
                return facility;
            }
        }
        return null;
    }
}
