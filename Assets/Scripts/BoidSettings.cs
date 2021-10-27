using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoidSettings", menuName = "ScriptableObjects/BoidSettings", order = 1)]
public class BoidSettings : ScriptableObject {
    public int totalBoids = 250;
    [Tooltip("Must check or uncheck this BEFORE pressing Play in Unity Editor")]
    public bool spawnUsingJobSystem = false;
    [Tooltip("Game Object for boid including 3d model, animations, etc")]
    public GameObject boidPrefab;
    [Range(0.1f, 5f)]
    public float mass = 1;
    [Range(0, 8)]
    public float speed = 3;
    [Range(0, 1f)]
    public float maxForce = 0.1f;
    [Range(0, 2f)]
    public float perceptionRange = 0.5f;
    [Range(0, 1f)]
    public float separationStrength = 0.2f;
    [Range(0, 1f)]
    public float alignmentStrength = 1f;
    [Range(0, 1f)]
    public float cohesionStrength = 1f;
    public bool moveFwd = true;
    public bool boundsOn = true;
    public bool drawDebugLines = false;

    public void ChangeSpeed(float spd) {
        speed = Mathf.Clamp(spd, 0, 8);
    }
    public void ChangeMaxForce(float mxForce) {
        maxForce = Mathf.Clamp(mxForce, 0, 1);
    }
    public void ChangePerception(float prcptn) {
        perceptionRange = Mathf.Clamp(prcptn, 0, 2);
    }
    public void ChangeSeparation(float seprtn) {
        separationStrength = Mathf.Clamp(seprtn, 0, 1);
    }
    public void ChangeAlignment(float alignmt) {
        alignmentStrength = Mathf.Clamp(alignmt, 0, 1);
    }
    public void ChangeCohesion(float cohesn) {
        cohesionStrength = Mathf.Clamp(cohesn, 0, 1);
    }
    public void ToggleDebugLines() {
        drawDebugLines = !drawDebugLines;
    }
}
