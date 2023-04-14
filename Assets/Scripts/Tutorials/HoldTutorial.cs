using UnityEngine;

public class HoldTutorial : MonoBehaviour {
    [SerializeField] private TutorialModalUI holdModal;

    private void Awake() {
        EventManager.i.onFishCaptured += OnFishCaptured;
        EventManager.i.onFishEscaped += OnFishEscaped;
        EventManager.i.onFishCaptureOutOfTime += OnFishCaptureOutOfTime;
        EventManager.i.onFishEnergizeStart += OnFishEnergizeStart;
        EventManager.i.onFishEnergizeEnd += OnFishEnergizeEnd;
        EventManager.i.onTutorialStepChange += OnTutorialStepChange;
    }

    private void OnDestroy() {
        EventManager.i.onFishCaptured -= OnFishCaptured;
        EventManager.i.onFishEscaped -= OnFishEscaped;
        EventManager.i.onFishCaptureOutOfTime -= OnFishCaptureOutOfTime;
        EventManager.i.onFishEnergizeStart -= OnFishEnergizeStart;
        EventManager.i.onFishEnergizeEnd -= OnFishEnergizeEnd;
        EventManager.i.onTutorialStepChange -= OnTutorialStepChange;
    }

    private void OnTutorialStepChange(int step) {
        if (step > Consts.TUTORIAL_STEP_HOLD_AND_CAPTURE) {
            Destroy(this);
        }
    }

    private void HideHoldModal() {
        if (holdModal.isActive) {
            holdModal.Hide();
        }
    }

    private void OnFishCaptured() {
        HideHoldModal();
    }

    private void OnFishCaptureOutOfTime() {
        HideHoldModal();
    }

    private void OnFishEscaped() {
        HideHoldModal();
    }

    private void OnFishEnergizeStart() {
        if (!holdModal.isActive) {
            holdModal.Show();
        }
    }

    private void OnFishEnergizeEnd() {
        HideHoldModal();
    }
}
