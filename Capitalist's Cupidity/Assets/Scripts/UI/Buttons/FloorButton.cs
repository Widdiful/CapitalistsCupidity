using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorButton : MonoBehaviour {

    public int floorNo;
    public int population;
    public float happiness;

    public Text nameText;
    public Text populationText;
    public Scrollbar happinessBar;

    private CameraControl cameraControl;

    void Start()
    {
        cameraControl = FindObjectOfType<CameraControl>();
    }

    public void UpdateInformation()
    {
        string name = "Floor " + floorNo;
        if (name == "Floor 0") {
            name = "Ground floor";
        }
        nameText.text = name;
        populationText.text = population.ToString();
        happinessBar.size = happiness;
    }

    public void Click()
    {
        cameraControl.ChangeFloor(floorNo);
    }
}
