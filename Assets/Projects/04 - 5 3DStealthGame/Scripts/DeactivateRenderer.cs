using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateRenderer : MonoBehaviour{
    void Awake() {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
