using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility : MonoBehaviour {

    //Private
    public FacilityInfo facilityInfo;
    private float fundingPercentage;
    public float averageEmployeeHappiness;
    private GameObject FacilityCanvas;

    // Use this for initialization
    void Start()
    {
        FacilityCanvas = GameObject.Find("FacilityCanvas");
        fundingPercentage = 1;

        //debug
        //facilityInfo = GameObject.FindObjectOfType<FacilityList>().GetFacilityByName("Cafeteria");
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

    public void OpenFacilityWindow()
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

    public float GetMonthlyExpense()
    {
        return facilityInfo.baseMonthlyExpenses * fundingPercentage;
    }

    public bool CheckIfEmpty()
    {
        if (facilityInfo.facilityType == FacilityInfo.FacilityType.Empty)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
