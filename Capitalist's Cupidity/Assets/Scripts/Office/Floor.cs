using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    public int floorNo;
    public int population;
    public float happiness;
    public int workspaceCount;
    public enum FloorTypes { Warehouse, Office };
    public FloorTypes floorType;
    private Transform floorArea;
    private Transform northWall;
    private Transform southWall;
    private Transform eastWall;
    private Transform westWall;
    public GameObject workspacePrefab;
    private int spacing;
    private float stairWidth = 1.5f;
    private float stairDepth = 6.0f;
    private Vector2 floorSize;

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


    public void InitialiseFloor(float width, float height, float depth, int number, FloorTypes type, int workspaces, int padding) {

        // Initialise variables
        floorNo = number;
        floorSize = new Vector2(width, depth);
        transform.position = new Vector3(width / 2f, floorNo * height, depth / 2f);
        floorType = type;
        workspaceCount = workspaces;
        spacing = padding;

        // Set wall sizes appropriately
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

        // Set work area
        int workAreaX, workAreaY, workAreaWidth, workAreaHeight, minX, minY;
        workAreaWidth = GetRandomFactor(workspaceCount);
        workAreaHeight = workspaceCount / workAreaWidth;
        minX = Mathf.FloorToInt((width - 1) - (width / 2)) * -1;
        minY = Mathf.FloorToInt((depth - 1) - (depth / 2));
        //workAreaX = Mathf.FloorToInt(Random.Range(minX, -minX - (workAreaWidth / 2) - (2 * spacing)));
        //workAreaY = Mathf.FloorToInt(Random.Range(minY, -minY + (workAreaHeight / 2) + (2 * spacing)));
        workAreaX = -3;
        workAreaY = 3;

        SpawnWorkspaces(workAreaX, workAreaY, workAreaWidth, workAreaHeight);

        if (floorNo > 0)
        {
            AddStairHole();
        }
    }

    // Spawns work space in given area
    private void SpawnWorkspaces(int x, int y, int width, int height)
    {
        for (int i = 0; i < height * spacing ; i += spacing)
        {
            for (int j = 0; j < width * spacing; j += spacing)
            {
                GameObject newWorkspace = Instantiate(workspacePrefab,
                    transform.TransformPoint(new Vector3(x + j, workspacePrefab.transform.localScale.y / 2f, y - i)),
                    Quaternion.identity, transform);
            }
        }
    }

    // Returns a random factor from a given integer
    private int GetRandomFactor(int val)
    {
        int result = 0; 
        List<int> factors = new List<int>();

        for(int i = 2; i < val; i++)
        {
            if (val % i == 0)
            {
                factors.Add(i);
            }
        }

        if (factors.Count > 0)
        {
            result = factors[Random.Range(0, factors.Count)];
        }

        return result;
    }

    // Adjusts floor to create a hole for the stairs
    public void AddStairHole()
    {
        Transform newFloor = Instantiate(floorArea, transform);
        newFloor.localScale = new Vector3(stairWidth, newFloor.localScale.y, floorSize.y - stairDepth);
        newFloor.localPosition = new Vector3((floorSize.x * 0.5f) - (stairWidth * 0.5f), 0, -(stairDepth * 0.5f));
        floorArea.localScale = new Vector3(floorArea.localScale.x - stairWidth, floorArea.localScale.y, floorArea.localScale.z);
        floorArea.localPosition = new Vector3(floorArea.localPosition.x - (stairWidth * 0.5f), floorArea.localPosition.y, floorArea.localPosition.z);
    }

    // Adds a staircase
    public void AddStairs()
    {
        GameObject newStairs = Instantiate(Resources.Load("OfficeParts/Stairs"), transform) as GameObject;
        newStairs.transform.localPosition = new Vector3(floorSize.x * 0.5f, 0, (floorSize.y / 2f) - stairDepth);
    }

    // Adds a lift
    public void AddLift()
    {

    }
}
