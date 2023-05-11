using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Dredged {
    public class PlayerStats : MonoBehaviour {
        public static PlayerStats inst;

        public List<FishData> FishDataArr = new List<FishData>();

        private int fish = 0;
        private float gas = 20;
        private int money = 4;

        public int maxFish = 4;
        public float maxGas = 20;

        [SerializeField] private Animator transitionAnimator;

        [SerializeField] private BoatController player;

        [SerializeField] private TextMeshProUGUI fishText;
        [SerializeField] private TextMeshProUGUI gasText;
        [SerializeField] private TextMeshProUGUI moneyText;

        public int Fish {
            get => fish;
            set {
                fish = value;
                fishText.text = fish + "/" + maxFish;
            }
        }
        public float Gas {
            get => gas;
            set {
                gas = value;
                gasText.text = (float)((int)(gas * 10)) / 10f + "/" + maxGas;

                if (gas <= 0 && !player.IsInShop) {
                    transitionAnimator.SetInteger("State", 0);
                    Invoke("resetPlayer", 1);
                }
            }
        }
        public int Money {
            get => money;
            set {
                money = value;
                moneyText.text = money + "";
            }
        }

        private void Awake() {
            inst = this;
        }

        private void Start() {
            Gas = maxGas;
            Fish = 0;
            Money = 0;
        }

        public void Hit() {
            Gas -= 2;
        }

        private void resetPlayer() {
            Gas = maxGas;
            Fish = 0;
            FishDataArr.Clear();
            player.transform.position = new Vector3(0, 1.5f, 0);
            player.transform.eulerAngles = Vector3.zero;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Invoke("showScreen", 1f);
        }

        private void showScreen() {
            transitionAnimator.SetInteger("State", 1);
        }
    }
}
