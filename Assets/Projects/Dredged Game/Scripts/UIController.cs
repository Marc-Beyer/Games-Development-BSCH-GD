using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Dredged {
    public class UIController : MonoBehaviour {
        public static UIController inst;

        [SerializeField] private TextMeshProUGUI promptText;

        private void Awake() {
            inst = this;
        }

        public void ShowPrompt(string text) {
            promptText.text = text;
        }
        public void HidePrompt() {
            promptText.text = "";
        }
    }
}
