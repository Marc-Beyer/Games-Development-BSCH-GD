using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
    [SerializeField] private Vector3 groundOffset = new Vector3(-15f, -15f, 0f);
    [SerializeField] private Transform player;

    [SerializeField] private int goal = 20;

    [SerializeField] private Transform groundPoolContainer;
    [SerializeField] private List<Ground> groundPool;

    private int currentPoolPosition = 0;
    private int progression = 0;

    private void Awake() {
        for (int i = 0; i < groundPoolContainer.childCount; i++) {
            Ground ground = groundPoolContainer.GetChild(i).GetComponent<Ground>();
            if(ground != null) groundPool.Add(ground);
        }
        for (int i = 0; i < groundPool.Count; i++) {
            groundPool[i].transform.position += i * groundOffset;
            groundPool[i].Randommize();
        }
    }

    private void FixedUpdate() {
        if (player.position.y < groundPool[currentPoolPosition].transform.position.y + groundOffset.y) {
            groundPool[currentPoolPosition].transform.position += groundOffset * groundPool.Count;
            groundPool[currentPoolPosition].Randommize();
            progression++;
            if (progression > goal - groundPool.Count + 1) {
                groundPool[currentPoolPosition].HasGoal();
            }
            if (progression > goal) {
                MenuController.instance.UpdateEnd();
            }
            currentPoolPosition++;
            currentPoolPosition %= groundPool.Count;
        }
    }
}
