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

        public void BuyBusiness()
        {
            purchased = true;
        }

        public float GetIncome()
        {
            return monthlyIncome;
        }
    }

    public List<Business> ListOfBusinesses = new List<Business>();

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
