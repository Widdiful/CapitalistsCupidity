using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float moveSpeed;
    public int selectedFloor;
    private float floorHeight;

	void Start () {
        floorHeight = FindObjectOfType<OfficeGenerator>().floorHeight;
	}
	
	void Update () {
        Vector3 targetPos = transform.position;
        targetPos.y = selectedFloor * floorHeight;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed);
	}
}
