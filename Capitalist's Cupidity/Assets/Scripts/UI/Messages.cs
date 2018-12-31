using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Messages : MonoBehaviour {

    [System.Serializable]
    public class NoticeboardItem
    {
        public string title;
        public string message;
        public List<string> comments = new List<string>();

        public NoticeboardItem(string _title, string _message)
        {
            title = _title;
            message = _message;
        }
    }

    public enum MessageType { Ticker, Noticeboard, Both };
    public Transform noticeboardContent;
    public GameObject noticeboardItemPrefab;
    public Text tickerMessage;
    public RectTransform tickerCanvas;
    public float speed;

    public TextAsset negativeCommentsText;
    public TextAsset positiveCommentsText;
    private string[] negativeComments;
    private string[] positiveComments;

    private Queue<string> messageQueue = new Queue<string>();
    public List<NoticeboardItem> noticeboard = new List<NoticeboardItem>();
    private RectTransform tickerRect;
    private Vector2 tickerStart;
    private Animator anim;

    private bool tickerFinished = true;

    public static Messages instance;

    void Awake() {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }

    void Start() {
        if (tickerMessage) {
            tickerRect = tickerMessage.rectTransform;
            tickerStart = tickerRect.anchoredPosition;
        }
        else
            Debug.LogWarning("Cannot set tickerRect as tickerMessage is null.");

        anim = tickerMessage.transform.parent.GetComponent<Animator>();
        negativeComments = negativeCommentsText.text.Split('\n');
        positiveComments = positiveCommentsText.text.Split('\n');

        //NewMessage("Test1", MessageType.Ticker);
        //NewMessage("Test2", MessageType.Ticker);
        //NewMessage("Test3", MessageType.Ticker);

        //CreateNoticeboardMessage("test1", "This is a test.");
        //CreateNoticeboardMessage("test2", "This is a test.");
        //CreateNoticeboardMessage("test3", "This is a test.");
    }

    public void NewMessage(string message, MessageType messageType) {
        if (messageType != MessageType.Noticeboard) {
            messageQueue.Enqueue(message);
            if (tickerFinished) {
                CreateTicker(messageQueue.Dequeue());
            }
        }
        if (messageType != MessageType.Ticker) {

        }
    }

    public void CreateNoticeboardMessage(string title, string message) {
        NoticeboardItem newNotice = new NoticeboardItem(title, message);
        noticeboard.Add(newNotice);
        for (int i = 0; i < 3; i++)
        {
            AddNegativeComment(newNotice);
        }
        NoticeboardItemManager newNoticeManager = Instantiate(noticeboardItemPrefab, noticeboardContent).GetComponent<NoticeboardItemManager>();
        newNoticeManager.titleText.text = newNotice.title;
        newNoticeManager.bodyText.text = newNotice.message;
        newNoticeManager.AddComments(newNotice.comments);
        newNoticeManager.transform.SetAsFirstSibling();
    }

    public void AddNegativeComment(NoticeboardItem noticeboardItem) {
        noticeboardItem.comments.Add(negativeComments[Random.Range(0, negativeComments.Length)]);
    }

    public void AddPositiveComment(NoticeboardItem noticeboardItem) {
        noticeboardItem.comments.Add(positiveComments[Random.Range(0, positiveComments.Length)]);
    }

    private void CreateTicker(string message) {
        if (tickerMessage) {
            tickerMessage.text = message;

            anim.SetBool("Open", true);
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
            if (messageQueue.Count > 0) {
                CreateTicker(messageQueue.Dequeue());
            }
            else {
                anim.SetBool("Open", false);
            }
        }
        else
            Debug.LogWarning("Cannot move ticker as tickerMessage is null.");
    }

    private bool IsVisibleOnCanvas(Vector2 point) {
        return tickerCanvas.rect.Contains(point);
    }
}
