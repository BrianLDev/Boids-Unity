using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoidSettings", menuName = "ScriptableObjects/BoidSettings", order = 1)]
public class BoidSettings : ScriptableObject {
    [Range(0, 5f)]
    public float mass = 1;
    [Range(0, 10f)]
    public float speed = 3;
    [Range(0, 1f)]
    public float perceptionRange = 0.5f;
    [Range(0, 2f)]
    public float separationStrength = 0.3f;
    [Range(0, 2f)]
    public float alignmentStrength = 1f;
    [Range(0, 2f)]
    public float cohesionStrength = 1f;
    public bool moveFwd = true;
    public bool boundsOn = true;
}
