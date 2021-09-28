using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoidSpawner : MonoBehaviour {
    public static Boid[] population;
    [SerializeField] private int totalBoids = 5000;
    [SerializeField] private float spawnRadius = 10;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private GameObject fishPrefab;
    private Vector3 boidLocation;

    private void Start() {
        if (!spawnLocation)
            spawnLocation = this.transform;
        population = new Boid[totalBoids];
        Boid newBoid;
        for (int i=0; i<totalBoids; i++) {
            boidLocation = new Vector3(Random.Range(spawnLocation.position.x - spawnRadius, spawnLocation.position.x + spawnRadius), 
                                        Random.Range(spawnLocation.position.y - spawnRadius, spawnLocation.position.y + spawnRadius), 
                                        Random.Range(spawnLocation.position.z - spawnRadius, spawnLocation.position.z + spawnRadius) );
            newBoid = Instantiate(fishPrefab, boidLocation, Quaternion.identity, this.transform).GetComponent<Boid>();
            population.Append(newBoid);
        }
        Debug.Log("Total boids: " + population.Length);
    }
}
