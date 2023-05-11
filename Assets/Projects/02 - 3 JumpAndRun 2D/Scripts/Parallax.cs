using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    [SerializeField] private Transform target;
    [SerializeField] private Material material;

    private void Awake() {
        if(target == null) target = transform;
        material = GetComponent<Renderer>().material;
    }

    void Update() {
        material.SetTextureOffset("_MainTex", target.position);
    }
}
