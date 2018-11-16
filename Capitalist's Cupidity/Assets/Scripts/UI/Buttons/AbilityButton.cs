using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour {

    public string abilityName;
    public float cost;

    public Text nameText;
    public Text costText;

    public void UpdateInformation() {
        nameText.text = abilityName;
        costText.text = cost.ToString();
    }

    public void Click() {

    }
}
