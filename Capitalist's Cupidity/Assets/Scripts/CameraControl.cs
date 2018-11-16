using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float moveSpeed;
    private int selectedFloor;
    public int visibleFloorLayer;
    public int invisibleFloorLayer;

    private float floorHeight;
    private OfficeGenerator office;

	void Start () {
        office = FindObjectOfType<OfficeGenerator>();
        if (office) {
            floorHeight = office.floorHeight;
        }
        else
            Debug.LogWarning("OfficeGenerator not correctly set up.");

        ChangeFloor(selectedFloor);
	}
	
	void Update () {
        Vector3 targetPos = transform.position;
        targetPos.y = selectedFloor * floorHeight;
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed);

        CheckWalls();
	}

    // Changes layers of floors
    public void ChangeFloor(int val) {
        selectedFloor = Mathf.Clamp(val, 0, office.floorCount - 1);

        foreach (Floor floor in office.GetFloors()) {

            // Set current and previous floor to fully visible
            if (floor.floorNo == selectedFloor || floor.floorNo == selectedFloor - 1){
                foreach (Transform obj in floor.GetComponentsInChildren<Transform>()) {
                    obj.gameObject.layer = visibleFloorLayer;
                }
            }

            // Set all above floors to fully invisible
            else if (floor.floorNo > selectedFloor){
                foreach (Transform obj in floor.GetComponentsInChildren<Transform>()) {
                    obj.gameObject.layer = invisibleFloorLayer;
                }
            }

            // Set all below floors's walls to visible, everything else invisible
            else {
                foreach (Transform obj in floor.GetComponentsInChildren<Transform>()) {
                    if (obj.name.Contains("Wall"))
                        obj.gameObject.layer = visibleFloorLayer;
                    else
                        obj.gameObject.layer = invisibleFloorLayer;
                }
            }
        }

        // Set current camera-facing walls to invisible
        office.GetFloors()[selectedFloor].transform.Find("SouthWall").gameObject.layer = invisibleFloorLayer;
        office.GetFloors()[selectedFloor].transform.Find("WestWall").gameObject.layer = invisibleFloorLayer;
    }

    // Checks which walls to hide
    private void CheckWalls() {
        // Figure out which walls are obscuring the room
        // Set them to invisible
    }
}
