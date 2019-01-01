using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeButton : MonoBehaviour {

    public string employeeName;
    public int floor;
    public float happiness;
    public Employee employee;

    public Text nameText;
    public Text floorText;
    public Scrollbar happinessBar;

    public void UpdateInformation() {
        nameText.text = employeeName;
        floorText.text = floor.ToString();
        happinessBar.size = happiness;
    }

    public void Click() {
        UIManager.instance.OpenEmployeeWindow(employee);
    }

    public void Hire() {
        Director.Instance.HireEmployee();
    }
}
