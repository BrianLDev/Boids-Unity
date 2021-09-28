using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
    public float speed = 5;
    private Boid[] neighbors;
    private Vector3 velocity, boundaryCenter;
    private float boundaryRadius = 10;
    private Quaternion targetRotation;
    private bool turningAround = false;

    private void Start() {
        velocity = Random.insideUnitSphere.normalized;
        transform.forward = velocity;
    }

    public void Initialize(Vector3 boundCenter, float boundRadius) {
        boundaryCenter = boundCenter;
        boundaryRadius = boundRadius;
    }
    
    private void Update() {
        Move();
        CheckBounds();
        TurningAround();
        // FindNeighbors();
        // Separation();
        // Alignment();
        // Cohesion;
    }

    private void Move() {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void CheckBounds() {
        if ((transform.position - boundaryCenter).magnitude > boundaryRadius * 0.9f) {
            turningAround = true;
            // targetRotation = Quaternion.Inverse(transform.rotation);
            targetRotation = Quaternion.LookRotation(boundaryCenter - transform.position + Random.insideUnitSphere * boundaryRadius);
        }
    }

    private void TurningAround() {
        if (turningAround) {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            // check if close enough to target since Slerp will approach but never reach it
            if (Quaternion.Dot(transform.rotation, targetRotation) > 0.99f) {
                turningAround = false;
            }
        }
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


}
