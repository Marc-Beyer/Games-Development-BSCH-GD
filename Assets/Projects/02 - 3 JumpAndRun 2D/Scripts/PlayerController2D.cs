using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour {
    public static PlayerController2D Instance;

    [SerializeField] private int _maxHearts = 2;

    [SerializeField] private float jumpForce;
    [SerializeField] private int _jumpPower = 2;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float movmentSpeed;
    [Range(0, .3f)][SerializeField] private float smoothTime = 0.05f;

    [SerializeField] private Transform spriteTrans;
    [SerializeField] private Animator spriteAnimator;
    [SerializeField] private ParticleSystem walkingParticleSystem;
    [SerializeField] private ParticleSystem jumpingParticleSystem;

    private bool isGrounded = false;
    private int currentJumpPower;
    private int _currentHearts;
    private Vector3 currentVelocity;

    private Rigidbody2D rgbody;

    public UnityEvent heartsChanged;
    public UnityEvent maxHeartsChanged;

    public int JumpPower { get => _jumpPower; set => _jumpPower = value; }
    public int CurrentHearts {
        get => _currentHearts;
        set {
            _currentHearts = value;
            heartsChanged.Invoke();
        }
    }

    public int MaxHearts {
        get => _maxHearts;
        set {
            _maxHearts = value;
            maxHeartsChanged.Invoke();
        }
    }

    public void AddMaxHearts(int hearts) {
        MaxHearts += hearts;
    }

    public void AddJumpPower(int power) {
        JumpPower += power;
    }

    void Awake() {
        Instance = this;
        rgbody = GetComponent<Rigidbody2D>();

        if (heartsChanged == null) heartsChanged = new UnityEvent();
        if (maxHeartsChanged == null) maxHeartsChanged = new UnityEvent();


    }

    private void Start() {
        // Using the encapsulated variable, because the event that is invoked when changing CurrentHearts is not setup on Awake
        CurrentHearts = MaxHearts;
    }

    private void FixedUpdate() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 targetVelocity = new Vector2(
            Mathf.Clamp(rgbody.velocity.x + horizontal * movmentSpeed, -maxSpeed, maxSpeed),
            rgbody.velocity.y
        );

        if (horizontal == 0) {
            targetVelocity.x = 0;
            spriteAnimator.SetInteger("state", isGrounded ? 0 : targetVelocity.y < 0 ? 3 : 4);
            walkingParticleSystem.Stop();
        } else {
            spriteTrans.localScale = new Vector3((horizontal < 0) ? -1 : 1, 1, 1);
            spriteAnimator.SetInteger("state", isGrounded ? 1 : 2);
            if (isGrounded) {
                if (!walkingParticleSystem.isPlaying) walkingParticleSystem.Play();
            } else {
                walkingParticleSystem.Stop();
            }
        }

        rgbody.velocity = Vector3.SmoothDamp(rgbody.velocity, targetVelocity, ref currentVelocity, smoothTime);


        if (transform.position.y < -10) {
            OpenMainMenu();
        }
    }

    void Update() {
        if (currentJumpPower > 0 && Input.GetButtonDown("Jump")) {
            rgbody.velocity = new Vector2(rgbody.velocity.x, 0);
            rgbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            currentJumpPower--;
            //jumpingParticleSystem.Clear();
            //jumpingParticleSystem.Play();
        }

        if (Input.GetButtonDown("Fire1")) {
            spriteAnimator.SetTrigger("attack_side");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer != 8 || other.isTrigger) return;
        isGrounded = false;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.layer != 8 || other.isTrigger) return;
        isGrounded = true;
        currentJumpPower = JumpPower;
    }
    public void OpenMainMenu() {
        if (MainMenuController.inst == null) {
            SceneManager.LoadScene(0);
        } else {
            MainMenuController.inst.OpenMainMenu();
        }
    }
}
