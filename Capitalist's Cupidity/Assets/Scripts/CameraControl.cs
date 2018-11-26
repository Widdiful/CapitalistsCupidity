using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class CameraControl : MonoBehaviour {

    public float moveSpeed;
    private int selectedFloor;
    public int visibleFloorLayer;
    public int invisibleFloorLayer;

    private float floorHeight;
    private OfficeGenerator office;

    private float mouseStartX;

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

        if (Input.GetMouseButtonDown(0))
        {
            mouseStartX = Input.mousePosition.x;
        }

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            transform.Rotate(0, (Input.mousePosition.x - mouseStartX) * 0.1f, 0);
            mouseStartX = Input.mousePosition.x;
            CheckWalls();
        }


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
                foreach (Light light in floor.GetComponentsInChildren<Light>()) {
                    light.enabled = true;
                }
            }

            // Set all above floors to fully invisible
            else if (floor.floorNo > selectedFloor){
                foreach (Transform obj in floor.GetComponentsInChildren<Transform>()) {
                    obj.gameObject.layer = invisibleFloorLayer;
                }
                foreach (Light light in floor.GetComponentsInChildren<Light>()) {
                    light.enabled = false;
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
                foreach (Light light in floor.GetComponentsInChildren<Light>()) {
                    light.enabled = false;
                }
            }
        }

        CheckWalls();

        // Set current camera-facing walls to invisible
        //office.GetFloors()[selectedFloor].transform.Find("SouthWall").gameObject.layer = invisibleFloorLayer;
        //office.GetFloors()[selectedFloor].transform.Find("WestWall").gameObject.layer = invisibleFloorLayer;
    }

    // Checks which walls to hide
    private void CheckWalls() {
        Transform currentFloor = office.GetFloors()[selectedFloor].transform;

        Vector3 leftPos = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height * 0.5f));
        leftPos.y = currentFloor.position.y + 0.1f;
        Vector3 rightPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height * 0.5f));
        rightPos.y = currentFloor.position.y + 0.1f;

        foreach (Transform child in currentFloor) {
            if (child.name.Contains("Wall")) child.gameObject.layer = visibleFloorLayer;
        }

        RaycastHit[] hits = Physics.RaycastAll(currentFloor.position, (leftPos - currentFloor.position).normalized, 10f).Concat(Physics.RaycastAll(currentFloor.position, (rightPos - currentFloor.position).normalized, 10f)).ToArray();
        foreach (RaycastHit hit in hits) {
            if (hit.collider.name.Contains("Wall")) {
                hit.collider.gameObject.layer = invisibleFloorLayer;
            }
        }
    }
}
