using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
    public float speed = 5;
    private Vector3 direction;

    private void Start() {
        direction = Random.insideUnitSphere.normalized;
        transform.forward = direction;
    }
    
    private void Update() {
        transform.position += transform.forward * speed * Time.deltaTime;
        
    }
}
