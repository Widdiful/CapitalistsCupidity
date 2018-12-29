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
    private GameObject childObject;
    private int floor;
    private int padding;
    // Use this for initialization
    void Start()
    {
        facilityCanvas = GameObject.Find("FacilityCanvas");
        purchaseCanvas = GameObject.Find("PurchaseFacilityCanvas").GetComponent<Canvas>();
        purchaseFacility = GameObject.FindObjectOfType<PurchaseFacilityCanvas>();
        padding = GameObject.FindObjectOfType<OfficeGenerator>().workspacePadding;
        fundingPercentage = 1;
        childObject = GameObject.Instantiate(facilityInfo.child, transform);
    }

    private void CalculateAverageEmployeeHappiness()
    {

    }

    public void CutFacility()
    {
        facilityInfo = FacilityList.instance.GetFacilityByName("Empty");
        if(childObject)
        {
            Destroy(childObject.gameObject);
        }
        if(facilityInfo.child != null)
        {
            GameObject.Instantiate(facilityInfo.child, transform);
        }
        name = "Empty";
    }

    public void BuyFacility(FacilityInfo facilityInformation, int xDirection, int yDirection)
    {
        PlayerStats.instance.ChangeCompanyFunds(-facilityInformation.costToBuy);
        fundingPercentage = 1;
        facilityInfo = facilityInformation;
        name = facilityInfo.facilityName;
        if (transform.GetChild(0))
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        if (facilityInfo.child != null)
        {
            childObject = GameObject.Instantiate(facilityInfo.child, transform);
            if(facilityInformation.width > 1 || facilityInformation.height > 1)
            {
                childObject.transform.localPosition = new Vector3(((facilityInformation.width / 2) * padding / 2) * xDirection, 0 , ((facilityInformation.height / 2) * padding/ 2) * yDirection);
            }
        }
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
        UIManager.instance.openedWindow = facilityCanvas.GetComponent<Canvas>();
    }

    public void OpenBuyFacilityWindow()
    {
        purchaseCanvas.enabled = true;
        purchaseFacility.SetFacility(this);
        UIManager.instance.openedWindow = purchaseCanvas;
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

    public void SetFloor(int floorNo)
    {
        floor = floorNo;
    }

    public int GetFloor()
    {
        return floor;
    }
}
