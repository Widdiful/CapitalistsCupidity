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
    public float incomeKeepPercent;
    private int employeeCount;
    private int fiveCoins;

    //References
    private Text personalFundsTextRef;
    private Text companyFundsTextRef;
    private Text fiveCoinTextRef;
    private Text employeeCountTextRef;

    public static PlayerStats instance;

    void Awake() {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }

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
    }
	
	// Update is called once per frame
	void Update ()
    {
        updateUI();
	}

    public void updateUI()
    {
        personalFundsTextRef.text = "$" + personalFunds.ToString("n2");
        companyFundsTextRef.text = "$" + companyFunds.ToString("n2");
        fiveCoinTextRef.text = fiveCoins.ToString();
        employeeCountTextRef.text = Director.Instance.totalActiveEmployees().ToString("n0");
    }

    public int GetFiveCoin()
    {
        return fiveCoins;
    }

    public void SetFiveCoin(int amount)
    {
        fiveCoins = amount;
    }

    public int getEmployeeCount()
    {
        return employeeCount;
    }

    public float GetCompanyFunds()
    {
        return companyFunds;
    }

    public void SetCompanyFunds(float amount)
    {
        companyFunds = amount;
    }

    public float GetPersonalFunds() {
        return personalFunds;
    }

    public void SetPersonalFunds(float amount)
    {
        personalFunds = amount;
    }

    public bool SpendMoney(float amount)
    {
        if (companyFunds >= amount)
        {
            ChangeCompanyFunds(-amount);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangeCompanyFunds(float income)
    {
        if(income > 0)
        {
            companyFunds += income - (income * incomeKeepPercent);
        }
        else
        {
            companyFunds += income;
        }

    }

    public void ChangePersonalFunds(float income)
    {
        personalFunds += income * incomeKeepPercent;
    }

    public void SetFiveCoins(int amount) {
        fiveCoins = amount;
    }
}
