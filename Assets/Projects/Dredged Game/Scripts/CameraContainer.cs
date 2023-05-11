using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dredged {

    public class CameraContainer : MonoBehaviour {

        [SerializeField] private Transform followObj;

        void Start() {

        }

        void Update() {
            transform.position = followObj.position;
        }
    }
}
