using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    public int floorNo;
    public int workspaceCount;
    public enum FloorTypes { Warehouse, Office };
    public FloorTypes floorType;
    private Transform floorArea;
    private Transform northWall;
    private Transform southWall;
    private Transform eastWall;
    private Transform westWall;
    public GameObject workspacePrefab;

    void Awake()
    {
        if (!floorArea) {
            floorArea = transform.Find("Floor");
            northWall = transform.Find("NorthWall");
            southWall = transform.Find("SouthWall");
            eastWall = transform.Find("EastWall");
            westWall = transform.Find("WestWall");
        }
    }


    public void InitialiseFloor(float width, float height, float depth, int number, FloorTypes type, int workspaces) {
        floorNo = number;
        transform.position = new Vector3(width / 2f, floorNo * height, depth / 2f);
        floorType = type;
        workspaceCount = workspaces;

        floorArea.localScale = new Vector3(width, floorArea.localScale.y, depth);
        floorArea.localPosition = Vector3.zero;
        northWall.localScale = new Vector3(width, height, northWall.localScale.z);
        northWall.localPosition = new Vector3(0, height / 2f, (depth / 2f) + 0.05f);
                 
        southWall.localScale = new Vector3(width, height, southWall.localScale.z);
        southWall.localPosition = new Vector3(0, height / 2f, -((depth / 2f) + 0.05f));
                 
        eastWall.localScale = new Vector3(eastWall.localScale.x, height, depth + 0.2f);
        eastWall.localPosition = new Vector3(-((width / 2f) + 0.05f), height / 2f, 0);
                 
        westWall.localScale = new Vector3(westWall.localScale.x, height, depth + 0.2f);
        westWall.localPosition = new Vector3((width / 2f) + 0.05f, height / 2f, 0);

        int workAreaX, workAreaY, workAreaWidth, workAreaHeight, minX, minY;
        workAreaWidth = workspaceCount / 2;
        workAreaHeight = workspaceCount / 2;
        minX = Mathf.FloorToInt((width - 1) - (width / 2)) * -1;
        minY = Mathf.FloorToInt((depth - 1) - (depth / 2));
        workAreaX = Mathf.FloorToInt(Random.Range(minX, -minX - (workAreaWidth / 2) - 2));
        workAreaY = Mathf.FloorToInt(Random.Range(minY, -minY + (workAreaHeight / 2) + 2));

        SpawnWorkspaces(workAreaX, workAreaY, workAreaWidth, workAreaHeight);
    }

    private void SpawnWorkspaces(int x, int y, int width, int height)
    {
        int spacing = 2;
        for (int i = 0; i < height * spacing ; i += spacing)
        {
            for (int j = 0; j < width * spacing; j += spacing)
            {
                GameObject newWorkspace = Instantiate(workspacePrefab, transform.TransformPoint(new Vector3(x + j, workspacePrefab.transform.localScale.y / 2f, y - i)), Quaternion.identity, transform);
            }
        }
    }
}
