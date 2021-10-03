using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
    public static List<Boid> population;
    public BoidSettings boidSettings;
    private List<Boid> neighbors;
    private Vector3 boundaryCenter;
    private float boundaryRadius = 10;
    private Quaternion targetRotation;
    private Vector3 velocityAdj, targetVelocity; 

    private void Awake() {
        transform.forward = Random.insideUnitSphere.normalized * boidSettings.speed;
        if (population == null)
            population = new List<Boid>();
        population.Add(this);
    }

    public void Initialize(Vector3 boundCenter, float boundRadius) {
        boundaryCenter = boundCenter;
        boundaryRadius = boundRadius;
        targetVelocity = new Vector3(0,0,0);
    }
    
    private void Update() {
        MoveFwd();
        CheckBounds();
        TurningAround();
        Flocking();
    }

    private void MoveFwd() {
        transform.position += transform.forward * boidSettings.speed * Time.deltaTime;
    }

    private void CheckBounds() {
        if ((transform.position - boundaryCenter).magnitude > boundaryRadius * 0.9f) {
            // targetRotation = Quaternion.Inverse(transform.rotation);
            targetRotation = Quaternion.LookRotation(boundaryCenter - transform.position + Random.onUnitSphere*boundaryRadius*0.5f);
        }
    }

    private void TurningAround() {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * boidSettings.speed);
    }

    private void Flocking() {
        FindNeighbors();
        // Separation();
        // Alignment();
        // Cohesion;
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

    private void Separation() {
        if (neighbors != null && neighbors.Count > 0) {
            velocityAdj = Vector3.zero; // reset velocity adjustment
            foreach (Boid other in neighbors) {
                // calc separation Vector and invert (larger distance = smaller value)
                velocityAdj += (other.transform.position-transform.position) / (other.transform.position-transform.position).magnitude;
                Debug.Log("Velocity adj " + velocityAdj);
            }
            velocityAdj /= neighbors.Count; // get avg inverted distance
            velocityAdj *= boidSettings.speed;

            if (targetVelocity == Vector3.zero) {
                targetVelocity = transform.forward * boidSettings.speed + velocityAdj;
            } else {
                targetVelocity += velocityAdj;
            }
            Debug.Log("Velocity adj " + velocityAdj);
            // handle movement
            if (velocityAdj != null && velocityAdj.magnitude > 0) {
                Debug.Log("Moving from " + transform.position + " to " + transform.position + velocityAdj);
                transform.position = Vector3.Slerp(transform.position, targetVelocity, Time.deltaTime);
            }
        }
    }

    // private void Alignment() {
        
    // }

    // private void Cohesion() {
        
    // }


}
