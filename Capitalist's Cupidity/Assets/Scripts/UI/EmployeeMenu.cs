using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeMenu : MonoBehaviour {

    public Employee employee;
    public Text nameText;
    public Text floorText;
    public Text earningsText;
    public Text salaryText;
    public Scrollbar happinessBar;

    public void UpdateUI()
    {
        nameText.text = employee.name;
        floorText.text = "Floor #" + employee.assignedFloor.ToString();
        earningsText.text = "123";
        salaryText.text = "10";
        happinessBar.size = employee.getHappiness() / 100f;
    }

    public void Fire()
    {
        employee.fireEmployee();
        UIManager.instance.CloseOpenedWindow();
        UIManager.instance.UpdateEmployeesTab();
    }
}
