using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private Transform followedTransform;
    [SerializeField] private Vector3 offset;

    void FixedUpdate() {
        transform.position = followedTransform.position + offset;
    }
}
