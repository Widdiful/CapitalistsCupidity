using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility : MonoBehaviour {

    //Private
    private FacilityInfo facilityInfo;
    private float fundingPercentage;
    private float averageEmployeeHappiness;
    private GameObject FacilityCanvas;

    // Use this for initialization
    void Start()
    {
        FacilityCanvas = GameObject.Find("FacilityCanvas");
        fundingPercentage = 1;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OpenFacilityWindow();
        }
    }

    private void CalculateAverageEmployeeHappiness()
    {

    }

    public void CutFacility()
    {
        facilityInfo = GameObject.FindObjectOfType<FacilityList>().GetFacilityByName("Empty");
    }

    private void BuyFacility(FacilityInfo facilityInformation)
    {
        facilityInfo = facilityInformation;
    }

    private void OpenFacilityWindow()
    {
        CalculateAverageEmployeeHappiness();
        FacilityCanvas.GetComponent<FacilityCanvas>().OpenFacilityWindow(this, facilityInfo.facilityName, facilityInfo.baseMonthlyExpenses, fundingPercentage, averageEmployeeHappiness); // Open the facility window and populate the values
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
