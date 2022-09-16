using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;

public class BoidJobs : MonoBehaviour
{
// #region Variables
//     public static List<BoidJobs> population;
//     public BoidSettings boidSettings;
//     private float3 boundaryCenter;
//     private float boundaryRadius = 10;
//     private Quaternion targetRotation;
//     private float3 velocity, acceleration, separationForce, alignmentForce, cohesionForce;
//     // variables for FindNeighbors made global to minimize garbage collection
//     private Dictionary<BoidJobs, (float3, float3, float3, float)> neighbors;  // Key=Boid, Values=(position, velocityOther, vectorBetween, sqrMagnitude distance)
//     private float3 vectorBetween, velocityOther, targetVector;
//     private float sqrPerceptionRange, sqrMagnitudeTemp;
// #endregion Variables

// #region Methods
//     private void Awake()
//     {
//         if (population == null)
//             population = new List<BoidJobs>();
//         population.Add(this);
//         Initialize();
//     }

//     public void Initialize()
//     {
//         transform.forward = UnityEngine.Random.insideUnitSphere.normalized;
//         velocity = transform.forward * boidSettings.speed;
//         acceleration = new float3(0, 0, 0);
//     }

//     public void SetBoundarySphere(float3 center, float radius)
//     {
//         boundaryCenter = center;
//         boundaryRadius = radius;
//     }

//     private void FixedUpdate()
//     {
//         Flocking(); // handles Separation, Alignment, Cohesion forces
//         Move(); // handles movement (shocker!) except for boundary turning which is handled below
//         TurnAtBounds(); // makes sure boids turn back when they reach bounds
//         ResetForces(); // reset all forces to 0 since intertial bodies continue at same velocity unless a force acts on them
//     }

//     private void ApplyForce(Vector3 force)
//     {
//         force /= boidSettings.mass;
//         acceleration += force;
//     }


//     private void Update()
//     {
//         // Create the job and assign all variables within the job
//         // Create the jobhandle and schedule the job (schedule early, complete late)
//     }

//     private void LateUpdate()
//     {
//         // Ensure completion of the job, returning control to main thread (schedule early, complete late)
//         // assign variables as needed (note that only NativeContainers retain data after a job is complete)
//     }

//     private void OnDestroy()
//     {
//         // Dispose of all NativeContainers (important! will cause memory leak if not done)
//         velocities.Dispose();
//         spriteTransformArray.Dispose();
//     }

//     // Example of IJob struct
//     [BurstCompile]
//     private struct DoWork : IJob
//     {
//         // Job variables here

//         public void Execute()
//         {
//             // Job execution code
//         }
//     }

//     // Example of IJobParallelFor struct
//     [BurstCompile]
//     private struct UpdateMeshJob : IJobParallelFor
//     {
//         // Job variables here

//         public void Execute(int i)
//         {
//             // Job execution code
//         }
//     }

//     // Example of IJobParallelForTransform struct
//     [BurstCompile]
//     struct PositionUpdateJob : IJobParallelForTransform
//     {
//         // Job variables here

//         // NOTE - this version of Execute also passes a TransformAccess variable
//         public void Execute(int i, TransformAccess transformTA)
//         {
//             // Job execution code
//         }
//     }
// #endregion Methods
}