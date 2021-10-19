using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour {
    [SerializeField] private int totalBoids = 5000;
    [Tooltip("Must check or uncheck this before pressing Play in Unity Editor")]
    [SerializeField] private bool useJobSystem = false;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private GameObject fishPrefab;
    private float boundaryRadius = 10;
    private Vector3 boidLocation;

    private void Awake() {
        if (!spawnLocation)
            spawnLocation = this.transform;
        else
            boundaryRadius = spawnLocation.localScale.x/2;
        
        if (useJobSystem) {
            SpawnBoidsJobs(totalBoids);
        } else {
            SpawnBoids(totalBoids);
        }
    }

    private void Start() {
        Debug.Log("Total boids: " + Boid.population.Count);
    }

    public void SpawnBoids(int number) {
        Boid newBoid;
        for (int i=0; i<totalBoids; i++) {
            boidLocation = Random.insideUnitSphere.normalized * Random.Range(0, boundaryRadius * 0.9f);
            newBoid = Instantiate(fishPrefab, boidLocation, Quaternion.identity, this.transform).GetComponent<Boid>();
            newBoid.SetBoundarySphere(spawnLocation.position, boundaryRadius);
        }
    }

    public void SpawnBoidsJobs(int number) {
        Debug.Log("Job system ain't ready yet!");
    }
}
