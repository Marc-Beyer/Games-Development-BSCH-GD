using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class TopDownPlayerController : MonoBehaviour {

    [SerializeField] private float speed = 25f;
    [SerializeField] private float zoomSpeed = 25f;
    [SerializeField] private float turnSpeed = 25f;

    [SerializeField] private Camera cam;
    public NavMeshAgent Player;
    [SerializeField] private Transform camContainer;

    public static TopDownPlayerController Instance;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        Vector3 nextPosition = new Vector3(-Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));

        camContainer.transform.position += camContainer.transform.TransformDirection(nextPosition * speed * Time.deltaTime);

        float zoom = Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        cam.fieldOfView -= zoom;

        if (Input.GetKey(KeyCode.E)) {
            camContainer.transform.eulerAngles += new Vector3(0, turnSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.Q)) {
            camContainer.transform.eulerAngles += new Vector3(0, -turnSpeed * Time.deltaTime, 0);
        }

        if (Input.GetMouseButtonDown(1)) {
            Ray pos = cam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(pos, out var hit)) {
                Player.SetDestination(hit.point);
            }
        }
    }
}
