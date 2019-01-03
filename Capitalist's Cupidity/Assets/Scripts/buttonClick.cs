using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonClick : MonoBehaviour {

    public AudioSource audioSource;

    private void Start() {
        foreach (Button button in FindObjectsOfType<Button>()) {
            button.onClick.AddListener(() => Click());
        }
    }

    public void Click() {
        audioSource.Play();
    }
}
