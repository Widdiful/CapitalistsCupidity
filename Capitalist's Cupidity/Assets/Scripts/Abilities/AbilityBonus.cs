using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBonus : BaseAbility {
    public float bonusAmount;
    public float happinessGain;

    private void Update()
    {
        abilityCost = GameObject.FindObjectsOfType<Employee>().Length * bonusAmount;
    }
    protected override void Effect()
    {
        foreach(Employee employee in GameObject.FindObjectsOfType<Employee>())
        {
            //increase employee happiness;
        }
        playerStats.ChangeCompanyFunds(-abilityCost);
    }
}
