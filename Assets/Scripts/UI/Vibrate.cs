using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrate : MonoBehaviour {
    [SerializeField] private float vibrationStrength;
    [SerializeField] private float secondsPerVibration;

    private RectTransform rt;
    private Vector2 basePosition;
    private bool isVibrating;

    private void Awake() {
        rt = GetComponent<RectTransform>();
        basePosition = rt.anchoredPosition;
        EventManager.i.onFishAwaitingCapture += OnFishAwaitingCapture;
        EventManager.i.onFishEscaped += OnMinigameOver;
        EventManager.i.onFishCaptureOutOfTime += OnMinigameOver;
        EventManager.i.onFishCaptured += OnMinigameOver;
    }

    private void OnDestroy() {
        EventManager.i.onFishAwaitingCapture -= OnFishAwaitingCapture;
        EventManager.i.onFishEscaped -= OnMinigameOver;
        EventManager.i.onFishCaptureOutOfTime -= OnMinigameOver;
        EventManager.i.onFishCaptured -= OnMinigameOver;
    }

    private IEnumerator DoVibrate() {
        while (isVibrating) {
            rt.anchoredPosition = basePosition + ((Vector2)Random.insideUnitCircle.normalized * vibrationStrength);
            yield return new WaitForSeconds(secondsPerVibration);
        }
        yield return null;
    }

    private void OnFishAwaitingCapture() {
        isVibrating = true;
        StartCoroutine(DoVibrate());
    }

    private void OnMinigameOver() {
        isVibrating = false;
    }
}
