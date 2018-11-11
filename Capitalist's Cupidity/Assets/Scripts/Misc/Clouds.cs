using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour {

    public float speed;

    void Start() {
        transform.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
    }

	void Update () {
        transform.Rotate(new Vector3(0, speed, 0));
	}
}
