using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacilityCanvas : MonoBehaviour
{
    Text facilityNameText;
    Text fundingText;
    Slider fundingSlider;
    Slider happinessSlider;
    Button confirmButton;
    Button cancelButton;
    Button sabotageButton;
    Facility selectedFacility;
    string facilityName;
    float funding;
    float baseExpense;
    float fundingPercent;

    // Use this for initialization
    void Start ()
    {
        facilityNameText = transform.GetChild(0).Find("FacilityNameText").GetComponent<Text>();
        fundingText = transform.GetChild(0).Find("FundingText").GetComponent<Text>();
        fundingSlider = transform.GetChild(0).Find("FundingSlider").GetComponent<Slider>();
        happinessSlider = transform.GetChild(0).Find("HappinessSlider").GetComponent<Slider>();
        confirmButton = transform.GetChild(0).Find("ConfirmButton").GetComponent<Button>();
        cancelButton = transform.GetChild(0).Find("CancelButton").GetComponent<Button>();
        sabotageButton = transform.GetChild(0).Find("SabotageButton").GetComponent<Button>();

        cancelButton.onClick.AddListener(CancelChanges);
        confirmButton.onClick.AddListener(ConfirmChanges);
        sabotageButton.onClick.AddListener(Sabotage);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(fundingPercent != fundingSlider.value)
        {
            fundingPercent = fundingSlider.value;
            UpdateUI();
        }
	}

    public void OpenFacilityWindow(Facility SelectedFacility, string FacilityName, float BaseExpense, float FundingPercent, float AverageHappiness)
    {
        selectedFacility = SelectedFacility; // Set the selected Facility
        facilityName = FacilityName;
        baseExpense = BaseExpense; // Set the base expense
        fundingPercent = FundingPercent; // Set the funding percent
        funding = (baseExpense * fundingPercent); // Set the funding amount

        GetComponent<Canvas>().enabled = true; // Open the canvas

        facilityNameText.text = facilityName; // Set facility name on canvas


        fundingText.text = "$" + funding.ToString("0.00") + " p/m"; // Set funding number on canvas

        fundingSlider.value = fundingPercent; // Set funding slider position on canvas

        happinessSlider.value = AverageHappiness; // Set happiness slider position on canvas

        sabotageButton.interactable = selectedFacility.canSabotage && selectedFacility.facilityInfo.facilityType != FacilityInfo.FacilityType.WorkSpace;
        sabotageButton.GetComponentInChildren<Text>().text = "Sabotage ($" + selectedFacility.facilityInfo.sabotageCost + ")";
    }

    private void UpdateUI()
    {
        funding = (baseExpense * fundingPercent); // Set the funding amount
        fundingText.text = "$" + funding.ToString("0.00") + " p/m"; // Set funding number on canvas
        if(funding <= 0)
        {
            fundingText.text = "Cut Facility";
        }
    }

    private void ConfirmChanges()
    {
        selectedFacility.UpdateFromFacilityWindow(fundingPercent); // Update the facility with the new values
        GetComponent<Canvas>().enabled = false; // Close the canvas
        UIManager.instance.windowOpen = false;
    }

    private void CancelChanges()
    {
        selectedFacility = null; // Deselect the facility
        GetComponent<Canvas>().enabled = false; // Close the canvas
        UIManager.instance.windowOpen = false;
    }

    private void Sabotage() {
        if (PlayerStats.instance.SpendMoney(selectedFacility.facilityInfo.sabotageCost)) {
            selectedFacility.SabotageFacility();
            sabotageButton.interactable = selectedFacility.canSabotage;
        }
    }

}
