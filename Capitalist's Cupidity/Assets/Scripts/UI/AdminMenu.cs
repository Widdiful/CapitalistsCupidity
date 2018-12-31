using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminMenu : MonoBehaviour {

    public Text incomePercentageText;
    public Slider incomePercentageSlider;
    public Text hoursPlayedText;
    public Transform businessBox;
    public GameObject purchasedBusinessListItem;

    private void Start() {
        incomePercentageSlider.value = PlayerStats.instance.startingIncomeKept;
        UpdateIncomePercentage(PlayerStats.instance.startingIncomeKept);
    }

    public void UpdateUI() {
        foreach(Text text in businessBox.GetComponentsInChildren<Text>()) {
            if (text.name != "Text") {
                Destroy(text.gameObject);
            }
        }

        int count = 0;
        foreach(Businesses.Business business in Businesses.instance.ListOfBusinesses) {
            if (business.purchased) {
                Text newText = Instantiate(purchasedBusinessListItem, businessBox).GetComponent<Text>();
                newText.text = business.businessName;
                count++;
            }
        }

        if (count == 0) {
            Text newText = Instantiate(purchasedBusinessListItem, businessBox).GetComponent<Text>();
            newText.text = "None";
        }

        StartCoroutine(moveBusinessBox());
    }

    public void UpdateIncomePercentage(float newPercentage) {
        float newValue = Mathf.Round(newPercentage * 100f) / 100f;
        PlayerStats.instance.incomeKeepPercent = newValue;
        incomePercentageText.text = Mathf.Floor(newValue * 100).ToString() + "%";
    }

    private void FixedUpdate() {
        hoursPlayedText.text = GameModeManager.instance.timeSpentUnscaledString;
    }

    IEnumerator moveBusinessBox() {
        //businessBox.SetAsFirstSibling();
        yield return new WaitForEndOfFrame();
        businessBox.SetAsLastSibling();
    }
}
