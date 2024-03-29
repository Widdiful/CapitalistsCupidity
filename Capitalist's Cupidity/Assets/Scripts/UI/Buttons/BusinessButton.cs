﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusinessButton : MonoBehaviour {

    public string businessName;
    public float cost;
    public float earnings;
    public Businesses.Business business;

    public Text nameText;
    public Text costText;
    public Text earningsText;

    public void UpdateInformation() {
        nameText.text = businessName;
        costText.text = "$" + cost.ToString("n0") + "m";
        earningsText.text = "$" + earnings.ToString("n0");
    }

    public void Click() {
        UIManager.instance.OpenBusinessWindow(business);
    }
}
