using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    [SerializeField] private List<Message> messages;

    private MenuControllerJuR menu;
    private int curMsg;

    private void Start() {
        menu = MenuControllerJuR.Instance;
    }

    public void Interact() {
        if (curMsg >= messages.Count) return;
        menu.AddMessage(messages[curMsg]);
        curMsg++;
    }
}
