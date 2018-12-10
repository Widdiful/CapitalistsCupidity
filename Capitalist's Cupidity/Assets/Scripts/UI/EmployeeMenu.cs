using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeMenu : MonoBehaviour {

    public Employee employee;
    public Text nameText;

    public void UpdateUI()
    {
        nameText.text = employee.name;
    }

    public void Fire()
    {
        employee.gameObject.SetActive(false);
        UIManager.instance.CloseOpenedWindow();
    }
}
