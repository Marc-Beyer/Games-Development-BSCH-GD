using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RPSType {
    ROCK,
    PAPER,
    SCISSORS,
    WELL
}

public class RPSCharacter : MonoBehaviour {

    public bool IsEnemy;
    public RPSType CharacterType;

    [SerializeField] private float speed;

    [SerializeField] private Color playerColor;
    [SerializeField] private Color enemyColor;

    [SerializeField] private SpriteRenderer rockGameobject;
    [SerializeField] private SpriteRenderer paperGameobject;
    [SerializeField] private SpriteRenderer scissorsGameobject;
    [SerializeField] private SpriteRenderer wellGameobject;
    [SerializeField] private SpriteRenderer bloodGameobject;

    private Transform wayPoint;
    private Rigidbody2D rgBody;
    private bool wasKilled = false;

    void Start() {
        wayPoint = GameObject.FindGameObjectWithTag("WayPoint").GetComponent<Transform>();
        rgBody = GetComponent<Rigidbody2D>();

        GetComponent<SpriteRenderer>().color = IsEnemy ? enemyColor : playerColor;

        switch (CharacterType) {
            case RPSType.ROCK:
                rockGameobject.gameObject.SetActive(true);
                gameObject.tag = (IsEnemy ? "Enemy" : "Player") + "Rock";
                break;
            case RPSType.PAPER:
                paperGameobject.gameObject.SetActive(true);
                gameObject.tag = (IsEnemy ? "Enemy" : "Player") + "Paper";
                break;
            case RPSType.SCISSORS:
                scissorsGameobject.gameObject.SetActive(true);
                gameObject.tag = (IsEnemy ? "Enemy" : "Player") + "Scissors";
                break;
            case RPSType.WELL:
                wellGameobject.gameObject.SetActive(true);
                gameObject.tag = (IsEnemy ? "Enemy" : "Player") + "Well";
                break;
        }
    }

    private void FixedUpdate() {
        if (wasKilled) return;

        rgBody.velocity = (wayPoint.position - transform.position).normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (IsEnemy) {
            switch (CharacterType) {
                case RPSType.ROCK:
                    if (collision.gameObject.CompareTag("PlayerRock")) kill();
                    if (collision.gameObject.CompareTag("PlayerPaper")) kill();
                    if (collision.gameObject.CompareTag("PlayerWell")) kill();
                    break;
                case RPSType.PAPER:
                    if (collision.gameObject.CompareTag("PlayerPaper")) kill();
                    if (collision.gameObject.CompareTag("PlayerScissors")) kill();
                    break;
                case RPSType.SCISSORS:
                    if (collision.gameObject.CompareTag("PlayerScissors")) kill();
                    if (collision.gameObject.CompareTag("PlayerRock")) kill();
                    if (collision.gameObject.CompareTag("PlayerWell")) kill();
                    break;
                case RPSType.WELL:
                    if (collision.gameObject.CompareTag("PlayerPaper")) kill();
                    if (collision.gameObject.CompareTag("PlayerWell")) kill();
                    break;
            }
        } else {
            switch (CharacterType) {
                case RPSType.ROCK:
                    if (collision.gameObject.CompareTag("EnemyRock")) kill();
                    if (collision.gameObject.CompareTag("EnemyPaper")) kill();
                    if (collision.gameObject.CompareTag("EnemyWell")) kill();
                    break;
                case RPSType.PAPER:
                    if (collision.gameObject.CompareTag("EnemyPaper")) kill();
                    if (collision.gameObject.CompareTag("EnemyScissors")) kill();
                    break;
                case RPSType.SCISSORS:
                    if (collision.gameObject.CompareTag("EnemyScissors")) kill();
                    if (collision.gameObject.CompareTag("EnemyRock")) kill();
                    if (collision.gameObject.CompareTag("EnemyWell")) kill();
                    break;
                case RPSType.WELL:
                    if (collision.gameObject.CompareTag("EnemyPaper")) kill();
                    if (collision.gameObject.CompareTag("EnemyWell")) kill();
                    break;
            }
        }
    }

    private void kill() {
        if (wasKilled) return;
        wasKilled = true;

        GetComponent<SpriteRenderer>().color =new Color(1,1,1,0);

        Destroy(GetComponent<Rigidbody2D>());
        rgBody.velocity = Vector2.zero;

        GetComponent<Collider2D>().enabled = false;

        GetComponent<SpriteRenderer>().sortingOrder = 90;
        rockGameobject.sortingOrder = 91;
        paperGameobject.sortingOrder = 91;
        scissorsGameobject.sortingOrder = 91;
        wellGameobject.sortingOrder = 91;

        bloodGameobject.gameObject.SetActive(true);

        RockPaperScissorsController.inst.Killed(IsEnemy);
        //Destroy(gameObject);
    }
}
