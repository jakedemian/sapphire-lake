using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour {
    public float moveSpeed;
    public float turnSpeed;
    public float decelerationFactor;
    public float slowVelocityThreshold;
    public float stopVelocityThreshold;
    public float wakeSpawnDelaySeconds;
    public float wakeSpawnSpeedIncreaseFactor;
    public Transform wakeSpawnPoint;
    public Transform wakeParentTransform;
    public GameObject wakePrefab;

    private Rigidbody2D rb;
    private Animator animator;
    private ThrowBobber throwBobber;
    private Vector2 input;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        throwBobber = GetComponent<ThrowBobber>();
        StartCoroutine(WakeSpawner());
    }

    void Update() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        animator.SetBool("isMoving", verticalInput > 0f && !DayNightCycle.i.timePaused);
        if (verticalInput <= 0f) {
            if (rb.velocity.magnitude < stopVelocityThreshold) {
                rb.velocity = Vector2.zero;
            } else {
                Vector2 velocity = rb.velocity;
                float slowFactor = decelerationFactor;

                if (velocity.magnitude < slowVelocityThreshold) {
                    slowFactor *= 3;
                }

                rb.velocity -= new Vector2(velocity.x * Time.deltaTime * slowFactor, velocity.y * Time.deltaTime * decelerationFactor);
            }
        }

        if (throwBobber.IsMovementLocked() || DayNightCycle.i.timePaused) return;

        float rotation = -horizontalInput * turnSpeed * Time.deltaTime;
        rb.rotation += rotation;

        Vector2 direction = transform.up;
        rb.velocity += direction * moveSpeed * Time.deltaTime * verticalInput;
    }

    private IEnumerator WakeSpawner() {
        float timer = 0f;
        while (true) {
            float verticalInput = Input.GetAxisRaw("Vertical");
            if (verticalInput > 0f) {
                timer += Time.deltaTime * (wakeSpawnSpeedIncreaseFactor * rb.velocity.magnitude);
                if (timer > wakeSpawnDelaySeconds) {
                    // spawn a wake
                    GameObject go = Instantiate(wakePrefab, wakeSpawnPoint.position, transform.rotation);
                    go.transform.parent = wakeParentTransform;
                    timer = 0f;
                }
            }

            yield return null;
        }
    }
}
