using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
    boidSettings.boundsOn = true;

    if (!spawnLocation)
      spawnLocation = this.transform;
    else
      boundaryRadius = spawnLocation.localScale.x / 2;
  }

  private void Start()
  {
    boidSettings.ResetSettings();
    SpawnBoids();

    if (mainVCam)
      WatchMainCamera();
    if (boidSettings.simMethod == SimMethod.Individual)
      Debug.Log("Total boids: " + Boid.boidList.Count);
    else
      Debug.Log("Total boids: " + BoidManager.BoidCount);
  }

  public void SpawnBoids()
  {
    if (boidSettings.nextSimMethod != boidSettings.simMethod)
      boidSettings.simMethod = boidSettings.nextSimMethod;
    Debug.Log("Spawning " + boidSettings.boidCount + " boids using method: " + boidSettings.simMethod.ToString());
    if (boidSettings.simMethod == SimMethod.Individual)
      SpawnBoidsIndividual();
    else if (boidSettings.simMethod == SimMethod.Manager)
      SpawnBoidsManager();
    else if (boidSettings.simMethod == SimMethod.MgrJobs)
      SpawnBoidsMgrJobs();
    else if (boidSettings.simMethod == SimMethod.MgrJobsECS)
      SpawnBoidsMgrJobsEcs();
  }

  private void SpawnBoidsIndividual()
  {
    // Create or clear BoidList
    if (Boid.boidList == null)
      Boid.boidList = new List<Boid>();
    else
      Boid.boidList.Clear();
    // Spawn Boids
    Boid newBoid;
    for (int i = 0; i < boidSettings.boidCount; i++)
    {
      boidLocation = Random.insideUnitSphere.normalized * Random.Range(0, boundaryRadius * 0.9f);
      newBoid = Instantiate(boidSettings.boidPrefab, boidLocation, Quaternion.identity, this.transform).GetComponent<Boid>();
      newBoid.boidSettings = boidSettings;
      newBoid.SetBoundarySphere(spawnLocation.position, boundaryRadius);
    }
  }

  private void SpawnBoidsManager() {
    // TODO: WRITE THIS
  }

  private void SpawnBoidsMgrJobs() {
    // TODO: WRITE THIS
  }

  private void SpawnBoidsMgrJobsEcs() {
    // TODO: WRITE THIS
  }

  public void RespawnBoids()
  {
    KillBoids();
    SpawnBoids();
    UIManager.Instance.RefreshUI();
  }

  public void KillBoids()
  {
    if (boidSettings.simMethod == SimMethod.Individual) {
      foreach (Boid boid in Boid.boidList) {
        Destroy(boid.gameObject);
      }
      Boid.boidList.Clear();
    }
    // TODO: WRITE KILL SCRIPT FOR OTHER SIM METHODS
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
      if (boidSettings.simMethod == SimMethod.Individual)
        fishVCam.Follow = fishVCam.LookAt = Boid.boidList[randomBoid].transform;
      else
        fishVCam.Follow = fishVCam.LookAt = Boid.boidList[randomBoid].transform;
    }
  }
}