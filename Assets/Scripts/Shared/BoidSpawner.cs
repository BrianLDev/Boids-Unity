using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
  [SerializeField] private BoidSettings boidSettings;
  [SerializeField] private Transform spawnLocation;
  [SerializeField] private CinemachineVirtualCamera mainVCam;
  [SerializeField] private CinemachineVirtualCamera fishVCam;
  private float boundaryRadius = 10;
  private Vector3 boidLocation;

  private void Awake()
  {
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
    SpawnBoids(boidSettings.totalBoids);

    if (mainVCam)
      WatchMainCam();
    Debug.Log("Total boids: " + Boid.population.Count);
  }

  public void SpawnBoids(int number)
  {
    // TODO: SELECT BETWEEN 4 SPAWN OPTIONS
    SpawnBoidsIndividual(number);
  }

  public void SpawnBoidsIndividual(int number)
  {
    Boid newBoid;
    for (int i = 0; i < boidSettings.totalBoids; i++)
    {
      boidLocation = Random.insideUnitSphere.normalized * Random.Range(0, boundaryRadius * 0.9f);
      newBoid = Instantiate(boidSettings.boidPrefab, boidLocation, Quaternion.identity, this.transform).GetComponent<Boid>();
      newBoid.boidSettings = boidSettings;
      newBoid.SetBoundarySphere(spawnLocation.position, boundaryRadius);
    }
  }

  // public void SpawnBoidsJobs(int number)
  // {
  //     BoidJobs newBoidJobs;
  //     for (int i = 0; i < boidSettings.totalBoids; i++)
  //     {
  //         boidLocation = Random.insideUnitSphere.normalized * Random.Range(0, boundaryRadius * 0.9f);
  //         newBoidJobs = Instantiate(boidSettings.boidPrefab, boidLocation, Quaternion.identity, this.transform).GetComponent<BoidJobs>();
  //         // newBoidJobs.SetBoundarySphere(spawnLocation.position, boundaryRadius);  // TODO: UNCOMMENT WHEN BOIDSJOBS DONE
  //     }
  // }

  public void RespawnBoids()
  {
    // TODO: KILL BOIDS THEN RESPAWN
  }

  public void KillBoids()
  {
    // TODO: KILL BOIDS
  }

  public void ToggleCam()
  {
    if (mainVCam.gameObject.activeInHierarchy)
      WatchFishCam();
    else
      WatchMainCam();
  }

  public void WatchMainCam()
  {
    mainVCam.gameObject.SetActive(true);
    fishVCam.gameObject.SetActive(false);
  }

  public void WatchFishCam()
  {
    if (fishVCam)
    {
      mainVCam.gameObject.SetActive(false);
      fishVCam.gameObject.SetActive(true);
      Boid followBoid = FindObjectOfType<Boid>();
      fishVCam.Follow = followBoid.transform;
      fishVCam.LookAt = followBoid.transform;
    }
  }
}