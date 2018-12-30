using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class PurchaseFacilityCanvas : MonoBehaviour {

    public GameObject buttonPrefab;
    private FacilityList facilityList;
    private Facility selectedFacility;
    private ToggleGroup toggleGroup;
    private Canvas canvas;
    private Toggle selectedToggle;
    private FacilityInfo selectedFacilityInfo;
    private PlayerStats playerStats;
    private Button confirmButton;
    private Floor selectedFloor;
    private OfficeGenerator officeGenerator;
    // Use this for initialization
    void Start ()
    {
        officeGenerator = GameObject.FindObjectOfType<OfficeGenerator>();
        canvas = GameObject.Find("PurchaseFacilityCanvas").GetComponent<Canvas>();
        facilityList = FacilityList.instance;
        toggleGroup = GetComponent<ToggleGroup>();
        playerStats = PlayerStats.instance;
        confirmButton = canvas.transform.Find("PurchaseButton").GetComponent<Button>();
        foreach (FacilityInfo facility in facilityList.facilityList)
        {
            if(facility.facilityType != FacilityInfo.FacilityType.Empty && facility.facilityType != FacilityInfo.FacilityType.Copy)
            {
                GameObject newButton = GameObject.Instantiate(buttonPrefab, this.transform);
                newButton.GetComponent<Toggle>().group = toggleGroup;
                newButton.transform.Find("Name").GetComponent<Text>().text = facility.facilityName;
                newButton.transform.Find("CostToBuy").GetComponent<Text>().text = "Purchase: $" + facility.costToBuy.ToString("0.00");
                newButton.transform.Find("CostPerMonth").GetComponent<Text>().text = "Monthly Cost: $" + facility.baseMonthlyExpenses.ToString("0.00");
            }
        }
	}

    private void Update()
    {
        if(canvas.enabled)
        {
            if (selectedToggle != toggleGroup.ActiveToggles().FirstOrDefault<Toggle>())
            {
                selectedToggle = toggleGroup.ActiveToggles().FirstOrDefault<Toggle>();
                selectedFacilityInfo = facilityList.GetFacilityByName(selectedToggle.transform.Find("Name").GetComponent<Text>().text);
            }
            if(selectedToggle != null)
            {
                if (playerStats.GetCompanyFunds() >= selectedFacilityInfo.costToBuy && selectedFacility.CheckFacilitySize(selectedFacilityInfo, selectedFacility.GetFloor()))
                {
                    confirmButton.interactable = true;
                }
                else
                {
                    confirmButton.interactable = false;
                }
            }
            else
            {
                confirmButton.interactable = false;
            }
        }
    }

    public void SetFacility(Facility facility)
    {
        selectedFacility = facility;
        selectedFloor = facility.GetFloor();

        for(int i = 0; i < selectedFloor.facilityArray.Count; i++)
        {
            for(int j = 0; j < selectedFloor.facilityArray[i].row.Count; j++)
            {
                if(selectedFloor.facilityArray[i].row[j] == facility)
                {
                    selectedFacility.xPosition = i;
                    selectedFacility.yPosition = j;
                }
            }
        }
    }

    public void PurchaseFacility()
    {
        if(selectedFacility != null)
        {
            selectedFacility.BuyFacility(selectedFacilityInfo);
        }
        ClosePurchaseFacilityWindow();
    }

    public void ClosePurchaseFacilityWindow()
    {
        selectedFacility = null;
        canvas.enabled = false;
        UIManager.instance.windowOpen = false;
    }
}
