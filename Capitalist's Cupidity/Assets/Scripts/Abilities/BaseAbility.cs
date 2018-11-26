using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    public string abilityName;
    public float abilityCost;
    public float cooldown;
    public bool singleUse;
    public bool unlocked;

    protected float currentCooldown;
    protected int uses;

    // Use this for initialization
    void Start()
    {
        currentCooldown = 0;
        uses = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void activate()
    {
        if (GetCanUse())
        {
            Effect();
        }
    }

    public bool GetCanUse()
    {
        if(currentCooldown <= 0 && unlocked)
        {
            if(!singleUse || (singleUse && uses == 0))
            {
                return true;
            }
        }
        return false;
    }

    protected virtual void Effect()
    {

    }
}
