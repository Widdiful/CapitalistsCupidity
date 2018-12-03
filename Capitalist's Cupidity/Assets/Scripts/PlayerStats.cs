using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : MonoBehaviour
{
    //Public
    public float startingCompanyFunds = 10000;
    public float startingIncomeKept = 0.2f;

    //Private
    private float personalFunds;
    private float companyFunds;
    private float incomeKeepPercent;
    private int employeeCount;
    private int fiveCoins;

    //References
    private Text personalFundsTextRef;
    private Text companyFundsTextRef;
    private Text fiveCoinTextRef;
    private Text employeeCountTextRef;

	// Use this for initialization
	void Start ()
    {
        //initalise references
        personalFundsTextRef = GameObject.Find("PersonalFunds").GetComponent<Text>();
        companyFundsTextRef = GameObject.Find("CompanyFunds").GetComponent<Text>();
        fiveCoinTextRef = GameObject.Find("5Coins").GetComponent<Text>();
        employeeCountTextRef = GameObject.Find("EmployeeCount").GetComponent<Text>();

        companyFunds = startingCompanyFunds;
        incomeKeepPercent = startingIncomeKept;
        fiveCoins = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        updateUI();
	}

    public void updateUI()
    {
        personalFundsTextRef.text = "Personal Funds: $" + personalFunds.ToString("##,#0.00");
        companyFundsTextRef.text = "Company Funds: $" + companyFunds.ToString("##,#0.00");
        fiveCoinTextRef.text = "5Coins: " + fiveCoins.ToString();
        employeeCountTextRef.text = "# Of Employees: " + employeeCount.ToString();
    }

    public float GetCompanyFunds()
    {
        return companyFunds;
    }

    public void ChangeCompanyFunds(float income)
    {
        companyFunds += income - (income * incomeKeepPercent);
    }

    public void ChangePersonalFunds(float income)
    {
        personalFunds += income * incomeKeepPercent;
    }
}
