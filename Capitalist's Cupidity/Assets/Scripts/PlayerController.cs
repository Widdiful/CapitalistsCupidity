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
        if(Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && !justTapped))
        {
            justTapped = true;
            Ray SelectRay = cameraRef.ScreenPointToRay(Input.mousePosition);
            if(Input.touchCount > 0)
            {
                SelectRay = cameraRef.ScreenPointToRay(Input.GetTouch(0).position);
            }
            //RaycastHit SelectHit;
            //Physics.Raycast(SelectRay, out SelectHit, Mathf.Infinity, LayerMask.GetMask("VisibleFloor"));
            RaycastHit[] hits = Physics.RaycastAll(SelectRay, Mathf.Infinity, LayerMask.GetMask("VisibleFloor"));
            System.Array.Reverse(hits);
            foreach (RaycastHit SelectHit in hits)
            { 
                if(SelectHit.collider)
                {
                    if (SelectHit.transform.GetComponent<Facility>() || SelectHit.transform.GetComponent<Employee>())
                    { 
                        if (!UIManager.instance.windowOpen)
                        {
                            if (SelectHit.transform.GetComponent<Employee>())
                            {
                                UIManager.instance.OpenEmployeeWindow(SelectHit.transform.GetComponent<Employee>());
                                break;
                            }
                            else if (SelectHit.transform.GetComponent<Facility>())
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
        if(Input.touchCount == 0)
        {
            justTapped = false;
        }
	}
}
