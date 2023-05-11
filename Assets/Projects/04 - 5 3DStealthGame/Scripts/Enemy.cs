using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum EnemyState {
    IDLE,
    IDLE_WALING,
    STUNNED,
    SUSPICIOUS,
    ALERTED
}

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour {
    public List<Transform> path;

    private int curPathPoint = 0;

    [SerializeField] private float hearingDistance = 2;
    [SerializeField] private float movingHearingDistance = 10;

    [SerializeField] private float goalWaitTimeMin = 2;
    [SerializeField] private float goalWaitTimeMax = 10;

    [SerializeField] private float stunnedTime;
    private bool stunned;

    private NavMeshAgent agent;
    private NavMeshAgent playerAgent;
    private Transform player;

    private EnemyState _state = EnemyState.IDLE_WALING;

    public EnemyState State {
        get => _state;
        set => _state = value;
    }

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = path[curPathPoint].position;

        playerAgent = TopDownPlayerController.Instance.Player;
        player = playerAgent.gameObject.transform;
    }

    void FixedUpdate() {
        if (agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude == 0f) {
            if (State == EnemyState.IDLE_WALING) {
                StartCoroutine("NextGoal");
            }
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < hearingDistance) {
            StartCoroutine(NextGoal());
        } else if (distanceToPlayer < movingHearingDistance && playerAgent.velocity.sqrMagnitude != 0) {
            StartCoroutine(HeardPlayer(player.position));
        }

    }
    IEnumerator HeardPlayer(Vector3 point) {
        switch (State) {
            case EnemyState.IDLE:
            case EnemyState.IDLE_WALING:
                State = EnemyState.SUSPICIOUS;
                break;
            case EnemyState.STUNNED:
                break;
            case EnemyState.SUSPICIOUS:
            case EnemyState.ALERTED:
                agent.SetDestination(point);
                yield return null;
                break;
            default:
                break;

        }
        agent.SetDestination(transform.position);

        yield return new WaitForSeconds(1);
        agent.SetDestination(point);
    }

    IEnumerator NextGoal() {
        State = EnemyState.IDLE;
        curPathPoint = (curPathPoint + 1) % path.Count;
        yield return new WaitForSeconds(Random.Range(goalWaitTimeMin, goalWaitTimeMax));
        agent.destination = path[curPathPoint].position;
        State = EnemyState.IDLE_WALING;
    }

    IEnumerator Stunned() {
        print("Stunning");
        agent.destination = transform.position;
        stunned = true;
        yield return new WaitForSeconds(stunnedTime);
        stunned = false;
    }
}
