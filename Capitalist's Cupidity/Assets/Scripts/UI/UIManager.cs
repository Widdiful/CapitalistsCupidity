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
    public Canvas adminMenuCanvas;
    public Canvas employeeMenuCanvas;
    public Canvas businessMenuCanvas;
    public Canvas gameOverMenu;

    private List<FloorButton> floorButtons = new List<FloorButton>();
    private List<EmployeeButton> employeeButtons = new List<EmployeeButton>();
    private List<FacilityButton> facilityButtons = new List<FacilityButton>();
    private List<BusinessButton> businessButtons = new List<BusinessButton>();

    public bool windowOpen;
    public Canvas openedWindow;

    private Coroutine managementPanelCoroutine;
    private Animator buttonAnim;
    private Animator paneAnim;

    private float updateTimer;

    // Variables
    private bool managementPaneOpen = false;
    private Text managementButtonText;

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

        GameObject newButton;
        for (int i = OfficeGenerator.instance.floorCount - 1; i >= 0; i--) {
            FloorButton newFloor = Instantiate(floorsButtonPrefab, floorsContent).GetComponent<FloorButton>();
            newFloor.floorNo = i;
            floorButtons.Add(newFloor);
        }
        for (int i = 0; i < Director.Instance.employeePoolCount; i++) {
            newButton = Instantiate(employeesButtonPrefab, employeesContent);
            newButton.SetActive(false);
            employeeButtons.Add(newButton.GetComponent<EmployeeButton>());
        }
        for (int i = 0; i < OfficeGenerator.instance.floorCount * OfficeGenerator.instance.workspaceCount; i++) {
            newButton = Instantiate(facilitiesButtonPrefab, facilitiesContent);
            newButton.SetActive(false);
            facilityButtons.Add(newButton.GetComponent<FacilityButton>());
        }
        for (int i = 0; i < Businesses.instance.ListOfBusinesses.Count; i++) {
            newButton = Instantiate(businessesButtonPrefab, businessesContent);
            newButton.SetActive(false);
            businessButtons.Add(newButton.GetComponent<BusinessButton>());
        }
    }

    private void FixedUpdate() {
        if (managementPaneOpen) {
            updateTimer += Time.fixedUnscaledDeltaTime;
            if (updateTimer >= 1) {
                switch (managementPane.GetChild(managementPane.childCount - 1).name) {
                    case "FloorsTab":
                        UpdateFloorsTab();
                        break;
                    case "EmployeesTab":
                        UpdateEmployeesTab();
                        break;
                    case "FacilitiesTab":
                        UpdateFacilitiesTab();
                        break;
                    case "BusinessTab":
                        UpdateBusinessTab();
                        break;
                }
                updateTimer = 0;
            }
        }
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
            for (int i = 0; i < floorButtons.Count; i++) {
                Floor floor = OfficeGenerator.instance.GetFloors()[i];
                floorButtons[i].UpdateInformation();
            }
        }
    }

    public void UpdateEmployeesTab()
    {
        if (Director.Instance && employeesContent && employeesButtonPrefab)
        {
            for(int i = 0; i < employeeButtons.Count; i++) {
                Employee emp = Director.Instance.employees[i];
                if (emp.gameObject.activeInHierarchy) {
                    employeeButtons[i].gameObject.SetActive(true);
                    employeeButtons[i].employee = emp;
                    employeeButtons[i].UpdateInformation();
                }
                else {
                    employeeButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void UpdateFacilitiesTab()
    {
        if (facilitiesContent && facilitiesButtonPrefab)
        {
            for (int i = 0; i < facilityButtons.Count; i++) {
                Facility facility = FindObjectsOfType<Facility>()[i];
                if (facility.facilityInfo.facilityType != FacilityInfo.FacilityType.WorkSpace && facility.facilityInfo.facilityType != FacilityInfo.FacilityType.Empty && facility.facilityInfo.facilityType != FacilityInfo.FacilityType.Copy) {
                    facilityButtons[i].gameObject.SetActive(true);
                    facilityButtons[i].facilityName = facility.facilityInfo.facilityName;
                    facilityButtons[i].fundingCurrent = facility.GetMonthlyExpense();
                    facilityButtons[i].happiness = 0.5f;
                    facilityButtons[i].facility = facility;
                    facilityButtons[i].floorNo = facility.GetComponentInParent<Floor>().floorNo;
                    facilityButtons[i].UpdateInformation();
                }
                else {
                    facilityButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void UpdateBusinessTab()
    {
        if (Businesses.instance && businessesContent && businessesButtonPrefab)
        {
            for (int i = 0; i < businessButtons.Count; i++) {
                Businesses.Business business = Businesses.instance.ListOfBusinesses[i];
                if (!business.purchased) {
                    businessButtons[i].gameObject.SetActive(true);
                    businessButtons[i].businessName = business.businessName;
                    businessButtons[i].cost = business.costToBuy;
                    businessButtons[i].earnings = business.monthlyIncome;
                    businessButtons[i].business = business;
                    businessButtons[i].UpdateInformation();
                }
                else {
                    businessButtons[i].gameObject.SetActive(false);
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
            if (button.name != "HireButton")
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

        Time.timeScale = 0;
        pauseMenuCanvas.enabled = true;
        openedWindow = pauseMenuCanvas;
        windowOpen = true;
    }

    public void OpenAdminMenu() {
        if (openedWindow == adminMenuCanvas) {
            CloseOpenedWindow();
            return;
        }

        if (windowOpen) {
            CloseOpenedWindow();
        }

        adminMenuCanvas.enabled = true;
        openedWindow = adminMenuCanvas;
        windowOpen = true;
        adminMenuCanvas.GetComponent<AdminMenu>().UpdateUI();
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

    public void OpenBusinessWindow(Businesses.Business business)
    {
        if (windowOpen)
        {
            CloseOpenedWindow();
        }

        businessMenuCanvas.enabled = true;
        openedWindow = businessMenuCanvas;
        windowOpen = true;

        BusinessMenu menu = businessMenuCanvas.GetComponent<BusinessMenu>();
        menu.business = business;
        menu.UpdateUI();
    }

    public void OpenGameOverWindow() {
        if (windowOpen) {
            CloseOpenedWindow();
        }

        gameOverMenu.enabled = true;
        openedWindow = gameOverMenu;
        windowOpen = true;
    }

    public void QuitToMenu() {

        if (GameModeManager.instance.gameMode == GameModeManager.GameModes.Free) {
            LocalDatabase.LocalDatabaseItem newScore = new LocalDatabase.LocalDatabaseItem();
            newScore.companyName = OfficeGenerator.instance.officeName;
            newScore.score = PlayerStats.instance.GetPersonalFunds().ToString("#0.00");
            LocalDatabase.instance.databaseFree.Add(newScore);
            savingAndLoading.instance.saveLeaderboards();
        }
        FindObjectOfType<LoadLevel>().Load(0);
    }
}
