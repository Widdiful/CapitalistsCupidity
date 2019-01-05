using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    public Dropdown gameTypeDropdown;
    public InputField companyNameField;
    public Text coinText;

    public string companyName;
    public int gameType;
    public int fiveCoins;

    public static MainMenuManager instance;

    private void Awake() {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
        DontDestroyOnLoad(this);
        //fiveCoins = PlayerPrefs.GetInt("fiveCoins");
    }

    private void Start() {
        savingAndLoading.instance.loadProfileData();
    }

    public void StartNewGame() {
        companyName = companyNameField.text;
        gameType = gameTypeDropdown.value;
    }

    public void ConfirmStartNewGame() {
        FindObjectOfType<LoadLevel>().Load(1);
        GetComponent<Canvas>().enabled = false;
    }

    public void BuyFiveCoins(int amount) {
        fiveCoins += amount;
        //PlayerPrefs.SetInt("fiveCoins", fiveCoins);
        savingAndLoading.instance.saveProfileData();
        coinText.text = "Owned 5Coins: " + MainMenuManager.instance.fiveCoins.ToString("n0");
    }
}
