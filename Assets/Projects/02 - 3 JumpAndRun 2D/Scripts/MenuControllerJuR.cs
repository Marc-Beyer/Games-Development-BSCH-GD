using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuControllerJuR : MonoBehaviour{
    public static MenuControllerJuR Instance;

    [SerializeField] private RectTransform heartImage;
    [SerializeField] private RectTransform heartBGImage;

    [SerializeField] private GameObject messagePanel;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI msgText;

    private PlayerController2D player;

    private List<Message> msgList = new List<Message>();

    public List<Message> MsgList { get => msgList; set => msgList = value; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        player = PlayerController2D.Instance;

        UpdateHearts();
        UpdateMaxHearts();
    }

    private void Update() {
        if (messagePanel.activeSelf && Input.GetKeyDown(KeyCode.E)) {
            if(MsgList.Count > 0) MsgList.RemoveAt(0);

            loadNewMessage();
        }
    }

    public void UpdateHearts() {
        heartImage.sizeDelta = new Vector2(heartImage.sizeDelta.y * player.CurrentHearts, heartImage.sizeDelta.y);
    }

    public void UpdateMaxHearts() {
        heartBGImage.sizeDelta = new Vector2(heartBGImage.sizeDelta.y * player.MaxHearts, heartBGImage.sizeDelta.y);
    }

    public void AddMessage(List<Message> messages) {
        MsgList.AddRange(messages);
        messagePanel.SetActive(true);
        loadNewMessage();
    }
    
    public void AddMessage(Message message) {
        MsgList.Add(message);
        messagePanel.SetActive(true);
        loadNewMessage();
    }

    private void loadNewMessage() { 
        if (MsgList.Count <= 0) {
            messagePanel.SetActive(false);
            return;
        }

        nameText.text = MsgList[0].Name;
        msgText.text = MsgList[0].Text;
    }
}

[System.Serializable]
public class Message {
    public string Name = "";
    public string Text = "";
}