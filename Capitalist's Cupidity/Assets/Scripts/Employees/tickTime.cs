using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class tickTime : MonoBehaviour {

    TimeSpan currentTime;
    public float monthInRealMinutes;

	// Use this for initialization
	void Start ()
    {
        currentTime = TimeSpan.FromMinutes(0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        currentTime += TimeSpan.FromSeconds(Time.deltaTime);

        if (currentTime >= TimeSpan.FromMinutes(monthInRealMinutes))
        {
            Director.Instance.numberOfMonths++;
            currentTime = TimeSpan.FromMinutes(0);
        }
	}
}
