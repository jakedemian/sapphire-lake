using System;
using System.Collections;
using UnityEngine;

public abstract class UIModal : MonoBehaviour {
    public float inAnimationSeconds;
    public float outAnimationSeconds;
    public float inBufferTime;
    public float outBufferTime;
    public AnimationCurve inOutAnimationCurve;
    public Vector2 onScreenAnchoredPosition;
    public Vector2 offScreenAnchoredPosition;
    public float shakeTimeSeconds;
    public float shakeSpeed;
    public float shakeStrength;

    public bool isActive { get; private set; }
    public bool isAnimating { get; private set; }
    private RectTransform rt;

    protected void Awake() {
        rt = GetComponent<RectTransform>();
    }

    public void Shake(Action callback = null) {
        StartCoroutine(
            DoShake(
                shakeTimeSeconds,
                shakeStrength,
                shakeSpeed,
                false,
                callback
            )
        );
    }

    public void ShakeAndHide(Action callback = null) {
        StartCoroutine(DoShake(shakeTimeSeconds, shakeStrength, shakeSpeed, true, () => {
            StartCoroutine(DoAnimation(
                onScreenAnchoredPosition,
                offScreenAnchoredPosition,
                outAnimationSeconds,
                outBufferTime,
                false,
                callback
            ));
        }));
    }

    public virtual void Show(Action callback = null) {
        isActive = true;
        StartCoroutine(
            DoAnimation(
                offScreenAnchoredPosition,
                onScreenAnchoredPosition,
                inAnimationSeconds,
                inBufferTime,
                false,
                callback
            )
        );
    }

    public virtual void Hide(Action callback = null) {
        isActive = false;
        StartCoroutine(
            DoAnimation(
                onScreenAnchoredPosition,
                offScreenAnchoredPosition,
                outAnimationSeconds,
                outBufferTime,
                false,
                callback
            )
        );
    }

    private IEnumerator DoAnimation(Vector2 start, Vector2 end, float seconds, float bufferTime, bool hasFollowup = false, Action callback = null) {
        isAnimating = true;
        float timer = 0f;
        while (timer < seconds) {
            timer += Time.deltaTime;
            float t = timer / seconds;
            rt.anchoredPosition = start + ((end - start) *
                new Vector2(inOutAnimationCurve.Evaluate(t), inOutAnimationCurve.Evaluate(t)));
            yield return null;
        }

        if (bufferTime > 0f) {
            yield return new WaitForSeconds(bufferTime);
        }

        if (!hasFollowup) {
            isAnimating = false;
        }

        if (callback != null) {
            callback();
        }
    }

    private IEnumerator DoShake(float seconds, float strength, float speed, bool hasFollowup = false, Action callback = null) {
        isAnimating = true;
        float xPosition = rt.anchoredPosition.x;
        float shakeTimer = 0f;
        while (shakeTimer < seconds) {
            shakeTimer += Time.deltaTime;
            float currentX = xPosition + (Mathf.Sin(shakeTimer * speed) * strength);
            rt.anchoredPosition = new Vector2(currentX, rt.anchoredPosition.y);
            yield return null;
        }

        if (!hasFollowup) {
            isAnimating = false;
        }

        if (callback != null) {
            callback();
        }
    }
}
