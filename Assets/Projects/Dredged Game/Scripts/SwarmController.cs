using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dredged {
    public class SwarmController : MonoBehaviour {
        public static SwarmController inst;

        [SerializeField] private float resetDistance;
        [SerializeField] private float spawnDistanceMin;
        [SerializeField] private float spawnDistanceMax;
        [SerializeField] private float spawnAngle = 45f;

        [SerializeField] private float updateRate = 5;

        [SerializeField] private List<FishSwarm> fishSwarms;
        [SerializeField] private Transform player;
        [SerializeField] private Transform playerSwarmSpawner;

        private FishData[] fishDatas = {
            new FishData("Coelacanth", 1, "A deep-sea fish with an ancient lineage."),
            new FishData("Devil's Hole Pupfish", 2, "An endangered species found only in one location in the world."),
            new FishData("Megamouth Shark", 3, "A rare and mysterious shark species."),
            new FishData("Atlantic Bluefin Tuna", 4, "A popular game fish that is heavily overfished."),
            new FishData("Siamese Fighting Fish", 5, "A colorful and aggressive freshwater fish."),
            new FishData("Paddlefish", 6, "A large, primitive fish with a long paddle-like snout."),
            new FishData("Wels Catfish", 7, "A large freshwater fish found in Europe and Asia."),
            new FishData("Mandarin Fish", 8, "A brightly colored fish native to China and Japan."),
            new FishData("Rainbow Trout", 9, "A popular game fish that is widely stocked in freshwater lakes and streams."),
            new FishData("Tuna", 10, "A highly migratory and commercially valuable fish.")
        };
        private List<FishData> fishDataList = new List<FishData>();

        private void Awake() {
            inst = this;

            foreach (FishData fish in fishDatas) {
                for (int i = 0; i < fish.Rarity; i++) {
                    fishDataList.Add(fish);
                }
            }
            foreach (FishSwarm fishSwarm in fishSwarms) {
                fishSwarm.CurrentfishData = fishDataList[Random.Range(0, fishDataList.Count)];
            }
        }

        private void Start() {
            InvokeRepeating("updateSwarms", 0, updateRate);
        }

        public void RegenerateSwarm(FishSwarm fishSwarm) {
            float rotaion = player.transform.eulerAngles.y;
            playerSwarmSpawner.eulerAngles = new Vector3(0, Random.Range(rotaion - spawnAngle / 2, rotaion + spawnAngle / 2), 0);
            fishSwarm.transform.position = playerSwarmSpawner.transform.position + playerSwarmSpawner.transform.TransformDirection(
                new Vector3(0, 0, Random.Range(spawnDistanceMin, spawnDistanceMax))
            );
            fishSwarm.CurrentfishData = fishDataList[Random.Range(0, fishDataList.Count)];
        }

        private void updateSwarms() {
            foreach (FishSwarm fishSwarm in fishSwarms) {
                if (Vector3.Distance(player.position, fishSwarm.transform.position) > resetDistance) {
                    RegenerateSwarm(fishSwarm);
                }
            }
        }

        private void generateFishData() {
            
        }

    }
}
