using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    public int floorNo;
    public int workspaceCount;
    public enum FloorTypes { Warehouse, Office };
    public FloorTypes floorType;

    public void InitialiseFloor(int number, FloorTypes type, int workspaces) {
        floorNo = number;
        transform.position = new Vector3(0, floorNo, 0);
        floorType = type;
        workspaceCount = workspaces;
    }
}
