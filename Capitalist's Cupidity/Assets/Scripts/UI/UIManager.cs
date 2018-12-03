﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    // Internal references
    public Transform managementButton;
    public Transform managementPane;
    public Transform floorsContent;
    public GameObject floorsButtonPrefab;
    public Transform employeesContent;
    public GameObject employeesButtonPrefab;
    public Transform facilitiesContent;
    public GameObject facilitiesButtonPrefab;
    public Transform businessesContent;
    public GameObject businessesButtonPrefab;
    public Transform abilitiesContent;
    public GameObject abilitiesButtonPrefab;

    public bool windowOpen;

    private Coroutine managementPanelCoroutine;
    private Animator buttonAnim;
    private Animator paneAnim;

    // Variables
    private bool managementPaneOpen = false;
    private Text managementButtonText;
    private Dictionary<int, FloorButton> floorButtons = new Dictionary<int, FloorButton>();
    private Dictionary<int, EmployeeButton> employeeButtons = new Dictionary<int, EmployeeButton>();

    // External references
    private OfficeGenerator officeGenerator;
    private Director director;
    private Businesses businesses;

    void Start() {
        managementButtonText = managementButton.GetComponentInChildren<Text>();

        officeGenerator = FindObjectOfType<OfficeGenerator>();
        director = FindObjectOfType<Director>();
        businesses = FindObjectOfType<Businesses>();
        buttonAnim = managementButton.GetComponent<Animator>();
        paneAnim = managementPane.GetComponent<Animator>();
    }

    public void ToggleManagementPane() {
        managementPaneOpen = !managementPaneOpen;

        switch (managementPaneOpen) {
            case true:
                managementButtonText.text = "Close";
                buttonAnim.SetBool("Open", true);
                paneAnim.SetBool("Open", true);
                UpdateAllTabs();
                break;

            case false:
                managementButtonText.text = "Open";
                buttonAnim.SetBool("Open", false);
                paneAnim.SetBool("Open", false);
                break;
        }
    }

    IEnumerator MoveManagementPane() {
        Vector2 newRectLocation = Vector2.zero;
        Vector2 newButtonLocation = Vector2.zero;
        RectTransform managementPaneRect = managementPane.GetComponent<RectTransform>();
        RectTransform managementButtonRect = managementButton.GetComponent<RectTransform>();

        switch (managementPaneOpen) {
            case true:
                newRectLocation = new Vector2(0, 0);
                newButtonLocation = new Vector2(-109, -30);
                break;

            case false:
                newRectLocation = new Vector2(200, 0);
                newButtonLocation = new Vector2(9, -30);
                break;
        }


        while (true) {
            managementPaneRect.anchoredPosition = Vector2.Lerp(managementPaneRect.anchoredPosition, newRectLocation, 0.2f);
            managementButtonRect.anchoredPosition = Vector2.Lerp(managementButtonRect.anchoredPosition, newButtonLocation, 0.2f);

            yield return null;
        }
    }

    public void UpdateAllTabs()
    {
        UpdateFloorsTab();
        UpdateEmployeesTab();
        UpdateFacilitiesTab();
        UpdateBusinessTab();
        UpdateAbilitiesTab();
    }

    public void UpdateFloorsTab()
    {
        if (officeGenerator && floorsContent && floorsButtonPrefab) {
            DeleteButtonsInTab(floorsContent);
            for (int i = officeGenerator.GetFloors().Count - 1; i >= 0; i--) {
                Floor floor = officeGenerator.GetFloors()[i];
                FloorButton newFloor = Instantiate(floorsButtonPrefab, floorsContent).GetComponent<FloorButton>();
                newFloor.floorNo = floor.floorNo;
                newFloor.population = floor.population;
                newFloor.happiness = floor.happiness;
                newFloor.UpdateInformation();
            }
        }
    }

    public void UpdateEmployeesTab()
    {
        if (director && employeesContent && employeesButtonPrefab)
        {
            DeleteButtonsInTab(employeesContent);
            foreach(Employee employee in director.employees)
            {
                EmployeeButton newButton = Instantiate(employeesButtonPrefab, employeesContent).GetComponent<EmployeeButton>();
                newButton.employeeName = employee.name;
                newButton.wage = 10f;
                newButton.happiness = employee.getHappiness() / 100f;
                newButton.UpdateInformation();
            }
        }
    }

    public void UpdateFacilitiesTab()
    {
        if (facilitiesContent && facilitiesButtonPrefab)
        {
            DeleteButtonsInTab(facilitiesContent);
            foreach (Facility facility in FindObjectsOfType<Facility>())
            {
                if (facility.facilityInfo.facilityType != FacilityInfo.FacilityType.WorkSpace)
                {
                    FacilityButton newButton = Instantiate(facilitiesButtonPrefab, facilitiesContent).GetComponent<FacilityButton>();
                    newButton.facilityName = facility.facilityInfo.facilityName;
                    newButton.fundingCurrent = facility.GetMonthlyExpense();
                    newButton.happiness = facility.averageEmployeeHappiness;
                    newButton.UpdateInformation();
                }
            }
        }
    }

    public void UpdateBusinessTab()
    {
        if (businesses && businessesContent && businessesButtonPrefab)
        {
            DeleteButtonsInTab(businessesContent);
            foreach (Businesses.Business business in businesses.ListOfBusinesses)
            {
                if (!business.purchased)
                {
                    BusinessButton newButton = Instantiate(businessesButtonPrefab, businessesContent).GetComponent<BusinessButton>();
                    newButton.businessName = business.businessName;
                    newButton.cost = business.costToBuy;
                    newButton.earnings = business.monthlyIncome;
                    newButton.UpdateInformation();
                }
            }
        }
    }

    public void UpdateAbilitiesTab()
    {

    }

    private void DeleteButtonsInTab(Transform tab)
    {
        foreach(Transform button in tab)
        {
            Destroy(button.gameObject);
        }
    }
}
