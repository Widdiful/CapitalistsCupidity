using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeGenerator : MonoBehaviour {

    public GameObject floorPrefab;
    public GameObject shadowPrefab;
    public string officeName;
    public int floorCount;
    public int warehouseCount;
    public Vector2 floorSize;
    public float floorHeight;
    public int workspaceCount;
    public int workspacePadding;

    private List<Floor> floors = new List<Floor>();
    private Transform officeParent;

    void Start() {
        CreateShadow();
        CreateFloors();
    }

    private void CreateShadow()
    {
        if (shadowPrefab)
        {
            float shadowHeight = floorCount * floorHeight;
            Transform shadow = Instantiate(shadowPrefab, new Vector3(0, shadowHeight * 0.5f, 0), Quaternion.identity, officeParent).transform;
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
        for(int i = 0; i < floorCount; i++) {
            CreateFloor(i);
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
}
