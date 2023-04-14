using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameFish : MonoBehaviour {
    public RectTransform target;
    public RectTransform playerTargetPoint;
    public RectTransform caughtAnimationTargetPoint;

    public FishData fishData { get; private set; }

    public float caughtAnimationSeconds;
    public float animationHeightFactor;
    public AnimationCurve caughtAnimationCurve;

    private RectTransform rectTransform;
    private Rigidbody2D rb;
    private float recoveryTimer;
    private bool isBeingReeled;
    private bool isBeingYanked;
    public bool isStopped { get; private set; }
    public bool isFatigued { get; private set; }
    public bool isEnergized { get; private set; }
    public Coroutine fatigueCoroutine;
    public Coroutine energizedCoroutine;

    private const float EFFECT_TICK_RATE_SECONDS = 1f;
    private float currentFishStrength => isEnergized ?
        fishData.energizedFishStrength : fishData.passiveStrength;
    private Vector2 playerTargetToFish => (
        transform.position - playerTargetPoint.transform.position
    ).normalized;
    private Vector2 reelVelocity => -playerTargetToFish * fishData.reelVulnerability * CanvasScalerInfo.i.currentScaleFactor;
    private Vector2 yankVelocity => -playerTargetToFish * fishData.yankVulnerability * CanvasScalerInfo.i.currentScaleFactor;
    private Vector2 fishVelocity => playerTargetToFish * currentFishStrength * CanvasScalerInfo.i.currentScaleFactor;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rectTransform = GetComponent<RectTransform>();

        EventManager.i.onMinigameInteractive += OnMinigameInteractive;
    }

    private void OnDestroy() {
        EventManager.i.onMinigameInteractive -= OnMinigameInteractive;
    }

    private void Update() {
        if (isStopped) return;

        if (isBeingReeled) {
            recoveryTimer += Time.deltaTime;
            float t = recoveryTimer / fishData.reelRecoverySeconds;
            rb.velocity = Vector2.Lerp(reelVelocity, fishVelocity, t);

            if (t >= 1f) {
                SetFishVelocity();
                recoveryTimer = 0f;
                isBeingReeled = false;
            }
        }

        if (isBeingYanked) {
            recoveryTimer += Time.deltaTime;
            float t = recoveryTimer / fishData.yankRecoverySeconds;
            rb.velocity = Vector2.Lerp(yankVelocity, fishVelocity, t);

            if (t >= 1f) {
                SetFishVelocity();
                recoveryTimer = 0f;
                isBeingYanked = false;
            }
        }
    }

    public void Initialize(FishData _fishData) {
        fishData = _fishData;
        transform.position = playerTargetPoint.transform.position + (
            (target.transform.position - playerTargetPoint.transform.position) *
            (1 - fishData.startingPercentage)
        );
        transform.localScale = Vector3.one;
    }

    public void OnMinigameInteractive() {
        fatigueCoroutine = StartCoroutine(ManageFatigue());
        energizedCoroutine = StartCoroutine(ManageEnergized());

        isStopped = false;
        isBeingYanked = false;
        isBeingReeled = false;
        recoveryTimer = 0f;

        SetFishVelocity();
    }

    private void OnDisable() {
        if (fatigueCoroutine != null) {
            StopCoroutine(fatigueCoroutine);
        }
        if (energizedCoroutine != null) {
            StopCoroutine(energizedCoroutine);
        }
        isFatigued = false;
        isEnergized = false;
    }

    private void SetFishVelocity() {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        rb.velocity = fishVelocity;
    }

    public void Reel() {
        if (isStopped) return;

        recoveryTimer = 0f;
        rb.velocity = reelVelocity;
        isBeingReeled = true;
    }

    public void Yank() {
        if (isStopped) return;

        // override possible reel state
        isBeingReeled = false;
        recoveryTimer = 0f;

        // update yank state
        isFatigued = false;
        EventManager.i.e_FishFatigueEnd();
        isBeingYanked = true;

        rb.velocity = yankVelocity;
        StopCoroutine(fatigueCoroutine);
        fatigueCoroutine = StartCoroutine(ManageFatigue());
    }

    public void Hold() {
        if (isStopped) return;

        if (isEnergized) {
            StopCoroutine(energizedCoroutine);
            energizedCoroutine = StartCoroutine(ManageEnergized());

            StartCoroutine(DoHold());
        }
    }

    public void Stop() {
        isStopped = true;
        isBeingReeled = false;
        isFatigued = false;
        isEnergized = false;
        rb.velocity = Vector2.zero;
    }

    public float GetDistanceToTarget() {
        return Vector2.Distance(transform.position, target.transform.position);
    }

    private IEnumerator ManageFatigue() {
        while (true) {
            yield return new WaitForSeconds(EFFECT_TICK_RATE_SECONDS);

            // don't fatigue while awaiting capture
            if (isStopped) {
                continue;
            }

            float rng = UnityEngine.Random.Range(0f, 1f);
            if (rng < fishData.fatigueChanceOnTick) {
                if (isEnergized) continue; // don't energize and fatigue at same time

                isFatigued = true;
                EventManager.i.e_FishFatigueStart();
                AudioManager.i.PlayFatigueSound();

                yield return new WaitForSeconds(fishData.fatigueEffectLengthSeconds);

                isFatigued = false;
                EventManager.i.e_FishFatigueEnd();
            }
        }
    }

    private IEnumerator ManageEnergized() {
        while (true) {
            yield return new WaitForSeconds(EFFECT_TICK_RATE_SECONDS);

            if (isStopped) {
                continue;
            }

            float rng = UnityEngine.Random.Range(0f, 1f);
            if (rng < fishData.energizedChanceOnTick) {
                if (isFatigued) continue;

                isEnergized = true;
                EventManager.i.e_FishEnergizeStart();

                if (!isBeingReeled && !isBeingYanked) {
                    SetFishVelocity();
                }

                AudioManager.i.PlayEnergizedSound();
                yield return new WaitForSeconds(fishData.energizedEffectLengthSeconds);
                isEnergized = false;
                EventManager.i.e_FishEnergizeEnd();
            }
        }
    }

    public IEnumerator DoCaughtAnimation(Action callback) {
        Vector2 startPosition = playerTargetPoint.anchoredPosition;
        Vector2 endPosition = caughtAnimationTargetPoint.anchoredPosition;

        float timer = 0f;
        while (timer < caughtAnimationSeconds) {
            timer += Time.deltaTime;
            float t = timer / caughtAnimationSeconds;
            rectTransform.anchoredPosition =
                Vector2.Lerp(startPosition, endPosition, t) +
                new Vector2(0f, caughtAnimationCurve.Evaluate(t) * animationHeightFactor);
            rectTransform.localScale = Vector2.Lerp(Vector2.one, Vector2.zero, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        callback();
    }

    private IEnumerator DoHold() {
        Stop();
        EventManager.i.e_FishEnergizeEnd();
        yield return new WaitForSeconds(fishData.holdTime);
        isStopped = false;
        SetFishVelocity();
        MinigameAudio.i.Unpause();
    }
}
