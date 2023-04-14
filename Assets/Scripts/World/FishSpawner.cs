using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour {
    public GameObject boat;
    public GameObject fishPrefab;
    public Transform fishPool;
    public float fishSpawnCooldown;
    public float fishSpawnTickChance;
    public float distanceTraveledThreshold;
    public float spawnDistanceFromBoat;
    public float spawnVarianceRadius;
    public float renderDistanceThreshold;
    public int maxNearbyFish;
    public float nearbyFishCheckDistance;

    private float distanceSinceLastTick;
    private Vector2 positionLastFrame;
    private bool tutorialActive => Globals.ValueInstance.tutorialActive;

    private void Awake() {
        positionLastFrame = boat.transform.position;
        StartCoroutine(SpawnFish());
        StartCoroutine(UpdateRendering());
    }

    private bool MinigameActive() {
        if (MinigameController.i == null) {
            return false;
        }

        return MinigameController.i.GetState() == MinigameState.Active;
    }

    private IEnumerator SpawnFish() {
        while (true) {
            if (MinigameActive() || tutorialActive) {
                yield return new WaitForSeconds(1);
                continue;
            }

            Vector2 currentPlayerPosition = boat.transform.position;
            distanceSinceLastTick += Vector2.Distance(currentPlayerPosition, positionLastFrame);
            positionLastFrame = currentPlayerPosition;

            if (distanceSinceLastTick <= distanceTraveledThreshold) {
                yield return new WaitForEndOfFrame();
                continue;
            }

            distanceSinceLastTick = 0f;

            // try a spawn
            float roll = Random.Range(0f, 1f);
            if (roll >= fishSpawnTickChance) {
                yield return new WaitForEndOfFrame();
                continue;
            }

            Vector2 direction = boat.transform.up.normalized;
            Vector2 spawnCenter = currentPlayerPosition + (direction * spawnDistanceFromBoat);
            Vector2 spawnPoint = spawnCenter + (Random.insideUnitCircle * spawnVarianceRadius);

            const int MAX_TRIES = 15;
            for (int i = 0; i < MAX_TRIES; i++) {
                var hit = Physics2D.Raycast(spawnPoint, Vector2.zero);
                if (hit.collider != null) {
                    spawnPoint = spawnCenter + (Random.insideUnitCircle * spawnVarianceRadius);
                    continue;
                }

                if (!IsValidNearbyFishCount()) {
                    break;
                }

                GameObject go = Instantiate(
                    fishPrefab,
                    spawnPoint,
                    Quaternion.identity
                );
                go.transform.SetParent(fishPool);

                yield return new WaitForSeconds(fishSpawnCooldown);
                break;
            }
            yield return null;
        }
    }

    private bool IsValidNearbyFishCount() {
        int nearbyFishCount = 0;
        Vector2 boatPosition = boat.transform.position;

        for (int i = 0; i < fishPool.childCount; i++) {
            Transform child = fishPool.GetChild(i);
            if (Vector2.Distance(child.position, boatPosition) <= nearbyFishCheckDistance) {
                nearbyFishCount++;
            }

            if (nearbyFishCount > maxNearbyFish) {
                return false;
            }
        }

        return true;
    }

    private IEnumerator UpdateRendering() {
        while (true) {
            Vector2 boatPosition = boat.transform.position;
            for (int i = 0; i < fishPool.childCount; i++) {
                Transform child = fishPool.GetChild(i);
                float distanceFromPlayer = Vector2.Distance(child.position, boatPosition);
                if (distanceFromPlayer >= renderDistanceThreshold && child.gameObject.activeInHierarchy) {
                    child.gameObject.SetActive(false);
                } else if (distanceFromPlayer < renderDistanceThreshold && !child.gameObject.activeInHierarchy) {
                    child.gameObject.SetActive(true);
                }
            }

            yield return new WaitForSeconds(1);
        }
    }

}
