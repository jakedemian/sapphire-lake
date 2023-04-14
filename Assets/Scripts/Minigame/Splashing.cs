using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Splashing : MonoBehaviour {
    public Sprite[] sprites;

    public float animationSpeed;

    private bool isAnimating;
    private Image image;
    private int index = 0;
    private float timer = 0;

    private readonly Color transparent = new Color(1f, 1f, 1f, 0f);
    private readonly Color opaque = new Color(1f, 1f, 1f, 1f);

    void Awake() {
        image = GetComponent<Image>();
        image.sprite = sprites[0];
        image.color = transparent;

        EventManager.i.onFishAwaitingCapture += OnFishAwaitingCapture;
        EventManager.i.onFishCaptureOutOfTime += OnFishCaptureOutOfTime;
        EventManager.i.onFishCaptured += OnFishCaptured;
    }

    private void Update() {
        if (!isAnimating) return;

        timer += Time.deltaTime;
        if (timer > animationSpeed) {
            timer = 0f;
            index = index == sprites.Length - 1 ? 0 : index + 1;
            image.sprite = sprites[index];
        }
    }

    private void OnFishAwaitingCapture() {
        AudioManager.i.PlaySplashingSound();
        image.color = opaque;
        isAnimating = true;
        timer = 0f;
    }

    private void OnFishCaptureOutOfTime() {
        image.color = transparent;
        isAnimating = false;
    }

    private void OnFishCaptured() {
        image.color = transparent;
        isAnimating = false;
    }
}
