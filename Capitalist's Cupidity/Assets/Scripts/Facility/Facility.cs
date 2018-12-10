using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility : MonoBehaviour
{

    //Private
    public FacilityInfo facilityInfo;
    private float fundingPercentage;
    public float averageEmployeeHappiness;
    private GameObject facilityCanvas;
    private Canvas purchaseCanvas;
    private PurchaseFacilityCanvas purchaseFacility;

    // Use this for initialization
    void Start()
    {
        facilityCanvas = GameObject.Find("FacilityCanvas");
        purchaseCanvas = GameObject.Find("PurchaseFacilityCanvas").GetComponent<Canvas>();
        purchaseFacility = GameObject.FindObjectOfType<PurchaseFacilityCanvas>();

        fundingPercentage = 1;

    }

    private void CalculateAverageEmployeeHappiness()
    {

    }

    public void CutFacility()
    {
        facilityInfo = GameObject.FindObjectOfType<FacilityList>().GetFacilityByName("Empty");
        name = "Empty";
    }

    public void BuyFacility(FacilityInfo facilityInformation)
    {
        GameObject.FindObjectOfType<PlayerStats>().ChangeCompanyFunds(-facilityInformation.costToBuy);
        fundingPercentage = 1;
        facilityInfo = facilityInformation;
        name = facilityInfo.facilityName;
    }

    public void OpenFacilityWindow()
    {
    
        if (CheckIfEmpty())
        {
            OpenBuyFacilityWindow();
        }
        else
        {
            OpenChangeFacilityWindow();
        }
    }
    public void OpenChangeFacilityWindow()
    {
        CalculateAverageEmployeeHappiness();
        facilityCanvas.GetComponent<FacilityCanvas>().OpenFacilityWindow(this, facilityInfo.facilityName, facilityInfo.baseMonthlyExpenses, fundingPercentage, averageEmployeeHappiness); // Open the facility window and populate the values
        GameObject.FindObjectOfType<UIManager>().openedWindow = facilityCanvas.GetComponent<Canvas>();
    }

    public void OpenBuyFacilityWindow()
    {
        purchaseCanvas.enabled = true;
        purchaseFacility.SetFacility(this);
        GameObject.FindObjectOfType<UIManager>().openedWindow = purchaseCanvas;
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
