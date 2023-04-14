using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreen : MonoBehaviour {
    public float fadeToBlackSeconds;
    public float fadeToTransparentSeconds;
    public float fadeToTransparentDelaySeconds;

    private SpriteRenderer sr;
    private readonly Color transparent = new Color(0, 0, 0, 0);
    private readonly Color opaque = new Color(0, 0, 0, 1);

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        EventManager.i.onDayStart += OnDayStart;
        EventManager.i.onDayEnd += OnDayEnd;
    }

    private void OnDayStart() {
        sr.color = opaque;
        StartCoroutine(FadeToTransparent());
    }

    private void OnDayEnd() {
        StartCoroutine(FadeToBlack());
    }

    private IEnumerator FadeToTransparent() {
        yield return new WaitForSeconds(fadeToTransparentDelaySeconds);

        float timer = 0f;
        while (timer < fadeToTransparentSeconds) {
            timer += Time.deltaTime;
            sr.color = Color.Lerp(opaque, transparent, timer / fadeToTransparentSeconds);
            yield return null;
        }
        EventManager.i.e_BlackScreenTransparent();
    }

    private IEnumerator FadeToBlack() {
        float timer = 0f;
        while (timer < fadeToBlackSeconds) {
            timer += Time.deltaTime;
            sr.color = Color.Lerp(transparent, opaque, timer / fadeToBlackSeconds);
            yield return null;
        }
        EventManager.i.e_BlackScreenOpaque();
    }
}
