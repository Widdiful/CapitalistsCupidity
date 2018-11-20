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

    //References
    private Text personalFundsTextRef;
    private Text companyFundsTextRef;
    private Text incomeKeptPercentTextRef;
    private Text employeeCountTextRef;

	// Use this for initialization
	void Start ()
    {
        //initalise references
        personalFundsTextRef = GameObject.Find("PersonalFundsText").GetComponent<Text>();
        companyFundsTextRef = GameObject.Find("CompanyFundsText").GetComponent<Text>();
        incomeKeptPercentTextRef = GameObject.Find("IncomeKeptPercentText").GetComponent<Text>();
        employeeCountTextRef = GameObject.Find("EmployeeCountText").GetComponent<Text>();

        companyFunds = startingCompanyFunds;
        incomeKeepPercent = startingIncomeKept;
    }
	
	// Update is called once per frame
	void Update ()
    {
        updateUI();
	}

    public void updateUI()
    {
        personalFundsTextRef.text = "Personal Funds: $" + personalFunds.ToString();
        companyFundsTextRef.text = "Company Funds: $" + companyFunds.ToString();
        incomeKeptPercentTextRef.text = "Income Kept: " + (incomeKeepPercent * 100).ToString() + "%";
        employeeCountTextRef.text = "# Of Employees: " + employeeCount.ToString();
    }

    public float GetCompanyFunds()
    {
        return companyFunds;
    }

    public void ChangeCompanyFunds(float value)
    {
        companyFunds += value;
    }

    public void ChangePersonalFunds(float value)
    {
        personalFunds += value;
    }
}
