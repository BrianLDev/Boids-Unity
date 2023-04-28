using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scenario A) Boid class handles each Boid behavior individually.  It is the slowest of the 4 scenarios.
/// </summary>
public class Boid : MonoBehaviour
{
  public static List<Boid> boidList;

  public BoidSettings boidSettings;
  private Vector3 boundaryCenter;
  private float boundaryRadius = 10;
  private bool turningAround = false;
  private Quaternion targetRotation;
  private Vector3 velocity, acceleration, separationForce, alignmentForce, cohesionForce;
  // Variables for FindNeighbors made global to minimize garbage collection
  // Neighbors: Key=Boid, Values=(position, velocityOther, vectorBetween, sqrMagnitude distance)
  private Dictionary<Boid, (Vector3, Vector3, Vector3, float)> neighbors;  
  private Vector3 vectorBetween, velocityOther, targetPosition;
  private float sqrPerceptionRange, sqrMagnitudeTemp;

  private void Awake()
  {
    if (boidList == null)
      boidList = new List<Boid>();
    boidList.Add(this);
    if (neighbors == null)
      neighbors = new Dictionary<Boid, (Vector3, Vector3, Vector3, float)>();
  }

  private void Start() {
    Initialize();
  }

  public void Initialize()
  {
    transform.forward = Random.insideUnitSphere.normalized;
    velocity = transform.forward * boidSettings.speed;
    acceleration = new Vector3(0, 0, 0);
  }

  public void SetBoundarySphere(Vector3 center, float radius)
  {
    boundaryCenter = center;
    boundaryRadius = radius;
  }

  private void Update()
  {
    Flocking();     // Handles Separation, Alignment, Cohesion calculations
    TurnAtBounds(); // Makes sure boids turn back when they reach bounds
    Move();         // Handles movement (shocker!) except for boundary turning which is handled below
    ResetForces();  // Reset all forces to 0 since inertial bodies continue at same velocity unless a force acts on them
  }

  private void ApplyForce(Vector3 force)
  {
    acceleration += (force / boidSettings.mass);
  }

  private void ResetForces()
  {
    acceleration = separationForce = alignmentForce = cohesionForce = Vector3.zero;
  }

  private void Move()
  {
    // Update velocity by (clamped) acceleration
    acceleration = Vector3.ClampMagnitude(acceleration, boidSettings.maxAccel);
    // velocity = Vector3.Lerp(velocity, velocity + acceleration, Time.deltaTime * boidSettings.speed * 2);
    velocity += acceleration;
    velocity = Vector3.ClampMagnitude(velocity, boidSettings.speed);
    if (velocity.sqrMagnitude <= .1f)
      velocity = transform.forward * boidSettings.speed;
    // Update position and rotation
    if (velocity != Vector3.zero) {
      transform.position += velocity * Time.deltaTime;
      transform.rotation = Quaternion.LookRotation(velocity);
    }
  }

  private void TurnAtBounds()
  {
    if (!boidSettings.boundsOn)
      return;
    // Check if outside bounds
    else if ((transform.position - boundaryCenter).sqrMagnitude > (boundaryRadius * boundaryRadius))
    {
      if (!turningAround) {
        // If not already turning around, set a new target position on the opposite side of the boundary sphere
        targetPosition = boundaryCenter + (boundaryCenter - transform.position);
        turningAround = true;
      }
      // Keep turning and moving towards targetPosition until targetRotation reached
      targetRotation = Quaternion.LookRotation(targetPosition);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * boidSettings.speed);
      velocity = Vector3.Slerp(velocity, targetPosition - transform.position, Time.deltaTime * boidSettings.speed);
    }
    // After reaching target rotation (within range), stop turning around
    else if (Quaternion.Angle(transform.rotation, targetRotation) <= .01f)
      turningAround = false;
  }

  private void Flocking()
  {
    // Since there is overlap in the data needed for Separation, Alignment & Cohesion, gather all the data once and store in a dictionary for efficiency and fast lookups
    FindNeighbors();

    Separation();
    Alignment();
    Cohesion();

    ApplyForce(separationForce);
    ApplyForce(alignmentForce);
    ApplyForce(cohesionForce);
  }

  private void FindNeighbors()
  {
    neighbors.Clear();

    // SqrMagnitude is a bit faster than Magnitude since it doesn't require sqrt function
    sqrPerceptionRange = boidSettings.perceptionRange * boidSettings.perceptionRange;
    // Reset values before looping through neighbors
    velocityOther = vectorBetween = Vector3.zero;
    sqrMagnitudeTemp = 0f;
    foreach (Boid other in boidList)
    {
      velocityOther = other.velocity;
      vectorBetween = other.transform.position - transform.position;
      sqrMagnitudeTemp = vectorBetween.sqrMagnitude;
      if (sqrMagnitudeTemp < sqrPerceptionRange)
      {
        // Skip self
        if (other != this)
        {
          // Store the neighbor Boid as dictionary for fast lookups, with value = a tuple of Vector3 position, velocityOther, vectorBetween, and float of the distance squared.
          neighbors.Add(other, (other.transform.position, velocityOther, vectorBetween, sqrMagnitudeTemp));
        }
      }
    }
  }

  // SEPARATION (aka buffer) == Steer to avoid crowding local flockmates
  private void Separation()
  {
    if (boidSettings.separationStrength > 0)
    {
      if (neighbors == null || neighbors.Count <= 0)
        return;
      else
      {
        foreach (KeyValuePair<Boid, (Vector3, Vector3, Vector3, float)> item in neighbors)
        {
          // Adjust range depending on strength
          if (item.Value.Item4 < boidSettings.perceptionRange * boidSettings.separationStrength)  // Item4 == squaredDistance
          {
            separationForce -= item.Value.Item3;    // Item3 == vectorBetween
          }
        }
        separationForce *= boidSettings.separationStrength;
        separationForce = Vector3.ClampMagnitude(separationForce, boidSettings.maxAccel / 2);   // Clamp separation to be much weaker than other 2 methods to avoid jitter
        if (boidSettings.drawDebugLines) {
          Debug.DrawLine(transform.position, transform.position + separationForce, Color.red);
        }
      }
    }
  }

  // ALIGNMENT (aka avg direction) == Steer towards the average heading of local flockmates
  private void Alignment()
  {
    if (boidSettings.alignmentStrength > 0)
    {
      if (neighbors == null || neighbors.Count <= 0)
        return;
      else
      {
        foreach (KeyValuePair<Boid, (Vector3, Vector3, Vector3, float)> item in neighbors)
        {
          alignmentForce += item.Value.Item2; // Sum all neighbor velocities (Item2 == velocity)
        }
        alignmentForce /= neighbors.Count;
        alignmentForce *= boidSettings.alignmentStrength;
        alignmentForce = Vector3.ClampMagnitude(alignmentForce, boidSettings.maxAccel);
        if (boidSettings.drawDebugLines) {
          Debug.DrawLine(transform.position, transform.position + alignmentForce, Color.green);
        }
      }
    }
  }

  // COHESION (aka center) == Steer to move toward the central position of local flockmates
  private void Cohesion()
  {
    if (boidSettings.cohesionStrength > 0)
    {
      if (neighbors == null || neighbors.Count <= 0)
        return;
      else
      {
        foreach (KeyValuePair<Boid, (Vector3, Vector3, Vector3, float)> item in neighbors)
        {
          cohesionForce += item.Value.Item1; // Sum all neighbor positions (Item1 == position)
        }
        cohesionForce /= neighbors.Count;   // Get average position (center)
        cohesionForce -= transform.position; // Convert to a vector pointing from boid to center
        cohesionForce *= boidSettings.cohesionStrength;
        cohesionForce = Vector3.ClampMagnitude(cohesionForce, boidSettings.maxAccel);
        if (boidSettings.drawDebugLines) {
          Debug.DrawLine(transform.position, transform.position + cohesionForce, Color.blue);
        }
      }
    }
  }

}
