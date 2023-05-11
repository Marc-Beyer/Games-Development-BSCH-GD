using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace Dredged {
    public enum KrakenState {
        IDLE,
        IN_RANGE,
        ATTACKING,
        DISAPEAR
    }

    public class Kraken : MonoBehaviour {

        [SerializeField] private float attentionRadius;
        [SerializeField] private float attackRadius;

        [SerializeField] private Transform[] wayPoints;
        [SerializeField] private Transform player;

        [SerializeField] private Animator hitImageAnimator;
        [SerializeField] private Animator krakenAnimator;

        private NavMeshAgent agent;
        [SerializeField] private KrakenState krakenState = KrakenState.IDLE;

        void Start() {
            agent = GetComponent<NavMeshAgent>();
            agent.destination = wayPoints[Random.Range(0, wayPoints.Length)].position;

            OnIdleState();
        }

        void FixedUpdate() {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            switch (krakenState) {
                case KrakenState.IDLE:
                    idleState(distanceToPlayer);
                    break;
                case KrakenState.IN_RANGE:
                    inRangeState(distanceToPlayer);
                    break;
                case KrakenState.ATTACKING:
                    attackState(distanceToPlayer);
                    break;
            }
        }

        // IDLE

        public void OnIdleState() {
            agent.destination = transform.position;
            krakenState = KrakenState.IDLE;
        }

        private void idleState(float distanceToPlayer) {
            if (agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude == 0f) {
                agent.destination = wayPoints[Random.Range(0, wayPoints.Length)].position;
            }
            if (distanceToPlayer < attentionRadius) {
                OnInRangeState();
            }
        }

        // IN_RANGE

        public void OnInRangeState() {
            krakenState = KrakenState.IN_RANGE;
        }

        private void inRangeState(float distanceToPlayer) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, player.position - transform.position, out hit, Mathf.Infinity)) {
                Debug.Log("SET DESTINATION TO PLAYER");
                agent.destination = player.position;
            } else {
                idleState(distanceToPlayer);
            }

            if (distanceToPlayer < attackRadius) {
                OnAttackState();
            }else if (distanceToPlayer > attentionRadius) {
                OnIdleState();
            }
        }

        // ATTACKING

        public void OnAttackState() {
            krakenState = KrakenState.ATTACKING;
            hitImageAnimator.SetTrigger("Hit");
            krakenAnimator.SetTrigger("Attack");
        }

        private void attackState(float distanceToPlayer) {
            if (distanceToPlayer > attackRadius) {
                OnInRangeState();
            } else {
                hitImageAnimator.SetTrigger("Hit");
                krakenAnimator.SetTrigger("Attack");
            }
        }

        // DISAPEAR

        public void OnDisapear() {
            agent.destination = wayPoints[Random.Range(0, wayPoints.Length)].position;
        }

        private void disapearState(float distanceToPlayer) {
            if (agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude == 0f) {
                OnIdleState();
            }
        }
    }
}
