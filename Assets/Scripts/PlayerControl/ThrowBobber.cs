using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBobber : MonoBehaviour {
    public GameObject bobberPrefab;
    public BobberAoe bobberAoe;
    public float throwSeconds;
    public AnimationCurve throwSpeed;
    public AnimationCurve arc;
    public float arcMultiplier;
    public float maxBobberRange; // = 1.75

    private Rigidbody2D rb;
    private GameObject bobber;
    private bool isAnimatingThrow;
    private Vector2 targetPosition;
    private float timer;
    private bool isAiming;

    private bool isFishHooked;

    private void Awake() {
        EventManager.i.onMinigameStart += OnMinigameStart;
        rb = GetComponent<Rigidbody2D>();
        bobberAoe.SetMaxRange(maxBobberRange);
        EventManager.i.onFishHooked += OnFishHooked;
        EventManager.i.onDayEnd += OnDayEnd;
    }

    private void OnDestroy() {
        EventManager.i.onFishHooked -= OnFishHooked;
        EventManager.i.onDayEnd -= OnDayEnd;
    }

    private void Update() {
        if (DayNightCycle.i.timePaused || PauseController.i.isPaused) return;

        if (LeftClickPressed() && bobber == null && rb.velocity == Vector2.zero) {
            bobberAoe.Show();
            isAiming = true;
        }

        if (RightClickPressed() && bobberAoe.isShowing) {
            bobberAoe.Hide();
            isAiming = false;
        }

        if (LeftClickReleased() && bobber == null && isAiming) {
            bobberAoe.Hide();
            isAiming = false;
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
            if (hit.collider == null) {
                float distanceToTarget = Vector2.Distance(mouseWorldPosition, transform.position);

                if (distanceToTarget > maxBobberRange) {
                    Vector2 directionToTarget = (mouseWorldPosition - (Vector2)transform.position).normalized;
                    targetPosition = (Vector2)transform.position + (directionToTarget * maxBobberRange);
                } else {
                    targetPosition = mouseWorldPosition;
                }

                bobber = Instantiate(bobberPrefab, transform.position, Quaternion.identity);
                isAnimatingThrow = true;
                AudioManager.i.PlayCastSound();
            } else {
                AudioManager.i.PlayOnCooldownSound();
            }
        }

        if (LeftClickReleased() && bobber != null && !isAnimatingThrow && !isFishHooked) {
            Destroy(bobber);
            bobber = null;
            EventManager.i.e_BobberPickedUp();
        }

        if (isAnimatingThrow) {
            timer += Time.deltaTime;
            bobber.transform.position =
                Vector2.Lerp(transform.position, targetPosition, throwSpeed.Evaluate(timer / throwSeconds)) +
                new Vector2(0f, arc.Evaluate(timer / throwSeconds) * arcMultiplier);

            if (timer >= throwSeconds) {
                EventManager.i.e_BobberLanded(targetPosition);
                AudioManager.i.PlayBobberHitWaterSound();
                bobber.transform.GetChild(0).gameObject.SetActive(true); // moving this into bobber
                isAnimatingThrow = false;
                timer = 0f;
            }
        }
    }

    private void OnFishHooked(GameObject fish) {
        isFishHooked = true;
    }

    private bool LeftClickPressed() {
        return Input.GetMouseButtonDown(0);
    }

    private bool LeftClickReleased() {
        return Input.GetMouseButtonUp(0);
    }

    private bool RightClickPressed() {
        return Input.GetMouseButtonUp(1);
    }

    private void OnMinigameStart() {
        isFishHooked = false;
        if (bobber != null) {
            Destroy(bobber);
            bobber = null;
        }
    }

    public bool IsMovementLocked() {
        return isAiming || isAnimatingThrow || bobber != null;
    }

    private void OnDayEnd() {
        isFishHooked = false;
        if (bobberAoe.isShowing) {
            bobberAoe.Hide();
        }
        isAiming = false;
        Destroy(bobber);
        bobber = null;
        isAnimatingThrow = false;
    }
}
