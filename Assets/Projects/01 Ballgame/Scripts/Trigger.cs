using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour {
    public UnityEvent triggerEvent;

    [SerializeField] private bool disableAfterTrigger = true;

    private void Awake() {
        if (triggerEvent == null) triggerEvent = new UnityEvent();
    }

    void OnTriggerEnter(Collider other) {
        trigger();
    }

    void OnTriggerEnter2D() {
        trigger();
    }

    private void trigger() {
        triggerEvent.Invoke();
        if (disableAfterTrigger) gameObject.SetActive(false);
    }
}
