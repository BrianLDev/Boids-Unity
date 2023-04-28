using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SimMethod { Individual, Manager, MgrJobs, MgrJobsECS }

[CreateAssetMenu(fileName = "BoidSettings", menuName = "ScriptableObjects/BoidSettings", order = 1)]
public class BoidSettings : ScriptableObject
{
  public SimMethod simMethod = SimMethod.Individual;
  public SimMethod nextSimMethod = SimMethod.Individual;    // Used to track the next method used on respawn
  public int boidCount;
  [Tooltip("Game Object for boid including 3d model, animations, etc")]
  public GameObject boidPrefab;
  [Range(0.01f, 1f)]
  public float separationStrength;
  [Range(0.01f, 1f)]
  public float alignmentStrength;
  [Range(0.01f, 1f)]
  public float cohesionStrength;
  [Range(0.1f, 5f)]
  public float mass;
  [Range(0.01f, 3)]
  public float speed;
  [Range(0.01f, 5f)]
  public float maxAccel;
  [Range(0.01f, 2f)]
  public float perceptionRange;
  public bool boundsOn = true;
  public bool drawDebugLines = false;
  private GameObject bounds;
  // Initial values to be used at start and reset
  private const int initBoidCount = 250;
  private const int maxBoidCount = 500;
  private const float initSeparStr = 0.65f;
  private const float initAlignStr = 0.55f;
  private const float initCohesStr = 0.4f;
  private const float initMass = 1;
  private const float initSpeed = 2.5f;
  private const float initMaxForce = 0.4f;
  private const float initPerceptRange = 1.0f;

  public void ChangeCount(float count) => boidCount = Mathf.Clamp((int)count, 1, maxBoidCount);
  public void ChangeSeparation(float separation) => separationStrength = Mathf.Clamp(separation, 0, 1);
  public void ChangeAlignment(float alignment) => alignmentStrength = Mathf.Clamp(alignment, 0, 1);
  public void ChangeCohesion(float cohesion) => cohesionStrength = Mathf.Clamp(cohesion, 0, 1);
  public void ChangeSpeed(float spd) => speed = Mathf.Clamp(spd, 0, 8);
  public void ChangeMaxForce(float mxForce) => maxAccel = Mathf.Clamp(mxForce, 0, 1);
  public void ChangePerception(float perception) => perceptionRange = Mathf.Clamp(perception, 0, 2);

  public void ResetSettings()
  {
    simMethod = SimMethod.Individual;
    nextSimMethod = SimMethod.Individual;
    boidCount = initBoidCount;
    separationStrength = initSeparStr;
    alignmentStrength = initAlignStr;
    cohesionStrength = initCohesStr;
    mass = initMass;
    speed = initSpeed;
    maxAccel = initMaxForce;
    perceptionRange = initPerceptRange;
    boundsOn = true;
    drawDebugLines = false;
    UIManager.Instance.RefreshUI();
  }
  public void Reset()
  {
    ResetSettings();
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

  // NOTE - TOGGLECAMERA METHOD IS ON THE BOIDSPAWNER SCRIPT
}