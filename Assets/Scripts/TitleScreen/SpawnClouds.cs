using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnClouds : MonoBehaviour {
    public GameObject cloudPrefab;

    private float cloudSpawnDistance => Mathf.Abs(transform.position.x);
    private GameObject mostRecentSpawn;

    private void Awake() {
        SpawnCloud();
    }

    private void Update() {
        if (mostRecentSpawn.transform.position.x <= 0f) {
            SpawnCloud();
        }
    }

    private void SpawnCloud() {
        mostRecentSpawn = Instantiate(cloudPrefab, transform.position, Quaternion.identity);
        mostRecentSpawn.transform.parent = transform;
    }
}
