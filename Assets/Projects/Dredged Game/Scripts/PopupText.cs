using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dredged {
    public class PopupText : MonoBehaviour {
        public GameObject hitSoundObj;
        public GameObject missSoundObj;

        public void SetSound(bool isHit) {
            if (isHit) {
                hitSoundObj.SetActive(true);
            } else {
                missSoundObj.SetActive(true);
            }
        }
    }
}
