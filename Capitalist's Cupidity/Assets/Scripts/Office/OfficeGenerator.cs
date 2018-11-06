using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeGenerator : MonoBehaviour {

    public GameObject floorPrefab;
    public string officeName;
    public int floorCount;
    public int warehouseCount;
    public Vector2 floorSize;
    public float floorHeight;
    public int workspaceCount;
    public int workspacePadding;
    private List<Floor> floors = new List<Floor>();

    void Start() {
        CreateFloors();
    }

    public void CreateFloor() {

    }

    public void CreateFloors() {
        for(int i = 0; i < floorCount; i++) {
            Floor newFloor = Instantiate(floorPrefab, transform.position, Quaternion.identity).GetComponent<Floor>();
            floors.Add(newFloor);

            Floor.FloorTypes newType = Floor.FloorTypes.Office;
            if (i < warehouseCount) {
                newType = Floor.FloorTypes.Warehouse;
            }

            newFloor.InitialiseFloor(floorSize.x, floorHeight, floorSize.y, i, newType, workspaceCount, workspacePadding);
            newFloor.transform.position = new Vector3(0, newFloor.transform.position.y, 0);

            if (i < floorCount - 1)
            {
                newFloor.AddStairs();
            }
        }
    }
}
