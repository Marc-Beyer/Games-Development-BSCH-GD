using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

    [SerializeField] public GameObject point;
    [SerializeField] public float pointProbability = 10;
    [SerializeField] public float pointOffset = 2;

    [SerializeField] public GameObject damage;
    [SerializeField] public float damageProbability = 10;
    [SerializeField] public float damageOffset = 2;

    [SerializeField] public GameObject goal;

    public void Randommize() {
        point.SetActive(Random.Range(0, 100) < pointProbability);
        point.transform.position = new Vector3(point.transform.position.x, point.transform.position.y, Random.Range(-pointOffset, pointOffset));

        damage.SetActive(Random.Range(0, 100) < damageProbability);
        damage.transform.position = new Vector3(damage.transform.position.x, damage.transform.position.y, Random.Range(-damageOffset, damageOffset));
    }

    public void HasGoal() {
        goal.SetActive(true);
        damage.SetActive(false);
        point.SetActive(false);
    }

    public void FellOff() {
        MenuController.instance.UpdateEnd();
    }
}

