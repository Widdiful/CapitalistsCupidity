using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusinessMenu : MonoBehaviour {

    public Businesses.Business business;
    public Text nameText;

    public void UpdateUI()
    {
        nameText.text = business.businessName;
    }

    public void Buy()
    {
        if (PlayerStats.instance.SpendMoney(business.costToBuy))
        {
            business.BuyBusiness();
            UIManager.instance.CloseOpenedWindow();
            UIManager.instance.UpdateBusinessTab();
        }
    }
}
