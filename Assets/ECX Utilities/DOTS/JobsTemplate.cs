/*
ECX UTILITY SCRIPTS
Unity C# Job System Template
Last updated: Apr 16, 2022
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;    // optimized version of System.Collections built for safe multi-threading. Includes NativeArray, NativeList, NativeQueue, NativeHashMap, NativeMultiHashMap.

public class JobsTemplate : MonoBehaviour {
    // Variables and NativeArrays
    private NativeArray<Vector3> velocities;
    private TransformAccessArray spriteTransformArray;

    private void Start() {
        // Initialize Data
    }

    private void Update() {
        // Create the job and assign all variables within the job
        // Create the jobhandle and schedule the job (schedule early, complete late)
    }

    private void LateUpdate() {
        // Ensure completion of the job, returning control to main thread (schedule early, complete late)
        // assign variables as needed (note that only NativeContainers retain data after a job is complete)
    }

    private void OnDestroy() {
        // Dispose of all NativeContainers (important! will cause memory leak if not done)
        velocities.Dispose();
        spriteTransformArray.Dispose();
    }

    // Example of IJob struct
    [BurstCompile]
    private struct DoWork : IJob {
        // Job variables here

        public void Execute () {
            // Job execution code
        }
    }

    // Example of IJobParallelFor struct
    [BurstCompile]
    private struct UpdateMeshJob : IJobParallelFor {
        // Job variables here

        public void Execute (int i) {
            // Job execution code
        }
    }

    // Example of IJobParallelForTransform struct
    [BurstCompile]
    struct PositionUpdateJob : IJobParallelForTransform {
        // Job variables here

        // NOTE - this version of Execute also passes a TransformAccess variable
        public void Execute(int i, TransformAccess transformTA) {
            // Job execution code
        }
    }
}