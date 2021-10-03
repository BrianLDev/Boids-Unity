using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class Boid : MonoBehaviour {
    public static List<Boid> population;
    [Range(0, 5f)]
    public float speed = 3;
    [Range(0, 2.5f)]
    public float perceptionRange = 0.5f;
    [Range(0, 5f)]
    public float separationStrength = 1f;
    [Range(0, 5f)]
    public float alignmentStrength = 1f;
    [Range(0, 5f)]
    public float cohesionStrength = 1f;
    private List<Boid> neighbors;
    private Vector3 boundaryCenter;
    private float boundaryRadius = 10;
    private Quaternion targetRotation;
    private Vector3 velocityAdj, targetVelocity; 

    private void Awake() {
        transform.forward = Random.insideUnitSphere.normalized * speed;
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

    private void Flocking() {
        // FindNeighbors();
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
        float sqrPerceptionRange = perceptionRange * perceptionRange;
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
            velocityAdj *= speed;

            if (targetVelocity == Vector3.zero) {
                targetVelocity = transform.forward * speed + velocityAdj;
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
