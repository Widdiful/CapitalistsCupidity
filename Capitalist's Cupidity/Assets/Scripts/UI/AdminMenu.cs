using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminMenu : MonoBehaviour {

    public Text incomePercentageText;
    public Slider incomePercentageSlider;

    private void Start() {
        incomePercentageSlider.value = PlayerStats.instance.startingIncomeKept;
        UpdateIncomePercentage(PlayerStats.instance.startingIncomeKept);
    }

    public void UpdateIncomePercentage(float newPercentage) {
        float newValue = Mathf.Round(newPercentage * 100f) / 100f;
        PlayerStats.instance.incomeKeepPercent = newValue;
        incomePercentageText.text = Mathf.Floor(newValue * 100).ToString() + "%";
    }
}
