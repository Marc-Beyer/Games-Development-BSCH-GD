using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dredged {

    [RequireComponent(typeof(Rigidbody))]
    public class Fish : MonoBehaviour {

        [SerializeField] private Transform follow;

        [SerializeField] private float maxSpeed;
        [SerializeField] private float minSpeed;

        [SerializeField] private float speed;

        private Rigidbody rgBody;

        private void Awake() {
            rgBody = GetComponent<Rigidbody>();
            speed = Random.Range(minSpeed, maxSpeed);
        }

        void FixedUpdate() {
            rgBody.velocity = (follow.position - transform.position).normalized * speed;
            transform.LookAt(follow);
        }
    }
}
