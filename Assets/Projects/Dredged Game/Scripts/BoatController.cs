using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dredged { 

    [RequireComponent(typeof(Rigidbody))]
    public class BoatController : MonoBehaviour {

        [SerializeField] private float shopRange = 6f;

        [SerializeField] private float speed = 1f;
        [SerializeField] private float maxSpeed = 1f;
        
        [SerializeField] private float rotaionSpeed = 1f;

        [SerializeField] private float gasDrain = .1f;

        [SerializeField] private ParticleSystem[] waterParticles;
        [SerializeField] private Camera cam;
        [SerializeField] private PlayerStats playerStats;

        [SerializeField] private AudioSource audio;

        private Rigidbody rgBody;

        public bool IsInShop = false;

        private void Awake() {
            rgBody = GetComponent<Rigidbody>();
        }

        void FixedUpdate() {
            float force = Input.GetAxis("Vertical");
            float rotaion = Input.GetAxis("Horizontal");

            if (force < -0.01f) rotaion = -rotaion;

            if(playerStats.Gas > 0) {

                if (force < -0.01f || force > 0.01f) {
                    playerStats.Gas -= gasDrain;
                    if (!audio.isPlaying) { 
                        audio.Play(); 
                    }
                } else if (audio.isPlaying) {
                    audio.Stop();
                }

                Vector3 addedForce = transform.transform.TransformDirection(new Vector3(0, 0, force));

                rgBody.AddForce(addedForce * speed);

                rgBody.velocity = Vector3.ClampMagnitude(rgBody.velocity, maxSpeed);

                toggleWaterParticles(rgBody.velocity.magnitude);
            }

            transform.eulerAngles += new Vector3(0, rotaion * rotaionSpeed * rgBody.velocity.magnitude, 0);
        }

        private void toggleWaterParticles(float velocity) {
            if (velocity > 0.1f) {
                if (waterParticles[0].isPlaying) return;
                foreach (ParticleSystem waterParticle in waterParticles) {
                    waterParticle.Play();
                }
            } else {
                if (!waterParticles[0].isPlaying) return;
                foreach (ParticleSystem waterParticle in waterParticles) {
                    waterParticle.Stop();
                }
            }
        }
    }
}
