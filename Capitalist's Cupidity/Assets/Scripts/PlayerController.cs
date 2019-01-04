using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera cameraRef;
    private bool justTapped;
	// Use this for initialization
	void Start ()
    {
        cameraRef = GameObject.FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.touchCount > 0 && !justTapped)
        {
            justTapped = true;
            Ray SelectRay = cameraRef.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit[] hits = Physics.RaycastAll(SelectRay, Mathf.Infinity, LayerMask.GetMask("VisibleFloor"));
            System.Array.Reverse(hits);
            foreach (RaycastHit SelectHit in hits)
            {
                if (SelectHit.collider)
                {
                    Facility facility = SelectHit.transform.GetComponent<Facility>();
                    Employee employee = SelectHit.transform.GetComponent<Employee>();
                    if (facility || employee)
                    {
                        if (!UIManager.instance.windowOpen)
                        {
                            if (employee)
                            {
                                if (employee.currentFloor == CameraControl.instance.selectedFloor)
                                {
                                    UIManager.instance.OpenEmployeeWindow(SelectHit.transform.GetComponent<Employee>());
                                    break;
                                }
                            }
                            else if (facility)
                            {
                                if (facility.GetFloor().floorNo == CameraControl.instance.selectedFloor)
                                {
                                    SelectHit.transform.GetComponent<Facility>().OpenFacilityWindow();
                                    UIManager.instance.windowOpen = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        if (Input.touchCount == 0)
        {
            justTapped = false;
        }

#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
        {
            Ray SelectRay = cameraRef.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(SelectRay, Mathf.Infinity, LayerMask.GetMask("VisibleFloor"));
            System.Array.Reverse(hits);
            foreach (RaycastHit SelectHit in hits)
            { 
                if(SelectHit.collider)
                {
                    Facility facility = SelectHit.transform.GetComponent<Facility>();
                    Employee employee = SelectHit.transform.GetComponent<Employee>();
                    if (facility || employee)
                    { 
                        if (!UIManager.instance.windowOpen)
                        {
                            if (employee)
                            {
                                if (employee.currentFloor == CameraControl.instance.selectedFloor) {
                                    UIManager.instance.OpenEmployeeWindow(SelectHit.transform.GetComponent<Employee>());
                                    break;
                                }
                            }
                            else if (facility)
                            {
                                if (facility.GetFloor().floorNo == CameraControl.instance.selectedFloor) {
                                    SelectHit.transform.GetComponent<Facility>().OpenFacilityWindow();
                                    UIManager.instance.windowOpen = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
#endif
    }
}
