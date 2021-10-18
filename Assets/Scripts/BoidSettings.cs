using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoidSettings", menuName = "ScriptableObjects/BoidSettings", order = 1)]
public class BoidSettings : ScriptableObject {
    [Range(0, 5f)]
    public float speed = 3;
    [Range(0, 2.5f)]
    public float perceptionRange = 0.25f;
    [Range(0, 5f)]
    public float separationStrength = 1f;
    [Range(0, 5f)]
    public float alignmentStrength = 1f;
    [Range(0, 5f)]
    public float cohesionStrength = 1f;
}
