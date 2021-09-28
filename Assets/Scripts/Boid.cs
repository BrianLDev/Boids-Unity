using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Boid : MonoBehaviour {
    public float speed = 5;
    public Transform boundaryBox;
    private Vector3 direction;
    private bool turning = false;

    private void Start() {
        direction = Random.insideUnitSphere.normalized;
        transform.forward = direction;
    }
    
    private void Update() {
        transform.position += transform.forward * speed * Time.deltaTime;
        HandleTurning();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bounds")) {
            TurnAround();

        }
    }

    private void TurnAround() {
        // TODO: WRITE THIS
    }

    private void HandleTurning() {
        if (turning) {
            // TODO: WRITE THIS
        }
    }
}
