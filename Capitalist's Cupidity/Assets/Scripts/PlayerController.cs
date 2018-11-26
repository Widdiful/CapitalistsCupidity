using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera cameraRef;

	// Use this for initialization
	void Start ()
    {
        cameraRef = GameObject.FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Ray SelectRay = cameraRef.ScreenPointToRay(Input.mousePosition);
            if(Input.touchCount > 0)
            {
                SelectRay = cameraRef.ScreenPointToRay(Input.GetTouch(0).position);
            }
            RaycastHit SelectHit;
            Physics.Raycast(SelectRay, out SelectHit, Mathf.Infinity, LayerMask.GetMask("VisibleFloor"));
            if(SelectHit.collider)
            {
                if(SelectHit.transform.GetComponent<Facility>())
                {
                    if (SelectHit.transform.GetComponent<Facility>().CheckIfEmpty())
                    {
                        SelectHit.transform.GetComponent<Facility>().OpenBuyFacilityWindow();
                    }
                    else
                    {
                        SelectHit.transform.GetComponent<Facility>().OpenFacilityWindow();
                    }
                }
            }
        }
	}
}
