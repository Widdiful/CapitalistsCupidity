using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItem : MonoBehaviour {

    public Text positionText;
    public Text nameText;
    public Text companyText;
    public Text scoreText;


    public int position;
    public string playerName;
    public string companyName;
    public string score;

    public void UpdateInformation() {
        positionText.text = "#" + position;
        nameText.text = playerName;
        companyText.text = companyName;
        scoreText.text = score;
    }
}
