using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour {

    public float moveSpeed;
    public int selectedFloor;
    public int visibleFloorLayer;
    public int invisibleFloorLayer;

    private float floorHeight;

    private float mouseStartX;
    private bool changedFloor = false;
    private bool canRotateCamera = false;

    public static CameraControl instance;

    public Text floorCounter;

    void Awake() {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }

    void Start () {
        floorHeight = OfficeGenerator.instance.floorHeight;

        ChangeFloor(0);
	}
	
	void Update () {
        Vector3 targetPos = transform.position;
        targetPos.y = selectedFloor * floorHeight;
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed);

        if (Input.touchCount > 0) {
            if (Input.touches[0].phase == TouchPhase.Began) {
                mouseStartX = Input.touches[0].position.x;
                canRotateCamera = true;
            }

            if (Input.touches[0].phase == TouchPhase.Moved && !EventSystem.current.IsPointerOverGameObject() && canRotateCamera) {
                transform.Rotate(0, (Input.touches[0].position.x - mouseStartX) * 0.1f, 0);
                mouseStartX = Input.touches[0].position.x;
                //CheckWalls();
            }
            if (Input.touches[0].phase == TouchPhase.Ended) {
                canRotateCamera = false;
            }
        }
        else {
            if (Input.GetMouseButtonDown(0)) {
                mouseStartX = Input.mousePosition.x;
                canRotateCamera = true;
            }

            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && canRotateCamera) {
                transform.Rotate(0, (Input.mousePosition.x - mouseStartX) * 0.1f, 0);
                mouseStartX = Input.mousePosition.x;
                //CheckWalls();
            }
            else if (!Input.GetMouseButtonDown(0)) {
                canRotateCamera = false;
            }
        }

        // really bad fix for a bug but idk what else to do
        if (!changedFloor) {
            ChangeFloor(selectedFloor);
            changedFloor = true;
        }
	}

    // Changes layers of floors
    public void ChangeFloor(int val) {
        selectedFloor = Mathf.Clamp(val, 0, OfficeGenerator.instance.maxFloors - 1);

        foreach (Floor floor in OfficeGenerator.instance.GetFloors()) {

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
                    if (obj.name.Contains("Wall")) { 
                        obj.gameObject.layer = visibleFloorLayer;
                    }
                    else
                    {
                        obj.gameObject.layer = invisibleFloorLayer;
                    }
                }
                foreach (Light light in floor.GetComponentsInChildren<Light>()) {
                    light.enabled = false;
                }
            }

            if (floor.floorNo == selectedFloor)
            {
                foreach(Transform obj in floor.GetComponentsInChildren<Transform>())
                {
                    if (obj.name.Contains("OuterWall"))
                        obj.gameObject.layer = invisibleFloorLayer;
                }
            }
            else if (floor.floorNo < selectedFloor)
            {
                foreach (Transform obj in floor.GetComponentsInChildren<Transform>())
                {
                    if (obj.name.Contains("OuterWall"))
                        obj.gameObject.layer = visibleFloorLayer;
                }
            }
        }

        foreach (Employee emp in Director.Instance.employees) {
            if (emp.gameObject.activeInHierarchy)
                emp.changeEmployeeLayer();
        }

        floorCounter.text = selectedFloor.ToString();

        //CheckWalls();

        // Set current camera-facing walls to invisible
        //office.GetFloors()[selectedFloor].transform.Find("SouthWall").gameObject.layer = invisibleFloorLayer;
        //office.GetFloors()[selectedFloor].transform.Find("WestWall").gameObject.layer = invisibleFloorLayer;
    }

    public void MoveFloor(int adjust) {
        ChangeFloor(selectedFloor + adjust);
    }

    // Checks which walls to hide
    private void CheckWalls() {
        Transform currentFloor = OfficeGenerator.instance.GetFloors()[selectedFloor].transform;

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
