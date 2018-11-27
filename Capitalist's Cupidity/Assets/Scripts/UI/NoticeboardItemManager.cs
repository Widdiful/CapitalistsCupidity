using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeboardItemManager : MonoBehaviour {

    public Text titleText;
    public Text bodyText;
    public GameObject commentPrefab;

    public void AddComments(List<string> comments)
    {
        foreach (string comment in comments) {
            GameObject newComment = Instantiate(commentPrefab, transform);
            newComment.GetComponentInChildren<Text>().text = comment;
        }
    }
}
