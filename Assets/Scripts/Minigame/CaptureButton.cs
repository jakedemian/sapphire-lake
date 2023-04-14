using UnityEngine;

public class CaptureButton : AbilityButton {
    [SerializeField] private GameObject captureButtonGlow;

    protected override void Awake() {
        base.Awake();
        EventManager.i.onFishAwaitingCapture += OnFishAwaitingCapture;
        EventManager.i.onFishCaptureOutOfTime += OnFishNotAwaitingCapture;
        EventManager.i.onFishCaptured += OnFishNotAwaitingCapture;
    }

    private void OnDestroy() {
        EventManager.i.onFishAwaitingCapture -= OnFishAwaitingCapture;
        EventManager.i.onFishCaptureOutOfTime -= OnFishNotAwaitingCapture;
        EventManager.i.onFishCaptured -= OnFishNotAwaitingCapture;
    }

    protected override void SetInitalButtonState() {
        base.Disable();
    }

    private void OnFishAwaitingCapture() {
        base.Enable();
        captureButtonGlow.SetActive(true);
    }

    private void OnFishNotAwaitingCapture() {
        captureButtonGlow.SetActive(false);
    }
}

