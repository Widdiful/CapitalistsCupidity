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
    private Floor floor;
    private int padding;
    public Facility CopyOf;
    public int xDirection, yDirection;
    public int xPosition, yPosition;
    public List<Employee> employees = new List<Employee>();
    // Use this for initialization
    void Start()
    {
        facilityCanvas = GameObject.Find("FacilityCanvas");
        purchaseCanvas = GameObject.Find("PurchaseFacilityCanvas").GetComponent<Canvas>();
        purchaseFacility = GameObject.FindObjectOfType<PurchaseFacilityCanvas>();
        padding = GameObject.FindObjectOfType<OfficeGenerator>().workspacePadding;
        fundingPercentage = 1;
        childObject = GameObject.Instantiate(facilityInfo.child, transform);
        xDirection = 1;
        yDirection = 1;
    }

    private void CalculateAverageEmployeeHappiness()
    {
        if(employees.Count > 0)
        {
            float total = 0;
            foreach(Employee employee in employees)
            {
                total += employee.getHappiness();
            }
            total /= employees.Count;
        }
    }

    public void CutFacility()
    {
        foreach (Floor.FacilityRow column in floor.facilityArray)
        {
            foreach (Facility facility in column.row) // loop through all the facilities on this floor
            {
                if (facility.CopyOf == this || facility == this) // if they are this facility or a copy of it, then set it to empty.
                {
                    facility.facilityInfo = FacilityList.instance.GetFacilityByName("Empty");
                    facility.name = facility.facilityInfo.facilityName;
                    if (facility.childObject)
                    {
                        Destroy(facility.childObject.gameObject);
                    }
                    if (facility.facilityInfo.child != null)
                    {
                        facility.childObject = GameObject.Instantiate(facility.facilityInfo.child, facility.transform);
                    }
                }
            }
        }
    }

    public void BuyFacility(FacilityInfo facilityInformation)
    {
        PlayerStats.instance.ChangeCompanyFunds(-facilityInformation.costToBuy);
        ChangeFacility(facilityInformation);
    }

    public void ChangeFacility(FacilityInfo facilityInformation)
    {
        int noOfFAcilities = 2; //3-1
        //Check empty on X+ and Y+
        for (int x = 0; Mathf.Abs(x) < facilityInformation.height; x += xDirection) // iterate through x in xdirection
        {
            for (int y = 0; Mathf.Abs(y) < facilityInformation.width; y += yDirection) // iterate through y in ydirection
            {
                if ((xPosition + x <= noOfFAcilities && xPosition + x >= 0) && (yPosition + y <= noOfFAcilities && yPosition + y >= 0))
                {
                    if (floor.facilityArray[xPosition + x].row[yPosition + y].facilityInfo.facilityType == FacilityInfo.FacilityType.Empty)
                    {
                        if (x == 0 && y == 0) // if tile is the starting one, place facility
                        {
                            fundingPercentage = 1;
                            facilityInfo = facilityInformation;
                            name = facilityInfo.facilityName;

                            if (childObject.gameObject) // if there's a child object
                            {
                                Destroy(childObject.gameObject);    // destroy it
                            }
                            if (facilityInfo.child != null)
                            {
                                childObject = GameObject.Instantiate(facilityInfo.child, transform); // spawn the new facility child object if it exists
                            }

                            float xMove = ((facilityInformation.width / 2) * padding / 2) * yDirection; // set the move distance on the x axis to the middle of all the facilitie spaces used
                            if (facilityInfo.width == 1)
                            {
                                xMove = 0;
                            }

                            float zMove = ((facilityInformation.height / 2) * padding / 2) * xDirection; // set the move distance on the z axis to the middle of all the facilitie spaces used
                            if (facilityInfo.height == 1)
                            {
                                zMove = 0;
                            }

                            childObject.transform.localPosition = new Vector3(xMove, 0, -zMove); // move child object to the middle
                        }
                        else // otherwise make it a copy of placed facility
                        {
                            Facility copyFacility = floor.facilityArray[xPosition + x].row[yPosition + y];
                            copyFacility.fundingPercentage = 1;
                            copyFacility.facilityInfo = FacilityList.instance.GetFacilityByName("Copy");
                            copyFacility.name = copyFacility.facilityInfo.facilityName;
                            copyFacility.CopyOf = this;

                            if (copyFacility.childObject)
                            {
                                Destroy(copyFacility.childObject.gameObject);
                            }
                            if (copyFacility.facilityInfo.child != null)
                            {
                                copyFacility.childObject = GameObject.Instantiate(copyFacility.facilityInfo.child, copyFacility.transform);
                            }
                        }
                    }
                }
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
        if (facilityInfo.facilityType == FacilityInfo.FacilityType.Copy)
        {
            CopyOf.OpenFacilityWindow();
        }
        else
        {
            CalculateAverageEmployeeHappiness();
            facilityCanvas.GetComponent<FacilityCanvas>().OpenFacilityWindow(this, facilityInfo.facilityName, facilityInfo.baseMonthlyExpenses, fundingPercentage, averageEmployeeHappiness); // Open the facility window and populate the values
            UIManager.instance.openedWindow = facilityCanvas.GetComponent<Canvas>();
        }
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

    public void SetFloor(Floor floorNo)
    {
        floor = floorNo;
    }

    public Floor GetFloor()
    {
        return floor;
    }

    public bool CheckFacilitySize(FacilityInfo facilityInformation, Floor selectedFloor)
    {
        bool buildOK = false;
        float noOfFAcilities = 2; //3-1
        int xMod = 1;
        int yMod = 1;
        for (int i = 0; i < 4; i++)
        {
            bool fail = false; //break boolean for second for loop
            if (!buildOK)
            {
                for (int x = 0; Mathf.Abs(x) < facilityInformation.height; x += xMod) // loop through the x in the array
                {
                    if (fail)
                    {
                        break; // if the previous check failed, immediately exit this loop to start a new directional check
                    }
                    xDirection = xMod;
                    for (int y = 0; Mathf.Abs(y) < facilityInformation.width; y += yMod) // loop through the y in the array
                    {
                        yDirection = yMod;
                        if ((xPosition + x <= noOfFAcilities && xPosition + x >= 0) && (yPosition + y <= noOfFAcilities && yPosition + y >= 0)) // out of bounds check
                        {
                            if (selectedFloor.facilityArray[xPosition + x].row[yPosition + y].facilityInfo.facilityType == FacilityInfo.FacilityType.Empty) // if space is empty
                            {
                                buildOK = true;
                            }
                            else
                            {
                                buildOK = false;
                                fail = true;
                                break;
                            }
                        }
                        else
                        {
                            buildOK = false;
                            fail = true;
                            break;
                        }
                    }

                }
            }
            if(buildOK)
            {
                break;
            }
            if (xMod == 1)
            {
                xMod = (-1);
            }
            else if(xMod == -1)
            {
                yMod = (-1);
                xMod = 1;
            }
        }
        // if all tiles needed are empty, return true for buildable
        return buildOK;
    }
}
