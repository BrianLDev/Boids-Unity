using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
  [SerializeField] private BoidSettings boidSettings;
  [SerializeField] private BoidManager boidManager;
  [SerializeField] private Transform spawnLocation;
  [SerializeField] private CinemachineVirtualCamera mainVCam;
  [SerializeField] private CinemachineVirtualCamera fishVCam;
  private float boundaryRadius = 10;
  private Vector3 boidLocation;

  private void Awake()
  {
    // Fail safe to auto-load boidSettings, but ideally just pre-assign the settings in the Unity Editor
    if (!boidSettings) {
      string path = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("BoidSettings")[1]);
      boidSettings = AssetDatabase.LoadAssetAtPath<BoidSettings>(path);
    }
    boidSettings.boundsOn = true;

    if (!spawnLocation)
      spawnLocation = this.transform;
    else
      boundaryRadius = spawnLocation.localScale.x / 2;
  }

  private void Start()
  {
    boidSettings.ResetSettings();
    SpawnBoids(boidSettings.boidCount, boidSettings.boidMethod);

    if (mainVCam)
      WatchMainCamera();
    if (boidSettings.boidMethod == BoidMethod.Individual)
      Debug.Log("Total boids: " + Boid.boidList.Count);
    else
      Debug.Log("Total boids: " + boidManager.BoidCount);
  }

  public void SpawnBoids(int number, BoidMethod method)
  {
    Debug.Log("Spawning " + number + " boids using method: " + method.ToString());
    if (method == BoidMethod.Individual)
      SpawnBoidsIndividual(number);
    else if (method == BoidMethod.Manager)
      SpawnBoidsManager(number);
    else if (method == BoidMethod.MgrJobs)
      SpawnBoidsMgrJobs(number);
    else if (method == BoidMethod.MgrJobsECS)
      SpawnBoidsMgrJobsEcs(number);
  }

  public void SpawnBoidsIndividual(int number)
  {
    Boid newBoid;
    for (int i = 0; i < boidSettings.boidCount; i++)
    {
      boidLocation = Random.insideUnitSphere.normalized * Random.Range(0, boundaryRadius * 0.9f);
      newBoid = Instantiate(boidSettings.boidPrefab, boidLocation, Quaternion.identity, this.transform).GetComponent<Boid>();
      newBoid.boidSettings = boidSettings;
      newBoid.SetBoundarySphere(spawnLocation.position, boundaryRadius);
    }
  }

  public void SpawnBoidsManager(int number) {
    // TODO: WRITE THIS
  }

  public void SpawnBoidsMgrJobs(int number) {
    // TODO: WRITE THIS
  }

  public void SpawnBoidsMgrJobsEcs(int number) {
    // TODO: WRITE THIS
  }

  public void RespawnBoids()
  {
    // TODO: KILL BOIDS THEN RESPAWN
  }

  public void KillBoids()
  {
    // TODO: KILL BOIDS
  }

  public void ToggleCamera()
  {
    if (mainVCam.gameObject.activeInHierarchy)
      WatchFishCamera();
    else
      WatchMainCamera();
  }

  public void WatchMainCamera()
  {
    mainVCam.gameObject.SetActive(true);
    fishVCam.gameObject.SetActive(false);
  }

  public void WatchFishCamera()
  {
    if (fishVCam)
    {
      mainVCam.gameObject.SetActive(false);
      fishVCam.gameObject.SetActive(true);
      int randomBoid = Random.Range(0, boidSettings.boidCount);
      // TODO - SET UP FOLLOW CAM FOR BOID MANAGER METHODS (ELSE CONDITION)
      if (boidSettings.boidMethod == BoidMethod.Individual)
        fishVCam.Follow = fishVCam.LookAt = Boid.boidList[randomBoid].transform;
      else
        fishVCam.Follow = fishVCam.LookAt = Boid.boidList[randomBoid].transform;
    }
  }
}