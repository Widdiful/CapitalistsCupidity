using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Facility : MonoBehaviour {
    //Public
    public enum FacilityType { Catering, CarPark, Toilets, Insurance, Security, Hardware, Christmas, Wages, Gym }
    public float costToBuy;
    public float baseMonthlyExpenses;
    public string facilityName;
    public FacilityType facilityType;

    //Private
    private float fundingPercentage;
    private float averageEmployeeHappiness;
    private GameObject FacilityCanvas;

	// Use this for initialization
	void Start ()
    {
        FacilityCanvas = GameObject.Find("FacilityCanvas");
	}

    private void CalculateAverageEmployeeHappiness()
    {

    }

    private void CutFacility()
    {

    }

    private void BuyFacility()
    {

    }

    private void OpenFacilityWindow()
    {
        CalculateAverageEmployeeHappiness();
        FacilityCanvas.transform.GetChild(0).Find("ConfirmButton").GetComponent<Button>().onClick.RemoveAllListeners();
        FacilityCanvas.GetComponent<Canvas>().enabled = true;
        FacilityCanvas.transform.GetChild(0).Find("FacilityNameText").GetComponent<Text>().text = facilityName;
        FacilityCanvas.transform.GetChild(0).Find("FundingText").GetComponent<Text>().text = (baseMonthlyExpenses * fundingPercentage).ToString();
        FacilityCanvas.transform.GetChild(0).Find("FundingSlider").GetComponent<Slider>().value = fundingPercentage;
        FacilityCanvas.transform.GetChild(0).Find("HappinessSlider").GetComponent<Slider>().value = averageEmployeeHappiness;
        FacilityCanvas.transform.GetChild(0).Find("ConfirmButton").GetComponent<Button>().onClick.AddListener(this.confirmChanges);
    }

    private void confirmChanges()
    {
        fundingPercentage = FacilityCanvas.transform.GetChild(0).Find("FundingSlider").GetComponent<Slider>().value;
        FacilityCanvas.GetComponent<Canvas>().enabled = false;
    }
}
