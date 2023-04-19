using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;

/// <summary>
/// Scenario B) BoidManager handles all boid behavior in a centralized manager class.
/// It is faster than A) individual boids, but slower than C) a manager that uses the Jobs system and D) Entities + Jobs system
/// </summary>
public class BoidManager : MonoBehaviour
{
  public static List<Boid> population;

  public BoidSettings boidSettings;
  private Vector3 boundaryCenter;
  private float boundaryRadius = 10;
  private Quaternion targetRotation;
  private Vector3 velocity, acceleration, separationForce, alignmentForce, cohesionForce;
  // variables for FindNeighbors made global to minimize garbage collection
  // neighbors: Key=Boid, Values=(position, velocityOther, vectorBetween, sqrMagnitude distance)
  private Dictionary<Boid, (Vector3, Vector3, Vector3, float)> neighbors;  
  private Vector3 vectorBetween, velocityOther, targetVector;
  private float sqrPerceptionRange, sqrMagnitudeTemp;

  private void Awake()
  {
    if (population == null)
      population = new List<Boid>();
    // population.Add(this);
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
    Flocking(); // handles Separation, Alignment, Cohesion calculations
    Move(); // handles movement (shocker!) except for boundary turning which is handled below
    TurnAtBounds(); // makes sure boids turn back when they reach bounds
    ResetForces(); // reset all forces to 0 since inertial bodies continue at same velocity unless a force acts on them
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
    velocity = Vector3.zero;    // reset velocity
    if (boidSettings.moveFwd)
      velocity = transform.forward * boidSettings.speed;  // add forward movement if box checked
    acceleration = Vector3.ClampMagnitude(acceleration, boidSettings.maxAccel);
    velocity += acceleration;
    // move position and rotation
    transform.position += velocity * Time.deltaTime;
    transform.rotation = Quaternion.LookRotation(velocity);
  }

  private void TurnAtBounds()
  {
    if (!boidSettings.boundsOn)
    {
      return;
    }
    else if ((transform.position - boundaryCenter).sqrMagnitude > (boundaryRadius * boundaryRadius) * 0.9f)
    {
      targetRotation = Quaternion.LookRotation(boundaryCenter - transform.position + Random.onUnitSphere * boundaryRadius * 0.5f);
    }
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * boidSettings.speed);
  }

  private void Flocking()
  {
    FindNeighbors();    // do once and store data used for all 3 methods in a dictionary for efficiency and speed

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

    // sqrMagnitude is a bit faster than Magnitude since it doesn't require sqrt function
    sqrPerceptionRange = boidSettings.perceptionRange * boidSettings.perceptionRange;
    // reset values before looping through neighbors
    velocityOther = vectorBetween = Vector3.zero;
    sqrMagnitudeTemp = 0f;
    foreach (Boid other in population)
    {
      // velocityOther = other.velocity;
      vectorBetween = other.transform.position - transform.position;
      sqrMagnitudeTemp = vectorBetween.sqrMagnitude;
      if (sqrMagnitudeTemp < sqrPerceptionRange)
      {
        if (other != this)
        {    // skip self
             // store the neighbor Boid as dictionary key, with value = a tuple of Vector3 vectorBetween, float of the distance squared for super fast lookups.
          neighbors.Add(other, (other.transform.position, velocityOther, vectorBetween, sqrMagnitudeTemp));
        }
      }
    }
  }

  // SEPARATION (aka buffer) = Steer to avoid crowding local flockmates
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
          // adjust range depending on strength
          if (item.Value.Item4 < boidSettings.perceptionRange * boidSettings.separationStrength)
          {
            separationForce -= item.Value.Item3;    // Item3 = vectorBetween
          }
        }
        separationForce *= boidSettings.separationStrength;
        separationForce = Vector3.ClampMagnitude(separationForce, boidSettings.maxAccel / 2);   // clamp separation to be much weaker than other 2 methods to avoid jitter
        if (boidSettings.drawDebugLines)
          Debug.DrawLine(transform.position, transform.position + separationForce, Color.red);
      }
    }
  }

  // ALIGNMENT (aka avg direction) = Steer towards the average heading of local flockmates
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
          alignmentForce += item.Value.Item2; // sum all neighbor velocities (Item2 = velocity)
        }
        // alignmentForce = alignmentForce.normalized * boidSettings.speed; // removed - slowed things down and not needed
        alignmentForce /= neighbors.Count;
        alignmentForce *= boidSettings.alignmentStrength;
        alignmentForce = Vector3.ClampMagnitude(alignmentForce, boidSettings.maxAccel);
        if (boidSettings.drawDebugLines)
          Debug.DrawLine(transform.position, transform.position + alignmentForce, Color.green);
      }
    }
  }

  // COHESION (aka center) = Steer to move toward the central position of local flockmates
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
          cohesionForce += item.Value.Item1; // sum all neighbor positions (Item1 = position)
        }
        cohesionForce /= neighbors.Count;   // get average position (center)
        cohesionForce -= transform.position; // convert to a vector pointing from boid to center
        cohesionForce *= boidSettings.cohesionStrength;
        cohesionForce = Vector3.ClampMagnitude(cohesionForce, boidSettings.maxAccel);
        if (boidSettings.drawDebugLines)
          Debug.DrawLine(transform.position, transform.position + cohesionForce, Color.blue);
      }
    }
  }

}
