using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Facility : MonoBehaviour
{
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
        fundingPercentage = 1;
	}

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            OpenFacilityWindow();
        }
    }

    private void CalculateAverageEmployeeHappiness()
    {

    }

    public void CutFacility()
    {
        // Cut facility here
    }

    private void BuyFacility()
    {

    }

    private void OpenFacilityWindow()
    {
        CalculateAverageEmployeeHappiness();
        FacilityCanvas.GetComponent<FacilityCanvas>().OpenFacilityWindow(this, facilityName, baseMonthlyExpenses, fundingPercentage, averageEmployeeHappiness); // Open the facility window and populate the values
    }

    public void UpdateFromFacilityWindow(float FundingPercentage)
    {
        fundingPercentage = FundingPercentage; // Set funding value based on slider position
        if (fundingPercentage <= 0)
        {
            CutFacility(); // if the funding percentage is now 0, then cut the facility.
        }
    }
}
