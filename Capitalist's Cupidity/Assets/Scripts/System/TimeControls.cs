using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControls : MonoBehaviour {

	public void ChangeTimeScale(float val)
    {
        Time.timeScale = val;
    }
}
