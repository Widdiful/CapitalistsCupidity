using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacilityButton : MonoBehaviour {

    public string facilityName;
    public int floorNo;
    public float fundingCurrent;
    public float happiness;
    public Facility facility;

    public Text nameText;
    public Text fundingText;
    public Scrollbar happinessBar;

    public void UpdateInformation() {
        nameText.text = facilityName;
        fundingText.text = floorNo.ToString();
        happinessBar.value = happiness;
    }

    public void Click() {
        if (facility)
            facility.OpenFacilityWindow();
    }
}
