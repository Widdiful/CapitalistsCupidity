using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusinessButton : MonoBehaviour {

    public string businessName;
    public float cost;
    public float earnings;

    public Text nameText;
    public Text costText;
    public Text earningsText;

    public void UpdateInformation() {
        nameText.text = businessName;
        costText.text = cost.ToString();
        earningsText.text = earnings.ToString();
    }

    public void Click() {

    }
}
