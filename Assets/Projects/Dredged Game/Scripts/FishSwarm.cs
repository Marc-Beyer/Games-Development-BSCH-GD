using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dredged {

    public class FishData {
        public string Name = "Normal Fish";
        public int Price = 5;
        public int Rarity = 5;
        public string Description;

        public FishData(string name, int rarity, string description) {
            Name = name;
            Rarity = rarity;
            Price = 500 / rarity;
            Description = description;
        }
    }

    public class FishSwarm : MonoBehaviour {

        public int pointNeededToCatch = 1000;

        public FishData CurrentfishData;

        [SerializeField] private Animator swarmAnimator;
        [SerializeField] private Transform followHolder;

        private bool isInRange = false;
        private bool isCatching = false;

        private UIController uiController;
        private CaptureController catchController;
        private PlayerStats playerStats;
        private Transform player;
        private SwarmController swarmController;

        private void Start() {
            uiController = UIController.inst;
            catchController = CaptureController.inst;
            playerStats = PlayerStats.inst;
            swarmController = SwarmController.inst;
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update() {
            if (isInRange && !isCatching && Input.GetButton("Jump")) {
                isCatching = true;
                uiController.HidePrompt();

                catchController.SetWinAngle(CurrentfishData.Rarity * 20);
                catchController.StartSpin((int points) => SpinCallback(points));
                swarmAnimator.SetInteger("state", 1);
                followHolder.position = new Vector3(player.position.x, followHolder.position.y, player.position.z);
            }
        }

        public void SpinCallback(int points) {
            isCatching = false;
            swarmAnimator.SetInteger("state", 0);
            followHolder.position = transform.position;
            if (points > pointNeededToCatch) {

                swarmController.RegenerateSwarm(this);

                if(playerStats.Fish + 1 > playerStats.maxFish) {
                    catchController.ShowPopupText("NO SPACE", Color.red, false);
                    return;
                }
                playerStats.FishDataArr.Add(CurrentfishData);

                if (
                    playerStats.Fish + 2 <= playerStats.maxFish &&
                    Random.Range(0, 20/CurrentfishData.Rarity * pointNeededToCatch) < points
                ) {
                    playerStats.FishDataArr.Add(CurrentfishData);
                    catchController.ShowPopupText("BONUS", Color.green, true);
                }

                playerStats.Fish = playerStats.FishDataArr.Count;
            } else if(isInRange) {
                uiController.ShowPrompt("The fish escaped!\n\nPress 'SPACE' to try again!");
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Player")) {
                isInRange = true;
                uiController.ShowPrompt("Press 'SPACE' to catch fish!");
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.CompareTag("Player")) {
                isInRange = false;
                uiController.HidePrompt();
            }
        }
    }
}
