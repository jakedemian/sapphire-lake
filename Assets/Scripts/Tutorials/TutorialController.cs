using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour {
    public static TutorialController i;

    [SerializeField] private GameObject tutorialPufftailPrefab;
    [SerializeField] private GameObject tutorialLimeBassPrefab;
    [SerializeField] private GameObject tutorialJadeAngelfishPrefab;
    [SerializeField] private GameObject tutorialDebugPanel;
    [SerializeField] private GameObject tutorialModalsContainer;
    [SerializeField] private GameObject tutorialEntitiesContainer;
    [SerializeField] private GameObject tutorialBarrier1;
    [SerializeField] private GameObject tutorialBarrier2;
    [SerializeField] private GameObject tutorialBarrier3;
    [SerializeField] private HideAbilityButtonDuringTutorial[] hideAbilityButtonScripts;
    [SerializeField] private TextMeshProUGUI stepText;
    [SerializeField] private bool debug;

    public int tutorialStep { get; private set; }

    private Vector2 lastFishHookedPosition;

    private void Awake() {
        i ??= this;
        if (debug) {
            tutorialDebugPanel.SetActive(true);
            UpdateStepText();
        }

        EventManager.i.onTriggerActivated += OnTriggerActivated;
        EventManager.i.onMinigameStart += OnMinigameStart;
        EventManager.i.onMinigameBeginAnimatingOffscreen += OnMinigameBeginAnimatingOffscreen;
        EventManager.i.onGameTimeResumed += OnGameTimeResumed;
        EventManager.i.onSuccessModalClosed += OnSuccessModalClosed;
        EventManager.i.onFishEscaped += OnFishEscaped;
        EventManager.i.onFishCaptureOutOfTime += OnFishCaptureOutOfTime;
        EventManager.i.onFishHooked += OnFishHooked;
        EventManager.i.onTutorialModalHidden += OnTutorialModalHidden;
    }

    private void OnDestroy() {
        EventManager.i.onTriggerActivated -= OnTriggerActivated;
        EventManager.i.onMinigameStart -= OnMinigameStart;
        EventManager.i.onMinigameBeginAnimatingOffscreen -= OnMinigameBeginAnimatingOffscreen;
        EventManager.i.onGameTimeResumed -= OnGameTimeResumed;
        EventManager.i.onSuccessModalClosed -= OnSuccessModalClosed;
        EventManager.i.onFishEscaped -= OnFishEscaped;
        EventManager.i.onFishCaptureOutOfTime -= OnFishCaptureOutOfTime;
        EventManager.i.onFishHooked -= OnFishHooked;
        EventManager.i.onTutorialModalHidden -= OnTutorialModalHidden;
    }

    private void Start() {
        // this is in Start() so GameController can delete this in it's Awake if
        // not Day 1 or if tutorials disabled
        Globals.ValueInstance.tutorialActive = true;
    }

    private void IncrementTutorialStep() {
        tutorialStep++;
        UpdateStepText();
        EventManager.i.e_TutorialStepChanged(tutorialStep);
    }

    private void HideTutorialStep() {
        EventManager.i.e_TutorialStepHide(tutorialStep);
    }

    private void UpdateStepText() {
        if (debug) {
            stepText.text = $"T Step: {tutorialStep}";
        }
    }

    private void OnFishHooked(GameObject fish) {
        lastFishHookedPosition = fish.transform.position;
    }

    private bool StepIs(int step) {
        return step == tutorialStep;
    }

    private bool StepBefore(int step) {
        return (step - 1) == tutorialStep;
    }

    ////////////////////////////////////
    // Tutorial Logic

    private void OnGameTimeResumed() {
        if (Globals.ValueInstance.dayCount == 1 && StepIs(Consts.TUTORIAL_STEP_NULL) &&
        Globals.ValueInstance.tutorialActive) {
            IncrementTutorialStep();
        }
    }

    private void OnTriggerActivated(TriggerId triggerId) {
        if (triggerId == TriggerId.CastLineTutorial &&
        StepBefore(Consts.TUTORIAL_STEP_HOW_TO_CAST)) {
            IncrementTutorialStep();
        }

        if (triggerId == TriggerId.FatigueInformationalTutorial &&
        StepBefore(Consts.TUTORIAL_STEP_FATIGUE_INFO)) {
            IncrementTutorialStep();
        }

        if (triggerId == TriggerId.EnergizedInformationalTutorial &&
        StepBefore(Consts.TUTORIAL_STEP_ENERGIZED_INFO)) {
            IncrementTutorialStep();
        }
    }

    private void OnMinigameStart() {
        if (StepBefore(Consts.TUTORIAL_STEP_REEL_AND_CAPTURE)) {
            IncrementTutorialStep();
        } else if (StepIs(Consts.TUTORIAL_STEP_REEL_AND_CAPTURE)) {
            EventManager.i.e_TutorialStepChanged(tutorialStep);
        }

        if (StepBefore(Consts.TUTORIAL_STEP_YANK_AND_CAPTURE)) {
            IncrementTutorialStep();
        } else if (StepIs(Consts.TUTORIAL_STEP_YANK_AND_CAPTURE)) {
            EventManager.i.e_TutorialStepChanged(tutorialStep);
        }


        if (StepBefore(Consts.TUTORIAL_STEP_HOLD_AND_CAPTURE)) {
            IncrementTutorialStep();
        } else if (StepIs(Consts.TUTORIAL_STEP_HOLD_AND_CAPTURE)) {
            EventManager.i.e_TutorialStepChanged(tutorialStep);
        }
    }

    private void OnMinigameBeginAnimatingOffscreen() {
        if (StepIs(Consts.TUTORIAL_STEP_REEL_AND_CAPTURE) ||
        StepIs(Consts.TUTORIAL_STEP_YANK_AND_CAPTURE) ||
        StepIs(Consts.TUTORIAL_STEP_HOLD_AND_CAPTURE)) {
            HideTutorialStep();
        }
    }

    private void OnFishEscaped() {
        if (StepIs(Consts.TUTORIAL_STEP_REEL_AND_CAPTURE)) {
            Instantiate(tutorialPufftailPrefab, lastFishHookedPosition, Quaternion.identity);
        } else if (StepIs(Consts.TUTORIAL_STEP_YANK_AND_CAPTURE)) {
            Instantiate(tutorialLimeBassPrefab, lastFishHookedPosition, Quaternion.identity);
        } else if (StepIs(Consts.TUTORIAL_STEP_HOLD_AND_CAPTURE)) {
            Instantiate(tutorialJadeAngelfishPrefab, lastFishHookedPosition, Quaternion.identity);
        }
    }

    private void OnFishCaptureOutOfTime() {
        if (StepIs(Consts.TUTORIAL_STEP_REEL_AND_CAPTURE)) {
            Instantiate(tutorialPufftailPrefab, lastFishHookedPosition, Quaternion.identity);
        } else if (StepIs(Consts.TUTORIAL_STEP_YANK_AND_CAPTURE)) {
            Instantiate(tutorialLimeBassPrefab, lastFishHookedPosition, Quaternion.identity);
        } else if (StepIs(Consts.TUTORIAL_STEP_HOLD_AND_CAPTURE)) {
            Instantiate(tutorialJadeAngelfishPrefab, lastFishHookedPosition, Quaternion.identity);
        }
    }

    private void OnSuccessModalClosed() {
        if (StepBefore(Consts.TUTORIAL_STEP_FIRST_CATCH)) {
            Destroy(tutorialBarrier1);
        } else if (StepBefore(Consts.TUTORIAL_STEP_POST_YANK_INFO)) {
            Destroy(tutorialBarrier2);
        } else if (StepBefore(Consts.TUTORIAL_STEP_FINAL)) {
            Destroy(tutorialBarrier3);
        }

        if (StepBefore(Consts.TUTORIAL_STEP_FIRST_CATCH) ||
        StepBefore(Consts.TUTORIAL_STEP_POST_YANK_INFO) ||
        StepBefore(Consts.TUTORIAL_STEP_FINAL)) {
            IncrementTutorialStep();
        }
    }

    private void OnTutorialModalHidden() {
        if (StepIs(Consts.TUTORIAL_STEP_FINAL)) {
            UnloadAllTutorials();
        }
    }

    public void UnloadAllTutorials() {
        Destroy(tutorialModalsContainer);
        Destroy(tutorialEntitiesContainer);
        for (int i = 0; i < hideAbilityButtonScripts.Length; i++) {
            Destroy(hideAbilityButtonScripts[i]);
        }
        Globals.ValueInstance.tutorialActive = false;
        Destroy(this);
    }
}
