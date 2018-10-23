using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    public int floorNo;
    public int workspaceCount;
    public enum FloorTypes { Warehouse, Office };
    public FloorTypes floorType;
    public GameObject floorArea;
    public GameObject northWall;
    public GameObject southWall;
    public GameObject eastWall;
    public GameObject westWall;


    public void InitialiseFloor(float width, float height, int number, FloorTypes type, int workspaces) {
        floorNo = number;
        transform.position = new Vector3(width / 2f, floorNo, height / 2f);
        floorType = type;
        workspaceCount = workspaces;

        floorArea.transform.localScale = new Vector3(width, floorArea.transform.localScale.y, height);
        northWall.transform.localScale = new Vector3(width, northWall.transform.localScale.y, northWall.transform.localScale.z);
        northWall.transform.localPosition = new Vector3(0, 0, (height / 2f) + 0.05f);

        southWall.transform.localScale = new Vector3(width, southWall.transform.localScale.y, southWall.transform.localScale.z);
        southWall.transform.localPosition = new Vector3(0, 0, -((height / 2f) + 0.05f));

        eastWall.transform.localScale = new Vector3(eastWall.transform.localScale.x, eastWall.transform.localScale.y, height + 0.2f);
        eastWall.transform.localPosition = new Vector3(-((width / 2f) + 0.05f), 0, 0);

        westWall.transform.localScale = new Vector3(westWall.transform.localScale.x, westWall.transform.localScale.y, height + 0.2f);
        westWall.transform.localPosition = new Vector3((width / 2f) + 0.05f, 0, 0);
    }
}
