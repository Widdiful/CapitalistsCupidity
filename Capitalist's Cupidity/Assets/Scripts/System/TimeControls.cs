using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControls : MonoBehaviour {

    private void Start() {
        Time.timeScale = 1;
    }

    public void ChangeTimeScale(float val)
    {
        Time.timeScale = val;
    }
}
