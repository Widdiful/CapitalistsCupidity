using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusinessMenu : MonoBehaviour {

    public Businesses.Business business;
    public Text nameText;
    public Text descriptionText;
    public Text costText;
    public Text earningText;

    public void UpdateUI()
    {
        nameText.text = business.businessName;
        descriptionText.text = business.description;
        costText.text = "$" + business.costToBuy.ToString("n0") + ",000,000";
        earningText.text = "$" + business.monthlyIncome.ToString("n0");
    }

    public void Buy()
    {
        if (PlayerStats.instance.SpendMoney(business.costToBuy * 1000000))
        {
            business.BuyBusiness();
            UIManager.instance.CloseOpenedWindow();
            UIManager.instance.UpdateBusinessTab();
        }
    }
}
