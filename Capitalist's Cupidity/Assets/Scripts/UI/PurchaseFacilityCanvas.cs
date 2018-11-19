using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseFacilityCanvas : MonoBehaviour {

    public GameObject buttonPrefab;
    private FacilityList facilityList;
    private Facility selectedFacility;

	// Use this for initialization
	void Start ()
    {
        facilityList = GameObject.FindObjectOfType<FacilityList>();
        foreach(FacilityInfo facility in facilityList.facilityList)
        {
            if(facility.facilityType != FacilityInfo.FacilityType.Empty)
            {
                GameObject newButton = GameObject.Instantiate(buttonPrefab, this.transform);
                newButton.transform.Find("Name").GetComponent<Text>().text = facility.facilityName;
                newButton.transform.Find("CostToBuy").GetComponent<Text>().text = "Purchase: $" + facility.costToBuy.ToString("0.00");
                newButton.transform.Find("CostPerMonth").GetComponent<Text>().text = "Monthly Cost: $" + facility.baseMonthlyExpenses.ToString("0.00");
            }
        }
	}

    public void SetFacility(Facility facility)
    {
        selectedFacility = facility;
    }


}
