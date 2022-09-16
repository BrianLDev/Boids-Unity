using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoidSettings", menuName = "ScriptableObjects/BoidSettings", order = 1)]
public class BoidSettings : ScriptableObject
{
#region Variables
    public int totalBoids = 200;
    // [Tooltip("Must check or uncheck this BEFORE pressing Play in Unity Editor")]
    // public bool useJobSystem = false;
    [Tooltip("Game Object for boid including 3d model, animations, etc")]
    public GameObject boidPrefab;
    [Range(0, 1f)]
    public float separationStrength = 0.3f;
    [Range(0, 1f)]
    public float alignmentStrength = 0.3f;
    [Range(0, 1f)]
    public float cohesionStrength = 0.5f;
    [Range(0.1f, 5f)]
    public float mass = 1;
    [Range(0, 3)]
    public float speed = 1f;
    [Range(0, 1f)]
    public float maxForce = 0.5f;
    [Range(0, 2f)]
    public float perceptionRange = 0.8f;
    public bool moveFwd = true;
    public bool boundsOn = true;
    public bool drawDebugLines = false;
    private GameObject bounds;
#endregion Variables

#region Methods 
    public void ChangeSeparation(float separation) => separationStrength = Mathf.Clamp(separation, 0, 1);
    public void ChangeAlignment(float alignment) => alignmentStrength = Mathf.Clamp(alignment, 0, 1);
    public void ChangeCohesion(float cohesion) => cohesionStrength = Mathf.Clamp(cohesion, 0, 1);
    public void ChangeSpeed(float spd) => speed = Mathf.Clamp(spd, 0, 8);
    public void ChangeMaxForce(float mxForce) => maxForce = Mathf.Clamp(mxForce, 0, 1);
    public void ChangePerception(float perception) => perceptionRange = Mathf.Clamp(perception, 0, 2);
    public void ResetSettings()
    {
        // NOTE - THIS RESETS THE ACTUAL VALUES BUT NOT THE UI SLIDERS.  NOT WORTH TIME/EFFORT TO CONNECT EVERYTHING TO UI.
        separationStrength = 0.5f;
        alignmentStrength = 0.5f;
        cohesionStrength = 0.3f;
        mass = 1;
        speed = 1;
        maxForce = 0.25f;
        perceptionRange = 0.75f;
        moveFwd = true;
        boundsOn = true;
        drawDebugLines = false;
    }
    public void ToggleDebugLines()
    {
        drawDebugLines = !drawDebugLines;
    }
    public void ToggleBounds()
    {
        boundsOn = !boundsOn;
        if (!bounds)
            bounds = GameObject.Find("Bounds");
        bounds.SetActive(boundsOn);
    }

    // NOTE - TOGGLECAM METHOD IS ON THE BOIDSPAWNER SCRIPT
#endregion Methods
}