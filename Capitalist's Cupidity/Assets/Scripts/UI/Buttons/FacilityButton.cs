﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacilityButton : MonoBehaviour {

    public string facilityName;
    public float fundingCurrent;
    public float happiness;

    public Text nameText;
    public Text fundingText;
    public Scrollbar happinessBar;

    public void UpdateInformation() {
        nameText.text = facilityName;
        fundingText.text = fundingCurrent.ToString();
        happinessBar.value = happiness;
    }

    public void Click() {

    }
}
