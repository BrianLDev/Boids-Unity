using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoidSettings", menuName = "ScriptableObjects/BoidSettings", order = 1)]
public class BoidSettings : ScriptableObject {
    public int totalBoids = 250;
    [Tooltip("Must check or uncheck this before pressing Play in Unity Editor")]
    public bool spawnUsingJobSystem = false;
    [Range(0, 5f)]
    public float mass = 1;
    [Range(0, 8)]
    public float speed = 3;
    [Range(1, 9f)]
    public float maxSpeed = 5;
    [Range(0, 1f)]
    public float maxForce = 0.1f;
    [Range(0, 2f)]
    public float perceptionRange = 0.5f;
    [Range(0, 2f)]
    public float separationStrength = 0.2f;
    [Range(0, 2f)]
    public float alignmentStrength = 1f;
    [Range(0, 2f)]
    public float cohesionStrength = 1f;
    public bool moveFwd = true;
    public bool boundsOn = true;
    public bool drawDebugLines = false;
}
