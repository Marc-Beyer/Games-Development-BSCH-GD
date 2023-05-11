using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dredged {
    public class Compass : MonoBehaviour {

        [SerializeField] private Transform player;
        [SerializeField] private Transform needle;

        void FixedUpdate() {
            needle.eulerAngles = new Vector3(0, 0, -player.eulerAngles.y);
        }
    }
}
