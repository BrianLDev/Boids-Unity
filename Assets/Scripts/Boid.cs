using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
    public static List<Boid> population;

    public BoidSettings boidSettings;
    private Vector3 boundaryCenter;
    private float boundaryRadius = 10;
    private Quaternion targetRotation;
    private Vector3 velocity, acceleration, separationForce, alignmentForce, cohesionForce;
    // variables for FindNeighbors made global to minimize garbage collection
    private Dictionary<Boid, (Vector3, float)> neighbors;  // stores Boid as dictionary key and value = a tuple of Vector3 vectorBetween, float of squared distance for fast lookup
    private Vector3 vectorBetween;
    private float sqrPerceptionRange, sqrMagnitudeTemp; 


    private void Awake() {
        if (population == null)
            population = new List<Boid>();
        population.Add(this);
        Initialize();
    }

    public void Initialize() {
        transform.forward = Random.insideUnitSphere.normalized;
        velocity = transform.forward * boidSettings.speed;
        acceleration = new Vector3(0,0,0);
    }

    public void SetBoundarySphere(Vector3 center, float radius) {
        boundaryCenter = center;
        boundaryRadius = radius;
    }

    private void FixedUpdate() {
        Flocking(); // handles Separation, Alignment, Cohesion forces
        Move(); // handles movement (shocker!) except for boundary turning which is handled below
        TurnAtBounds(); // makes sure boids turn back when they reach bounds
        ResetForces(); // set all forces to 0 since intertial bodies continue at same speed unless a force acts on them
    }

    private void ApplyForce(Vector3 force) {
        force /= boidSettings.mass;
        acceleration += force;
    }

    private void ResetForces() {
        acceleration = separationForce = alignmentForce = cohesionForce = Vector3.zero;
    }

    private void Move() {
        velocity = Vector3.zero;    // reset velocity
        if (boidSettings.moveFwd)
            velocity = transform.forward * boidSettings.speed;  // add forward movement
        velocity += acceleration * boidSettings.speed;  // add acceleration
        // Debug.Log("Velocity = " + velocity);
        transform.position += velocity * Time.fixedDeltaTime;   // move position
    }

    private void TurnAtBounds() {
        if (!boidSettings.boundsOn) {
            return;
        }
        else if ((transform.position - boundaryCenter).sqrMagnitude > (boundaryRadius * boundaryRadius) * 0.9f) {
            // targetRotation = Quaternion.Inverse(transform.rotation);
            targetRotation = Quaternion.LookRotation(boundaryCenter - transform.position + Random.onUnitSphere*boundaryRadius*0.5f);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * boidSettings.speed);
    }

    private void Flocking() {
        FindNeighbors();
        Separation();
        Alignment();
        Cohesion();
    }

    private void FindNeighbors() {
        if (neighbors == null)
            neighbors = new Dictionary<Boid, (Vector3, float)>();
        else
            neighbors.Clear();

        // sqrMagnitude is a bit faster than Magnitude since it doesn't require sqrt function
        sqrPerceptionRange = boidSettings.perceptionRange * boidSettings.perceptionRange;
        vectorBetween = Vector3.zero;
        sqrMagnitudeTemp = 0f;
        foreach (Boid other in population) {
            vectorBetween = other.transform.position - transform.position;
            sqrMagnitudeTemp = vectorBetween.sqrMagnitude;
            if (sqrMagnitudeTemp < sqrPerceptionRange) {
                if (other != this) {    // skip self
                    // store the neighbor Boid as dictionary key, with value = a tuple of Vector3 vectorBetween, float of the distance squared for super fast lookups.
                    neighbors.Add(other, (vectorBetween, sqrMagnitudeTemp));
                }
            }
        }
    }

    // Separation = Steer to avoid crowding local flockmates
    private void Separation() {
        if (boidSettings.separationStrength > 0) {
            if (neighbors == null || neighbors.Count <= 0) {
                return;
            } 
            else {
                foreach (KeyValuePair<Boid, (Vector3, float)> item in neighbors) {
                    // get the vector between self/other (Item1), then multiply by inverse squared magnitude of distance (Item2), then flip the sign from positive to negative (avoid instead of seek)
                    separationForce -= (item.Value.Item1 * 1/item.Value.Item2);
                }
                separationForce /= neighbors.Count; // get avg separationForce
                separationForce *= boidSettings.separationStrength;
                ApplyForce(separationForce);
                // Debug.Log("Separation Force " + separationForce);   
            }
        }
    }

    // Alignment = Steer towards the average heading of local flockmates
    private void Alignment() {
        if (boidSettings.alignmentStrength > 0) {
            if (neighbors == null || neighbors.Count <= 0) {
                return;
            }
            else {
                foreach (KeyValuePair<Boid, (Vector3, float)> item in neighbors) {
                    // TODO: CODE THIS
                }
            }
        }
    }

    // Cohesion = Steer to move toward the average position of local flockmates
    private void Cohesion() {
        if (boidSettings.cohesionStrength > 0) {
            if (neighbors == null || neighbors.Count <= 0) {
                return;
            }
            else {
                foreach (KeyValuePair<Boid, (Vector3, float)> item in neighbors) {
                    // TODO: CODE THIS
                }
            }
        }
    }


}
