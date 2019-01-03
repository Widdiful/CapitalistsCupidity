using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeManager : MonoBehaviour
{
    public List<Facility> listOfWorkspaces = new List<Facility>();
    public List<Facility> listOfCafeterias = new List<Facility>();
    public List<Facility> listOfBreakRooms = new List<Facility>();
    public List<Facility> listOfToilets = new List<Facility>();
    public Dictionary<FacilityInfo.FacilityType, List<Facility>> FacilityLists = new Dictionary<FacilityInfo.FacilityType, List<Facility>>();

    public static OfficeManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            FacilityLists.Add(FacilityInfo.FacilityType.WorkSpace, listOfWorkspaces);
            FacilityLists.Add(FacilityInfo.FacilityType.Catering, listOfCafeterias);
            FacilityLists.Add(FacilityInfo.FacilityType.WaterFountain, listOfBreakRooms);
            FacilityLists.Add(FacilityInfo.FacilityType.Toilets, listOfToilets);
        }

        if (instance != this)
            Destroy(this);
    }

    public Facility GetEmptyFacility(FacilityInfo.FacilityType facilityType)
    {

        foreach(Facility facility in FacilityLists[facilityType])
        {
            if(facility.employees.Count == 0)
            {
                return facility;
            }
        }

        return null;
    }


    public void AddFacility(Facility facility)
    {
        FacilityLists[facility.facilityInfo.facilityType].Add(facility);
    }

    public void RemoveFacility(Facility facility)
    {
        FacilityLists[facility.facilityInfo.facilityType].Remove(facility);
    }

}
