using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Transform managementButton;
    public Transform managementPane;
    public Transform floorsContent;

    public GameObject floorsButtonPrefab;

    private bool managementPaneOpen = false;
    private Text managementButtonText;

    private Coroutine managementPanelCoroutine;
    private Dictionary<int, FloorButton> floorButtons = new Dictionary<int, FloorButton>();

    void Start() {
        managementButtonText = managementButton.GetComponentInChildren<Text>();
    }

    public void ToggleManagementPane() {
        managementPaneOpen = !managementPaneOpen;

        switch (managementPaneOpen) {
            case true:
                managementButtonText.text = "Close";
                UpdateAllTabs();
                break;

            case false:
                managementButtonText.text = "Open";
                break;
        }

        if (managementPanelCoroutine != null) StopCoroutine(managementPanelCoroutine);
        managementPanelCoroutine = StartCoroutine(MoveManagementPane());
    }

    IEnumerator MoveManagementPane() {
        Vector2 newRectLocation = Vector2.zero;
        Vector2 newButtonLocation = Vector2.zero;
        RectTransform managementPaneRect = managementPane.GetComponent<RectTransform>();
        RectTransform managementButtonRect = managementButton.GetComponent<RectTransform>();

        switch (managementPaneOpen) {
            case true:
                newRectLocation = new Vector2(0, 0);
                newButtonLocation = new Vector2(-109, -30);
                break;

            case false:
                newRectLocation = new Vector2(200, 0);
                newButtonLocation = new Vector2(9, -30);
                break;
        }


        while (true) {
            managementPaneRect.anchoredPosition = Vector2.Lerp(managementPaneRect.anchoredPosition, newRectLocation, 0.2f);
            managementButtonRect.anchoredPosition = Vector2.Lerp(managementButtonRect.anchoredPosition, newButtonLocation, 0.2f);

            yield return null;
        }
    }

    public void UpdateAllTabs()
    {
        UpdateFloorsTab();
    }

    public void UpdateFloorsTab()
    {
        if (floorsContent && floorsButtonPrefab) {
            foreach (Floor floor in FindObjectsOfType<Floor>()) {
                if (!floorButtons.ContainsKey(floor.floorNo)) {
                    FloorButton newFloor = Instantiate(floorsButtonPrefab, floorsContent).GetComponent<FloorButton>();
                    floorButtons.Add(floor.floorNo, newFloor);
                }

                floorButtons[floor.floorNo].floorNo = floor.floorNo;
                floorButtons[floor.floorNo].population = floor.population;
                floorButtons[floor.floorNo].happiness = floor.happiness;
                floorButtons[floor.floorNo].UpdateInformation();
            }
        }
    }
}
