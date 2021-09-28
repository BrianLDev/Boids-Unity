using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
    public float speed = 5;
    public float perceptionRange = 2;
    public static List<Boid> population;
    private List<Boid> neighbors;
    private Vector3 boundaryCenter;
    private float boundaryRadius = 10;
    private Quaternion targetRotation;

    private void Awake() {
        transform.forward = Random.insideUnitSphere.normalized * speed;
        if (population == null)
            population = new List<Boid>();
        population.Add(this);
    }

    public void Initialize(Vector3 boundCenter, float boundRadius) {
        boundaryCenter = boundCenter;
        boundaryRadius = boundRadius;
    }
    
    private void Update() {
        Move();
        CheckBounds();
        TurningAround();
        FindNeighbors(population);
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

    private void FindNeighbors(List<Boid> population) {
        if (neighbors == null)
            neighbors = new List<Boid>();
        else
            neighbors.Clear();

        foreach (Boid other in population) {
            if ((other.transform.position - transform.position).magnitude < perceptionRange) {
                neighbors.Add(other);
            }
        }
        Debug.Log(this.name + " found " + neighbors.Count + " neighbors.");
    }

    // private void Separation(Boid[] neighbors) {

    // }

    // private void Alignment(Boid[] neighbors) {
        
    // }

    // private void Cohesion(Boid[] neighbors) {
        
    // }


}
