using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ballgame {
    public class PlayerController : MonoBehaviour {

        [SerializeField] private float speed;
        [SerializeField] private float maxSpeed = 1;

        private Rigidbody rgBody;

        private void Awake() {
            rgBody = GetComponent<Rigidbody>();
        }

        void Update() {
            rgBody.AddForce(new Vector3(0, 0, Input.GetAxis("Horizontal") * speed));
        }

        private void FixedUpdate() {
            rgBody.velocity = Vector3.ClampMagnitude(rgBody.velocity, maxSpeed);
        }
    }

}