using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialModalUI : UIModal {
    [SerializeField] private int tutorialStep;
    [SerializeField] private bool shouldStopTime;
    [SerializeField] private bool dismissable;

    // true if controlled by code, false if controlled by step number
    [SerializeField] private bool controlled;

    private VideoPlayer video;
    private bool dismissLocked = true;

    private new void Awake() {
        base.Awake();

        video = GetComponentInChildren<VideoPlayer>();
        if (video != null) {
            video.url = System.IO.Path.Combine(Application.streamingAssetsPath, "cast-line.ogv");
        }

        EventManager.i.onTutorialStepChange += OnTutorialStepChange;
        EventManager.i.onTutorialStepHide += OnTutorialStepHide;
    }

    private void OnTutorialStepChange(int step) {
        if (step == this.tutorialStep) {
            Show();

            if (video != null) {
                video.Play();
            }
        } else if (isActive && !controlled) {
            Hide();
        }
    }

    private void OnTutorialStepHide(int step) {
        if (step == this.tutorialStep && isActive) {
            Hide();
        }
    }

    private void Update() {
        if (!isActive || !dismissable || (dismissable && dismissLocked)) return;

        if (Input.GetKeyDown(KeyCode.F)) {
            Hide();
        }
    }

    public override void Show(Action callback = null) {
        base.Show(callback);

        if (shouldStopTime) {
            DayNightCycle.i.SetPaused(true);
        }
        if (dismissable) {
            StartCoroutine(UnlockDismiss());
        }
    }

    public override void Hide(Action callback = null) {
        base.Hide(() => {
            callback?.Invoke();
            EventManager.i.e_TutorialModalHidden();
        });

        if (shouldStopTime) {
            DayNightCycle.i.SetPaused(false);
        }
        if (dismissable) {
            dismissLocked = true;
        }
    }

    private IEnumerator UnlockDismiss() {
        yield return new WaitForEndOfFrame();
        dismissLocked = false;
    }
}
