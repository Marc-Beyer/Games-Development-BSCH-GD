using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {
    [SerializeField] private Transform followedTransform;
    [SerializeField] private Vector3 offset;

    [SerializeField] private bool x = true;
    [SerializeField] private bool y = true;
    [SerializeField] private bool z = true;

    void FixedUpdate() {
        transform.position = new Vector3(
            x ? followedTransform.position.x + offset.x : transform.position.x,
            y ? followedTransform.position.y + offset.y : transform.position.y,
            z ? followedTransform.position.z + offset.z : transform.position.z
            );
    }
}
