using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchase5Coins : MonoBehaviour {

    public int value;

    public void Purchase() {
        MainMenuManager.instance.BuyFiveCoins(value);
    }
}
