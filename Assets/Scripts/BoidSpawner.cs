using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoidSpawner : MonoBehaviour {
    public static Boid[] population;
    [SerializeField] private int totalBoids = 5000;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private GameObject fishPrefab;
    private float boundaryRadius = 10;
    private Vector3 boidLocation;

    private void Start() {
        if (!spawnLocation)
            spawnLocation = this.transform;
        else
            boundaryRadius = spawnLocation.localScale.x/2;
        
        population = new Boid[totalBoids];
        Boid newBoid;
        for (int i=0; i<totalBoids; i++) {
            boidLocation = Random.insideUnitSphere.normalized * Random.Range(0, boundaryRadius * 0.9f);
            newBoid = Instantiate(fishPrefab, boidLocation, Quaternion.identity, this.transform).GetComponent<Boid>();
            newBoid.Initialize(spawnLocation.position, boundaryRadius);
            population.Append(newBoid);
        }
        Debug.Log("Total boids: " + population.Length);
    }
}
