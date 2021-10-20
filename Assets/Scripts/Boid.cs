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
    private Dictionary<Boid, (Vector3, Vector3, float)> neighbors;  // Key=Boid, Values=(velocityOther, vectorBetween, sqrMagnitude distance)
    private Vector3 vectorBetween, velocityOther, targetVector;
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
        ResetForces(); // reset all forces to 0 since intertial bodies continue at same velocity unless a force acts on them
    }

    private void ApplyForce(Vector3 force) {
        force /= boidSettings.mass;
        acceleration += force;
    }

    private void ResetForces() {
        acceleration = separationForce = alignmentForce = cohesionForce = Vector3.zero;
        if (boidSettings.speed > boidSettings.maxSpeed - 1f)
            boidSettings.speed = boidSettings.maxSpeed - 1f;
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
        FindNeighbors();    // do once and store data used for all 3 methods in a dictionary for efficiency and speed

        Separation();
        Alignment();
        Cohesion();

        ApplyForce(separationForce);
        ApplyForce(alignmentForce);
        ApplyForce(cohesionForce);
    }

    private void FindNeighbors() {
        if (neighbors == null)
            neighbors = new Dictionary<Boid, (Vector3, Vector3, float)>();
        else
            neighbors.Clear();

        // sqrMagnitude is a bit faster than Magnitude since it doesn't require sqrt function
        sqrPerceptionRange = boidSettings.perceptionRange * boidSettings.perceptionRange;
        // reset values before looping through neighbors
        velocityOther = vectorBetween = Vector3.zero;
        sqrMagnitudeTemp = 0f;
        foreach (Boid other in population) {
            velocityOther = other.velocity;
            vectorBetween = other.transform.position - transform.position;
            sqrMagnitudeTemp = vectorBetween.sqrMagnitude;
            if (sqrMagnitudeTemp < sqrPerceptionRange) {
                if (other != this) {    // skip self
                    // store the neighbor Boid as dictionary key, with value = a tuple of Vector3 vectorBetween, float of the distance squared for super fast lookups.
                    neighbors.Add(other, (velocityOther, vectorBetween, sqrMagnitudeTemp));
                }
            }
        }
    }

    // SEPARATION (aka avoidance) = Steer to avoid crowding local flockmates
    private void Separation() {
        if (boidSettings.separationStrength > 0) {
            if (neighbors == null || neighbors.Count <= 0) {
                return;
            } 
            else {
                foreach (KeyValuePair<Boid, (Vector3, Vector3, float)> item in neighbors) {
                    // get the vector between self/other (Item2), then multiply by inverse squared magnitude of distance (Item3), then flip the sign from positive to negative (avoid instead of seek)
                    separationForce -= (item.Value.Item2 * 1/item.Value.Item3);
                }
                separationForce /= neighbors.Count; // get avg separationForce
                separationForce *= boidSettings.separationStrength; 
            }
        }
    }

    // ALIGNMENT (aka copy) = Steer towards the average heading of local flockmates
    private void Alignment() {
        if (boidSettings.alignmentStrength > 0) {
            if (neighbors == null || neighbors.Count <= 0) {
                return;
            }
            else {
                foreach (KeyValuePair<Boid, (Vector3, Vector3, float)> item in neighbors) {
                    alignmentForce += item.Value.Item1; // sum all neighbor vectors
                }
                alignmentForce /= neighbors.Count;    // shrink to avg length/speed
                alignmentForce = alignmentForce.normalized;
                alignmentForce *= boidSettings.maxSpeed;
                alignmentForce -= velocity; // get vector between target and current velocity
                alignmentForce *= boidSettings.alignmentStrength;
            }
        }
    }

    // COHESION (aka center) = Steer to move toward the central position of local flockmates
    private void Cohesion() {
        if (boidSettings.cohesionStrength > 0) {
            if (neighbors == null || neighbors.Count <= 0) {
                return;
            } 
            else {
                foreach (KeyValuePair<Boid, (Vector3, Vector3, float)> item in neighbors) {
                    // TODO: CODE THIS

                }
            }
        }
    }


}
