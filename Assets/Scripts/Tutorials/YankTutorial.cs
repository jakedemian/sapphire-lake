using UnityEngine;

public class YankTutorial : MonoBehaviour {
    [SerializeField] private TutorialModalUI yankModal;

    private void Awake() {
        EventManager.i.onFishCaptured += OnFishCaptured;
        EventManager.i.onFishEscaped += OnFishEscaped;
        EventManager.i.onFishCaptureOutOfTime += OnFishCaptureOutOfTime;
        EventManager.i.onFishFatigueStart += OnFishFatigueStart;
        EventManager.i.onFishFatigueEnd += OnFishFatigueEnd;
        EventManager.i.onTutorialStepChange += OnTutorialStepChange;
    }

    private void OnDestroy() {
        EventManager.i.onFishCaptured -= OnFishCaptured;
        EventManager.i.onFishEscaped -= OnFishEscaped;
        EventManager.i.onFishCaptureOutOfTime -= OnFishCaptureOutOfTime;
        EventManager.i.onFishFatigueStart -= OnFishFatigueStart;
        EventManager.i.onFishFatigueEnd -= OnFishFatigueEnd;
        EventManager.i.onTutorialStepChange -= OnTutorialStepChange;
    }

    private void OnTutorialStepChange(int step) {
        if (step > Consts.TUTORIAL_STEP_YANK_AND_CAPTURE) {
            Destroy(this);
        }
    }

    private void HideYankModal() {
        if (yankModal.isActive) {
            yankModal.Hide();
        }
    }

    private void OnFishCaptured() {
        HideYankModal();
    }

    private void OnFishCaptureOutOfTime() {
        HideYankModal();
    }

    private void OnFishEscaped() {
        HideYankModal();
    }

    private void OnFishFatigueStart() {
        if (!yankModal.isActive) {
            yankModal.Show();
        }
    }

    private void OnFishFatigueEnd() {
        HideYankModal();
    }
}
