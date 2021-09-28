using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
    public float speed = 5;
    private Boid[] neighbors;
    private Vector3 velocity, boundaryCenter;
    private float boundaryRadius = 10;
    private Quaternion targetRotation;

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
            // targetRotation = Quaternion.Inverse(transform.rotation);
            targetRotation = Quaternion.LookRotation(boundaryCenter - transform.position + Random.onUnitSphere*boundaryRadius*0.5f);
        }
    }

    private void TurningAround() {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
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
