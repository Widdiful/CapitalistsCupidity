using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera camera;

	// Use this for initialization
	void Start ()
    {
        camera = GameObject.FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Ray SelectRay = camera.ScreenPointToRay(Input.mousePosition);
            if(Input.touchCount > 0)
            {
                SelectRay = camera.ScreenPointToRay(Input.GetTouch(0).position);
            }
            RaycastHit SelectHit;
            Physics.Raycast(SelectRay, out SelectHit);
            if(SelectHit.collider)
            {
                if(SelectHit.transform.GetComponent<Facility>())
                {
                    if (SelectHit.transform.GetComponent<Facility>().CheckIfEmpty())
                    {

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
