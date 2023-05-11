using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

public class GameController : MonoBehaviour {

    public static GameController instance;

    [SerializeField] private int score = 0;

    public UnityEvent scoreChangedEvent;

    public int Score {
        get => score;
        set {
            score = value;
            scoreChangedEvent.Invoke();
            if (score < 0) {
                MenuController.instance.UpdateEnd();
            }
        }
    }

    public void AddToScore(int points) {
        Score += points;
    }

    private void Awake() {
        if (instance == null) instance = this;
        if (scoreChangedEvent == null) scoreChangedEvent = new UnityEvent();
    }
}
