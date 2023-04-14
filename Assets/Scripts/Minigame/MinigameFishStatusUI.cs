using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameFishStatusUI : MonoBehaviour {
    public RectTransform fatigue;
    public RectTransform fatigueAnchorPoint;

    public RectTransform energized;
    public RectTransform energizedAnchorPoint;

    public AnimationCurve animationCurve;
    public float animationSpeed;
    public float animationStrength;

    private MinigameFish fish;
    private float animationTimer;

    private void Awake() {
        fish = GetComponent<MinigameFish>();
    }

    private void Update() {
        if (fish.isFatigued && !fatigue.gameObject.activeInHierarchy) {
            fatigue.gameObject.SetActive(true);
            StartCoroutine(Animate(fatigue, fatigueAnchorPoint));
        } else if (!fish.isFatigued && fatigue.gameObject.activeInHierarchy) {
            fatigue.gameObject.SetActive(false);
        }

        if (fish.isEnergized && !energized.gameObject.activeInHierarchy) {
            energized.gameObject.SetActive(true);
            StartCoroutine(Animate(energized, energizedAnchorPoint));
        } else if (!fish.isEnergized && energized.gameObject.activeInHierarchy) {
            energized.gameObject.SetActive(false);
        }
    }

    private IEnumerator Animate(RectTransform transform, RectTransform anchor) {
        Vector2 direction = Vector2.right;
        float switchDirectionTime = animationSpeed;
        float timer = 0f;

        while (fish.isFatigued) {
            while (timer < switchDirectionTime) {
                timer += Time.deltaTime;
                float percent = timer / switchDirectionTime;
                transform.localPosition = (Vector2)anchor.localPosition +
                    direction * animationStrength * animationCurve.Evaluate(percent);
                yield return null;
            }
            timer = 0f;
            direction = -direction;
            yield return null;
        }
    }
}
