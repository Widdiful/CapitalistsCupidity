using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeButton : MonoBehaviour {

    public string employeeName;
    public float wage;
    public float happiness;

    public Text nameText;
    public Text wageText;
    public Scrollbar happinessBar;

    public void UpdateInformation() {
        nameText.text = employeeName;
        wageText.text = "£" + wage + "/hr";
        happinessBar.value = happiness;
    }

    public void Click() {

    }
}
