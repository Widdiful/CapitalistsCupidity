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


    public void InitialiseFloor(float width, float height, float depth, int number, FloorTypes type, int workspaces) {
        floorNo = number;
        transform.position = new Vector3(width / 2f, floorNo * height, depth / 2f);
        floorType = type;
        workspaceCount = workspaces;

        floorArea.transform.localScale = new Vector3(width, floorArea.transform.localScale.y, depth);
        floorArea.transform.localPosition = Vector3.zero;
        northWall.transform.localScale = new Vector3(width, height, northWall.transform.localScale.z);
        northWall.transform.localPosition = new Vector3(0, height / 2f, (depth / 2f) + 0.05f);

        southWall.transform.localScale = new Vector3(width, height, southWall.transform.localScale.z);
        southWall.transform.localPosition = new Vector3(0, height / 2f, -((depth / 2f) + 0.05f));

        eastWall.transform.localScale = new Vector3(eastWall.transform.localScale.x, height, depth + 0.2f);
        eastWall.transform.localPosition = new Vector3(-((width / 2f) + 0.05f), height / 2f, 0);

        westWall.transform.localScale = new Vector3(westWall.transform.localScale.x, height, depth + 0.2f);
        westWall.transform.localPosition = new Vector3((width / 2f) + 0.05f, height / 2f, 0);
    }
}
