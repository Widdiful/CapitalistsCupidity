﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeGenerator : MonoBehaviour {

    [System.Serializable]
    public class FacilitySpawnInformation
    {
        public string name;
        public int count;
    }

    public GameObject floorPrefab;
    public GameObject shadowPrefab;
    public string officeName;
    public int floorCount;
    public int maxFloors;
    public int warehouseCount;
    public Vector2 floorSize;
    public float floorHeight;
    public int workspaceCount;
    public int workspacePadding;
    public List<FacilitySpawnInformation> facilitySpawnInformation = new List<FacilitySpawnInformation>();
    public List<GameObject> lifts = new List<GameObject>();

    private List<Floor> floors = new List<Floor>();
    private List<Facility> facilities = new List<Facility>();
    private Transform officeParent;
    private Dictionary<string, List<int>> facilityFloors = new Dictionary<string, List<int>>();
    private List<int> facilitiesPerFloor = new List<int>();

    private Transform shadow;

    public static OfficeGenerator instance;

    void Awake() {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }

    void Start() {
        for (int i = 0; i < floorCount; i++)
            facilitiesPerFloor.Add(0);

        CreateShadow();
        CreateFloors();
    }

    public void BuyFloor() {
        if (maxFloors < floorCount) {
            floors[maxFloors].purchased = true;
            maxFloors++;
            CameraControl.instance.ChangeFloor(maxFloors);
        }
    }

    public List<Facility> getFacilities()
    {
        return facilities;
    }
    private void CreateShadow()
    {
        if (shadowPrefab)
        {
            float shadowHeight = floorCount * floorHeight;
            shadow = Instantiate(shadowPrefab, new Vector3(0, shadowHeight * 0.5f, 0), Quaternion.identity, officeParent).transform;
            shadow.localScale = new Vector3(floorSize.x + 0.2f, shadowHeight, floorSize.y + 0.2f);
        }
    }

    private Floor CreateFloor(int floorNo) {
        Floor newFloor = Instantiate(floorPrefab, transform.position, Quaternion.identity, officeParent).GetComponent<Floor>();
        floors.Add(newFloor);

        Floor.FloorTypes newType = Floor.FloorTypes.Office;
        if (floorNo < warehouseCount) {
            newType = Floor.FloorTypes.Warehouse;
        }

        newFloor.InitialiseFloor(floorSize.x, floorHeight, floorSize.y, floorNo, newType, workspaceCount, workspacePadding);
        newFloor.transform.position = new Vector3(0, newFloor.transform.position.y, 0);

        if (floorNo < floorCount - 1) {
            newFloor.AddStairs();
        }

        if (floorNo == 0) {
            AddEntrance();
        }

        return newFloor;
    }

    private void CreateFloors() {
        officeParent = new GameObject(officeName).transform;
        List<int> floorsWithFacility = new List<int>();

        // Decide which floors each facility will spawn on
        foreach(FacilitySpawnInformation info in facilitySpawnInformation)
        {
            List<int> floors = new List<int>();
            int j = 0;
            for (int i = 0; i < info.count; i++)
            {
                int floorNumber = Random.Range(0, maxFloors);
                int attempts = 0;
                while (((floors.Contains(floorNumber) && (floorsWithFacility.Contains(floorNumber) && floorsWithFacility.Count <= maxFloors) || facilitiesPerFloor[floorNumber] >= 1)  && attempts < 25))
                {
                    floorNumber = Random.Range(0, maxFloors);
                    attempts++;
                }
                if (attempts <= 25)
                {
                    floors.Add(floorNumber);
                    facilitiesPerFloor[floorNumber]++;
                    floorsWithFacility.Add(floorNumber);
                }
                j = i;
            }
            facilityFloors.Add(info.name, floors);
        }

        // Create floors
        for(int i = 0; i < floorCount; i++) {
            Floor newFloor = CreateFloor(i);
            if (i < maxFloors) newFloor.purchased = true;
            facilities.AddRange(newFloor.facilities);

            List<Facility> tempFacilities = newFloor.facilities;
            foreach(string facilityType in facilityFloors.Keys)
            {
                if (facilityFloors[facilityType].Contains(i))
                {
                    Facility temp = tempFacilities[Random.Range(0, tempFacilities.Count - 1)];
                    if (temp.CheckFacilitySize(FacilityList.instance.GetFacilityByName(facilityType)))
                    {                        
                        
                        temp.ChangeFacility(FacilityList.instance.GetFacilityByName(facilityType));
                    }
                    //temp.facilityInfo = FacilityList.instance.GetFacilityByName(facilityType);
                    //temp.name = facilityType;
                    tempFacilities.Remove(temp);
                }
            }

            lifts.Add(newFloor.lift);
        }

        // Set all empty facilities to workspaces
        foreach(Facility facility in facilities)
        {
            if (facility.facilityInfo.facilityType == FacilityInfo.FacilityType.Empty && facility.GetFloor().floorNo < maxFloors)
            {
                facility.ChangeFacility(FacilityList.instance.GetFacilityByName("Work Space"));
                //facility.facilityInfo = FacilityList.instance.GetFacilityByName("Work Space");
                //facility.name = "Work Space";
            }
        }
    }

    public List<Floor> GetFloors() {
        return floors;
    }

    // Adds an entrance
    private void AddEntrance() {
        GameObject newEntrance = Instantiate(Resources.Load("OfficeParts/Entrance"), officeParent) as GameObject;
        newEntrance.transform.position = new Vector3(0, 0.5f, -(floorSize.y / 2f) - 0.1f);
        newEntrance.GetComponentInChildren<TextMesh>().text = officeName;
    }

    public int GetNumberOfWorkspaces() {
        return OfficeManager.instance.FacilityLists[FacilityInfo.FacilityType.WorkSpace].Count;
    }
}
