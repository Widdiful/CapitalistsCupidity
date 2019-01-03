using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Businesses : MonoBehaviour
{
    [System.Serializable]
    public class Business
    {
        public string businessName;
        public float costToBuy;
        public float monthlyIncome;
        public bool purchased;
        public string description;

        public void BuyBusiness()
        {
            purchased = true;
            Messages.instance.NewMessage(businessName + " has been purchased.", Messages.MessageType.Ticker);
        }

        public float GetIncome()
        {
            return monthlyIncome;
        }
    }

    public List<Business> ListOfBusinesses = new List<Business>();

    public static Businesses instance;

    void Awake() {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }

    public float GetTotalMonthlyIncome()
    {
        float income = 0;
        foreach(Business business in ListOfBusinesses)
        {
            if(business.purchased)
            {
                income += business.GetIncome();
            }
        }
        return income;
    }
}
