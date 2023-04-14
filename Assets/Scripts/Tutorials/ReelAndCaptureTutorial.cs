using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelAndCaptureTutorial : MonoBehaviour {
    [SerializeField] private TutorialModalUI reelModal;
    [SerializeField] private TutorialModalUI captureModal;

    private void Awake() {
        EventManager.i.onTutorialStepChange += OnTutorialStepChange;
        EventManager.i.onFishCaptured += OnFishCaptured;
        EventManager.i.onFishEscaped += OnFishEscaped;
        EventManager.i.onFishCaptureOutOfTime += OnFishCaptureOutOfTime;
        EventManager.i.onFishAwaitingCapture += OnFishAwaitingCapture;
    }

    private void OnDestroy() {
        EventManager.i.onFishCaptured -= OnFishCaptured;
        EventManager.i.onFishEscaped -= OnFishEscaped;
        EventManager.i.onFishCaptureOutOfTime -= OnFishCaptureOutOfTime;
        EventManager.i.onTutorialStepChange -= OnTutorialStepChange;
        EventManager.i.onFishAwaitingCapture -= OnFishAwaitingCapture;
    }

    private void OnTutorialStepChange(int step) {
        if (step == Consts.TUTORIAL_STEP_REEL_AND_CAPTURE) {
            reelModal.Show();
        } else if (step > Consts.TUTORIAL_STEP_REEL_AND_CAPTURE) {
            Destroy(this);
        }
    }

    private void HideCaptureModal() {
        captureModal.Hide();
    }

    private void OnFishCaptured() {
        HideCaptureModal();
    }

    private void OnFishCaptureOutOfTime() {
        HideCaptureModal();
    }

    private void OnFishEscaped() {
        reelModal.Hide();
    }

    private void OnFishAwaitingCapture() {
        captureModal.Show();
        reelModal.Hide();
    }
}
