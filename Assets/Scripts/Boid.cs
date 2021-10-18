using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Timeline;

public class Boid : MonoBehaviour {
    public static List<Boid> population;
    public BoidSettings boidSettings;
    private List<Boid> neighbors;
    private Vector3 boundaryCenter;
    private float boundaryRadius = 10;
    private Quaternion targetRotation;
    private Vector3 separationForce, alignmentForce, cohesionForce, acceleration; 

    private void Awake() {
        transform.forward = Random.insideUnitSphere.normalized * boidSettings.speed;
        if (population == null)
            population = new List<Boid>();
        population.Add(this);
    }

    public void Initialize(Vector3 boundCenter, float boundRadius) {
        boundaryCenter = boundCenter;
        boundaryRadius = boundRadius;
        acceleration = new Vector3(0,0,0);
    }

    private void FixedUpdate() {
        MoveFwd();
        TurnAtBounds();
        Flocking();    
    }

    private void MoveFwd() {
        transform.position += transform.forward * boidSettings.speed * Time.deltaTime;
    }

    private void TurnAtBounds() {
        if ((transform.position - boundaryCenter).magnitude > boundaryRadius * 0.9f) {
            // targetRotation = Quaternion.Inverse(transform.rotation);
            targetRotation = Quaternion.LookRotation(boundaryCenter - transform.position + Random.onUnitSphere*boundaryRadius*0.5f);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * boidSettings.speed);
    }

    private void Flocking() {
        FindNeighbors();
        Separation();
        // Alignment();
        // Cohesion;
        
        acceleration = separationForce + alignmentForce + cohesionForce;
        transform.position += acceleration * Time.deltaTime;
        // reset forces
        separationForce = alignmentForce = cohesionForce = Vector3.zero;
    }

    private void FindNeighbors() {
        if (neighbors == null)
            neighbors = new List<Boid>();
        else
            neighbors.Clear();

        // sqrMagnitude is a bit faster than Magnitude since it doesn't require sqrt
        float sqrPerceptionRange = boidSettings.perceptionRange * boidSettings.perceptionRange;
        float sqrMagnitudeTemp = 0f;
        foreach (Boid other in population) {
            sqrMagnitudeTemp = (other.transform.position - transform.position).sqrMagnitude;
            if (sqrMagnitudeTemp < sqrPerceptionRange) {
                if (other.transform.position != transform.position) // skip self
                    neighbors.Add(other);
            }
        }
    }

    // Separation = Steer to avoid crowding local flockmates
    private void Separation() {
        if (neighbors == null || neighbors.Count <= 0) {
            return;
        } 
        else {
            foreach (Boid other in neighbors) {
                // calc separation Vector and invert (larger distance = smaller value)
                separationForce += (other.transform.position-transform.position) / (other.transform.position-transform.position).magnitude;
            }
            separationForce /= neighbors.Count; // get avg inverted distance
            separationForce *= boidSettings.speed;
            Debug.Log("Separation Force " + separationForce);   
        }
    }

    // Alignment = Steer towards the average heading of local flockmates
    private void Alignment() {
        if (neighbors == null || neighbors.Count <= 0) {
            return;
        }
        else {
            foreach (Boid other in neighbors) {

            }
        }
    }

    // Cohesion = Steer to move toward the average position of local flockmates
    private void Cohesion() {
        if (neighbors == null || neighbors.Count <= 0) {
            return;
        }
        else {
            
        }
    }


}
