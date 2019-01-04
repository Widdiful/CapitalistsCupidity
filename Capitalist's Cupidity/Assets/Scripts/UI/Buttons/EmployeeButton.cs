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
        nameText.text = employee.name;
        floorText.text = employee.assignedFloor.ToString();
        happinessBar.size = employee.getHappiness() / 100.0f;
    }

    public void Click() {
        UIManager.instance.OpenEmployeeWindow(employee);
    }

    public void Hire() {
        if (OfficeGenerator.instance.GetNumberOfWorkspaces() > Director.Instance.getCurrentEmployees())
            Director.Instance.HireEmployee();
    }
}
