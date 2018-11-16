using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Messages : MonoBehaviour {

    public Text tickerMessage;
    public float speed;

    private RectTransform tickerRect;
    private RectTransform tickerCanvas;
    private Vector2 tickerStart;

    private bool tickerFinished;

    void Start() {
        if (tickerMessage) {
            tickerRect = tickerMessage.rectTransform;
            tickerStart = tickerRect.anchoredPosition;
            tickerCanvas = tickerMessage.canvas.GetComponent<RectTransform>();
        }
        else
            Debug.LogWarning("Cannot set tickerRect as tickerMessage is null.");

        CreateTicker("Lorem ipsum dolor sit amet, consectetur adipiscing elit.Aliquam quis leo vel ex egestas aliquam.Vivamus dignissim sed nisi in molestie.Nulla lacinia non sapien quis eleifend.Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.Nam vel tincidunt tortor.Mauris tempus mi diam, et ultrices urna lobortis eget.Sed maximus vehicula ipsum, non molestie magna.Praesent sed eros sed enim tempus dictum. Vivamus et efficitur lectus, eget faucibus arcu.Nunc pretium, orci non tempor pretium, nulla leo mattis lectus, eget maximus eros ante non justo.Pellentesque dapibus urna vitae maximus porttitor.Praesent et neque eget augue placerat mattis.Maecenas malesuada, arcu non elementum molestie, erat mi consectetur nibh, at dictum magna lorem et augue.Etiam tristique imperdiet elit, at vulputate turpis porta non.Phasellus posuere et augue eget viverra.Etiam vitae lorem vitae diam auctor efficitur id eget tellus.Morbi vehicula ac nisi id lacinia.Etiam blandit lectus non faucibus sollicitudin. Nulla faucibus, dui sed convallis tristique, quam turpis pretium dolor, sit amet fringilla eros velit a nisl.Curabitur eros dolor, posuere at mauris vel, viverra vulputate justo.Proin varius lacinia turpis, nec interdum diam facilisis at.Nam tincidunt massa at nulla tristique, id volutpat mauris euismod.Fusce sagittis bibendum aliquet.Phasellus a eros tincidunt, cursus libero sit amet, dignissim nisi.Suspendisse malesuada ultricies neque in vestibulum.Duis non cursus nibh, non sodales ante.Pellentesque rhoncus fringilla nisl, lobortis luctus metus maximus non.Donec fringilla arcu vel consectetur pulvinar.Quisque nisl erat, mollis et lacus eu, faucibus viverra lacus.Nunc lacinia efficitur suscipit.Fusce non magna sed lectus euismod rhoncus.Aliquam id pharetra felis. Ut faucibus enim purus, ac finibus dolor tempor ac.Sed elementum lectus id pretium blandit.Praesent vel sodales felis, in cursus risus.In efficitur suscipit bibendum.Donec sodales luctus faucibus.Nullam posuere efficitur odio, non semper nisl elementum ut.Duis eleifend lectus consectetur nisl pretium dictum.Mauris eget nibh mollis, condimentum tellus at, congue ex.Vestibulum placerat ex in sem sagittis, id volutpat erat sollicitudin.Sed congue, dui ut feugiat fringilla, nisi turpis euismod purus, sed pharetra quam ex nec ligula.Donec in nunc lectus.Vestibulum accumsan dictum nulla, a blandit orci aliquam quis. Maecenas vitae dignissim justo.Aenean ac consequat odio.Cras a est quam.Proin at suscipit magna.Vivamus fermentum nec lacus ac ornare.Curabitur volutpat nulla nisi, sit amet iaculis eros eleifend a.Donec turpis erat, rhoncus eget elit quis, iaculis vestibulum justo.Vivamus gravida, orci vitae vestibulum mollis, ligula nulla laoreet mi, non luctus felis lorem vitae mi.Phasellus ullamcorper pharetra risus, a laoreet nisl auctor vitae. ");
    }

    public void CreateTicker(string message) {
        if (tickerMessage) {
            tickerMessage.text = message;

            StartCoroutine(MoveTicker());
        }
        else
            Debug.LogWarning("Cannot create ticker as tickerMessage is null.");
    }

    IEnumerator MoveTicker() {
        if (tickerMessage) {
            tickerRect.anchoredPosition = new Vector2((tickerCanvas.sizeDelta.x * 0.5f) + tickerMessage.preferredWidth * 0.5f, tickerStart.y);
            Vector2 moveVector = new Vector2(speed, 0);

            bool endOfMessageShown = false;
            tickerFinished = false;
            while (IsVisibleOnCanvas(new Vector2(tickerRect.anchoredPosition.x + (tickerRect.sizeDelta.x / 2f), tickerRect.anchoredPosition.y)) || !endOfMessageShown) {
                if (IsVisibleOnCanvas(new Vector2(tickerRect.anchoredPosition.x + (tickerRect.sizeDelta.x / 2f), tickerRect.anchoredPosition.y)))
                    endOfMessageShown = true;

                tickerRect.anchoredPosition -= moveVector;

                yield return null;
            }
            endOfMessageShown = false;
            tickerFinished = true;

            // Check queue for more messages
        }
        else
            Debug.LogWarning("Cannot move ticker as tickerMessage is null.");
    }

    private bool IsVisibleOnCanvas(Vector2 point) {
        return tickerCanvas.rect.Contains(point);
    }
}
