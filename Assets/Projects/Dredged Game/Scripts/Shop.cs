using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dredged {
    public class Shop : MonoBehaviour {
        public static Shop inst;

        [SerializeField] private int refillPrice = 50;
        [SerializeField] private int maxGasPrice = 200;
        [SerializeField] private int maxFishPrice = 200;
        [SerializeField] private int betterGearPrice = 400;
        [SerializeField] private int lightPrice = 900;
        [SerializeField] private int trophyPrice = 9000;

        [SerializeField] private float addSpotLight;
        [SerializeField] private Light spotLight;

        [SerializeField] private GameObject shopUi;
        [SerializeField] private GameObject buttonObj;
        [SerializeField] private Transform sellContainer;

        [SerializeField] private BoatController player;
        [SerializeField] private Kraken kraken;
        [SerializeField] private UIController uiController;

        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Color[] colors;

        [SerializeField] private AudioSource shopMusic;
        [SerializeField] private AudioSource seaMusic;

        private bool isInShopArea = false;
        private PlayerStats playerStats;

        private List<SellButton> sellBtnList = new List<SellButton>();

        private void Start() {
            playerStats = PlayerStats.inst;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void CloseShop() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            MainMenuController.inst.CanEscape = true;

            if (isInShopArea) {
                uiController.ShowPrompt("Press 'ENTER' to go to the SHOP!");
            }

            shopUi.SetActive(false);
        }

        public void AddLight() {
            if(playerStats.Money > lightPrice) {
                playerStats.Money -= lightPrice;
                spotLight.spotAngle = spotLight.spotAngle + addSpotLight;
            }
        }

        public void RefillGas() {
            if (playerStats.Money > refillPrice) {
                playerStats.Money -= refillPrice;
                playerStats.Gas = playerStats.maxGas;
            }
        }
        public void AddMaxGas() {
            if (playerStats.Money > maxGasPrice) {
                playerStats.Money -= maxGasPrice;
                playerStats.maxGas = playerStats.maxGas + 5;
                playerStats.Gas = playerStats.Gas;
            }
        }

        public void AddMaxFish() {
            if (playerStats.Money > maxFishPrice) {
                playerStats.Money -= maxFishPrice;
                playerStats.maxFish = playerStats.maxFish + 1;
                playerStats.Fish = playerStats.Fish;
            }
        }

        public void BetterGear() {
            if (playerStats.Money > betterGearPrice) {
                playerStats.Money -= betterGearPrice;
                CaptureController.inst.GearBonus += 45;
            }
        }

        public void Trophy() {
            if (playerStats.Money > trophyPrice) {
                playerStats.Money -= trophyPrice;
                playerStats.maxFish = playerStats.maxFish + 1;
                playerStats.Fish = playerStats.Fish;
            }
        }

        public void SellFish(FishData fish) {
            playerStats.Money += fish.Price;
            playerStats.FishDataArr.Remove(fish);
            playerStats.Fish = playerStats.FishDataArr.Count;

            clearSellBtns();
            addSellBtns();
        }

        private void clearSellBtns() {
            foreach (SellButton btn in sellBtnList) {
                Destroy(btn.gameObject);
            }
            sellBtnList.Clear();
        }

        private void addSellBtns() {
            foreach (FishData fish in playerStats.FishDataArr) {
                SellButton sellButton = Instantiate(buttonObj, sellContainer).GetComponent<SellButton>();
                sellButton.SetFishData(fish, sprites[Random.Range(0, sprites.Length)], colors[Random.Range(0, colors.Length)], this);
                sellBtnList.Add(sellButton);
            }
        }

        private void Update() {
            if (isInShopArea && Input.GetButtonDown("Submit")) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                uiController.HidePrompt();
                shopUi.SetActive(true);

                clearSellBtns();
                addSellBtns();
                MainMenuController.inst.CanEscape = false;
            } 
            
            if (shopUi.activeInHierarchy && Input.GetButtonDown("Cancel")) {
                CloseShop();
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                shopMusic.Play();
                seaMusic.Stop();

                player.IsInShop = true;
                isInShopArea = true;
                uiController.ShowPrompt("Press 'ENTER' to go to the SHOP!");
                kraken.OnDisapear();
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) {
                shopMusic.Stop();
                seaMusic.Play();

                if(MainMenuController.inst) MainMenuController.inst.CanEscape = true;

                uiController.HidePrompt();
                player.IsInShop = false;
                isInShopArea = false;
                shopUi.SetActive(false);
            }
        }
    }
}

