using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeGenerator : MonoBehaviour {

    public GameObject floorPrefab;
    public int floorCount;
    public int warehouseCount;
    public Vector2 floorSize;
    public float floorHeight;
    public int workspaceCount;
    private List<Floor> floors = new List<Floor>();

    void Start() {
        CreateFloors();
    }

    public void CreateFloor() {

    }

    public void CreateFloors() {
        for(int i = 0; i < floorCount; i++) {
            Floor newFloor = Instantiate(floorPrefab, Vector3.zero, Quaternion.identity).GetComponent<Floor>();
            floors.Add(newFloor);

            Floor.FloorTypes newType = Floor.FloorTypes.Office;
            if (i < warehouseCount) {
                newType = Floor.FloorTypes.Warehouse;
            }

            newFloor.InitialiseFloor(i, newType, 0);
        }
    }
}
