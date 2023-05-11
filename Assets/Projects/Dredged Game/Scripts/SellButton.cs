using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Dredged {
    public class SellButton : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image img;

        private FishData fishData;
        private Shop shop;


        internal void SetFishData(FishData fish, Sprite sprite, Color color, Shop shop) {
            fishData = fish;
            this.shop = shop;
            text.text = fish.Name + " $" + fish.Price;
            img.sprite = sprite;
            img.color = color;
        }

        public void SellFush() {
            shop.SellFish(fishData);
        }
    }
}
