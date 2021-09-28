using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEditor;
using UnityEngine;

public class Boid : MonoBehaviour {
    public float speed = 5;
    private Boid[] neighbors;
    private Vector3 direction, goalDirection;
    private bool turning = false;

    private void Start() {
        direction = Random.insideUnitSphere.normalized;
        transform.forward = direction;
    }
    
    private void Update() {
        Move();
        HandleTurning();
        // FindNeighbors();
        // Separation();
        // Alignment();
        // Cohesion;
    }

    private void Move() {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    // public static Boid[] FindNeighbors() {
    //     Boid[] neighbors;
    // }

    // private void Separation(Boid[] neighbors) {

    // }

    // private void Alignment(Boid[] neighbors) {
        
    // }

    // private void Cohesion(Boid[] neighbors) {
        
    // }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bounds"))
            TurnAround();
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
