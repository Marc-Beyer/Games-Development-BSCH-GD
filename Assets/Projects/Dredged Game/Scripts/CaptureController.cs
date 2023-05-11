using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Dredged {

    public class CaptureController : MonoBehaviour {
        public static CaptureController inst;

        public float WinAngle = 45f;

        [SerializeField] private float startWheelSpeed = 1000;
        [SerializeField] private float wheelSpeedFriction = 10;

        [SerializeField] private int hitPoints = 100;
        [SerializeField] private int missPoints = 90;
        [SerializeField] private int bonusPoints = 500;


        [SerializeField] private GameObject ui;
        [SerializeField]private Transform wheelImg;
        [SerializeField]private Image winPartImg;
        [SerializeField] private TextMeshProUGUI countDownText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Transform canvas;
        [SerializeField] private GameObject popupTextObj;

        public int hits = 0;
        public int misses = 0;

        private float curWheelSpeed = 0;
        private bool isWheelSpinning = false;

        private System.Action<int> activeCallback;

        public int GearBonus = 0;

        private void Awake() {
            inst = this;
        }

        public void StartSpin(System.Action<int> callback) {
            activeCallback = callback;

            winPartImg.fillAmount = WinAngle / 360f;
            wheelImg.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));

            hits = 0;
            misses = 0;
            scoreText.text = "Press Space when the arrow is on the green space to get points.";

            int time = 3;
            countDownText.text = time.ToString();

            ui.SetActive(true);

            StartCoroutine(InvokeRoutine(() => countDown(time), 1f));
        }

        internal void SetWinAngle(int angle) {
            WinAngle = Mathf.Min(angle + GearBonus, 340);
        }

        private void Update() {
            spinWheel();
        }

        private void spinWheel() {
            if (!isWheelSpinning) return;

            wheelImg.eulerAngles += new Vector3(0, 0, curWheelSpeed * Time.deltaTime);
            curWheelSpeed -= wheelSpeedFriction * Time.deltaTime;

            checkForHits();

            if (curWheelSpeed <= 0) {
                int bonus = 0;
                if (wheelImg.eulerAngles.z > 0 && wheelImg.eulerAngles.z < WinAngle) {
                    bonus = bonusPoints;

                    ShowPopupText("+" + bonusPoints, Color.green);
                }

                int points = (bonus + hits * hitPoints - misses * missPoints);
                scoreText.text = "Score:\n-------------------\nHits:\t\t\t" + hits + "x +" + hitPoints + "\nMisses:\t\t" + misses + "x -" + missPoints;
                if(bonus > 0) {
                    scoreText.text += "\nBONUS:\t\t" + bonus;
                }
                scoreText.text += "\n-------------------\n" + points;

                isWheelSpinning = false;
                StartCoroutine(InvokeRoutine(() => endSpin(points), 3f));
                
            }
        }

        public void ShowPopupText(string str, Color textColor) {
            TextMeshProUGUI text = Instantiate(popupTextObj, canvas).GetComponent<TextMeshProUGUI>();
            text.text = str;
            text.color = textColor;
            StartCoroutine(InvokeRoutine(() => destroyObj(text.gameObject), 1f));
        }

        private void endSpin(int points) {
            scoreText.text = "";
            ui.SetActive(false);
            activeCallback(points);
        }

        private void checkForHits() {
            if (Input.GetButtonDown("Jump")) {
                if (wheelImg.eulerAngles.z > 0 && wheelImg.eulerAngles.z < WinAngle) {
                    hits++;
                    ShowPopupText("+" + hitPoints, Color.green);
                } else {
                    misses++;
                    ShowPopupText("-" + missPoints, Color.red);
                }
            }
        }

        private void countDown(int time) {
            time--;
            countDownText.text = time.ToString();
            
            if (time <= 0) {
                countDownText.text = "";
                isWheelSpinning = true;
                curWheelSpeed = startWheelSpeed;
            } else {
                StartCoroutine(InvokeRoutine(() => countDown(time), 1f));
            }
        }

        private void destroyObj(GameObject obj) {
            Destroy(obj);
        }

        private static IEnumerator InvokeRoutine(System.Action f, float delay) {
            yield return new WaitForSeconds(delay);
            f();
        }
    }
}
