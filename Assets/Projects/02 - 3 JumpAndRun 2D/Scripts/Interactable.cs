using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour {
    [SerializeField] private GameObject keyGameObject;

    private bool canInteract = false;

    public UnityEvent InteractEvent;

    private void Awake() {
        if(InteractEvent == null) InteractEvent = new UnityEvent();
    }

    private void Update() {
        if(canInteract && Input.GetKey(KeyCode.E)) {
            InteractEvent.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        canInteract = true;
        if(keyGameObject != null) keyGameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        canInteract = false;
        if (keyGameObject != null) keyGameObject.SetActive(false);
    }
}
