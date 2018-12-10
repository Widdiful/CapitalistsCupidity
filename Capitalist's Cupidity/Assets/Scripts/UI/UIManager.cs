using System.Collections;
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
    public Canvas pauseMenuCanvas;
    public Canvas employeeMenuCanvas;

    public bool windowOpen;
    public Canvas openedWindow;

    private Coroutine managementPanelCoroutine;
    private Animator buttonAnim;
    private Animator paneAnim;

    // Variables
    private bool managementPaneOpen = false;
    private Text managementButtonText;
    private Dictionary<int, FloorButton> floorButtons = new Dictionary<int, FloorButton>();
    private Dictionary<int, EmployeeButton> employeeButtons = new Dictionary<int, EmployeeButton>();

    public static UIManager instance;

    void Awake() {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }

    void Start() {
        managementButtonText = managementButton.GetComponentInChildren<Text>();

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
        if (OfficeGenerator.instance && floorsContent && floorsButtonPrefab) {
            DeleteButtonsInTab(floorsContent);
            for (int i = OfficeGenerator.instance.GetFloors().Count - 1; i >= 0; i--) {
                Floor floor = OfficeGenerator.instance.GetFloors()[i];
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
        if (Director.Instance && employeesContent && employeesButtonPrefab)
        {
            DeleteButtonsInTab(employeesContent);
            foreach(Employee employee in Director.Instance.employees)
            {
                EmployeeButton newButton = Instantiate(employeesButtonPrefab, employeesContent).GetComponent<EmployeeButton>();
                newButton.employeeName = employee.name;
                newButton.floor = employee.assignedFloor;
                newButton.happiness = employee.getHappiness() / 100f;
                newButton.employee = employee;
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
                if (facility.facilityInfo.facilityType != FacilityInfo.FacilityType.WorkSpace && facility.facilityInfo.facilityType != FacilityInfo.FacilityType.Empty)
                {
                    FacilityButton newButton = Instantiate(facilitiesButtonPrefab, facilitiesContent).GetComponent<FacilityButton>();
                    newButton.facilityName = facility.facilityInfo.facilityName;
                    newButton.fundingCurrent = facility.GetMonthlyExpense();
                    newButton.happiness = 0.5f;
                    newButton.facility = facility;
                    newButton.floorNo = facility.GetComponentInParent<Floor>().floorNo;
                    newButton.UpdateInformation();
                }
            }
        }
    }

    public void UpdateBusinessTab()
    {
        if (Businesses.instance && businessesContent && businessesButtonPrefab)
        {
            DeleteButtonsInTab(businessesContent);
            foreach (Businesses.Business business in Businesses.instance.ListOfBusinesses)
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

    public void CloseOpenedWindow()
    {
        openedWindow.enabled = false;
        openedWindow = null;
        windowOpen = false;
    }

    public void OpenPauseMenu()
    {
        if (openedWindow == pauseMenuCanvas)
        {
            CloseOpenedWindow();
            return;
        }

        if (windowOpen)
        {
            CloseOpenedWindow();
        }

        pauseMenuCanvas.enabled = true;
        openedWindow = pauseMenuCanvas;
        windowOpen = true;
    }

    public void OpenEmployeeWindow(Employee employee)
    {
        if (windowOpen)
        {
            CloseOpenedWindow();
        }

        employeeMenuCanvas.enabled = true;
        openedWindow = employeeMenuCanvas;
        windowOpen = true;

        EmployeeMenu menu = employeeMenuCanvas.GetComponent<EmployeeMenu>();
        menu.employee = employee;
        menu.UpdateUI();
    }
}
