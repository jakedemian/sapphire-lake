using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum MinigameState {
    Active,
    Inactive,
    SuccessModal,
    AwaitingUI
}

public class MinigameController : MonoBehaviour {
    public static MinigameController i;
    public GameObject minigameContainer;
    public UIMinigameContainer uiMinigame;
    public UISuccessModal uiSuccessModal;

    public RectTransform line;
    public MinigameFish fish;
    public GameObject fishTarget;
    public GameObject playerTarget;
    public Ability capture;
    public Ability reel;
    public Ability yank;
    public Ability hold;

    public Color isCapturingColor;
    public float winLoseDistancePadding;

    public bool isAwaitingCapture { get; private set; }
    public float progressPercent { get; private set; }
    private Coroutine awaitingCapture;
    private FishData fishData;
    public MinigameState state = MinigameState.Inactive;

    private void Awake() {
        i ??= this;
    }

    private void Update() {
        if (isAwaitingCapture || state != MinigameState.Active) {
            return;
        }

        progressPercent = Util.GetPercentToTarget(
           fishTarget.transform.position,
           playerTarget.transform.position,
           fish.gameObject.transform.position
        );

        if (progressPercent >= 1f - winLoseDistancePadding) {
            fish.Stop();
            progressPercent = 1f;
            awaitingCapture = StartCoroutine(AwaitCapture());
        } else if (progressPercent <= 0f + winLoseDistancePadding) {
            fish.Stop();
            progressPercent = 0f;
            EventManager.i.e_FishEscaped();
            FishEscapes();
        }
    }

    public void InitializeMinigame(FishData _fishData) {
        fishData = _fishData;

        EventManager.i.e_MinigameStart();
        MusicManager.i.PauseMusic();
        minigameContainer.SetActive(true);
        fish.Initialize(fishData);
        uiMinigame.OnStart();
        line.gameObject.SetActive(true);
        progressPercent = fishData.startingPercentage;

        StartCoroutine(WaitForMinigameAnimation(() => {
            MinigameAudio.i.PlayReeling();
            state = MinigameState.Active;
            EventManager.i.e_MinigameInteractive();
        }));
    }

    public void ReelInput(InputAction.CallbackContext context) {
        if (!context.performed || state != MinigameState.Active || fish.isStopped)
            return;

        if (!reel.isOnCooldown) {
            MinigameAudio.i.DoReelPitchIncrease();
            fish.Reel();
        }

        reel.Activate();
    }

    public void YankInput(InputAction.CallbackContext context) {
        if (!context.performed || state != MinigameState.Active || fish.isStopped) return;

        if (!yank.isOnCooldown && fish.isFatigued) {
            MinigameAudio.i.DoYankPitchIncrease();
            fish.Yank();
        }

        yank.Activate();
    }

    public void HoldInput(InputAction.CallbackContext context) {
        if (!context.performed || state != MinigameState.Active) return;


        if (!hold.isOnCooldown && fish.isEnergized) {
            MinigameAudio.i.Pause();
            fish.Hold();
        }

        hold.Activate();
    }

    public void CaptureInput(InputAction.CallbackContext context) {
        if (!context.performed || state != MinigameState.Active) return;

        if (isAwaitingCapture) {
            capture.Activate();
            CaptureFish();
        }
    }

    public void ContinueInput(InputAction.CallbackContext context) {
        if (!context.performed) return;

        if (state == MinigameState.SuccessModal) {
            state = MinigameState.Inactive;
            uiSuccessModal.Hide();

            StartCoroutine(WaitForSuccessModalAnimation(() => {
                MusicManager.i.UnpauseMusic();
            }));
        }
    }

    private void CaptureFish() {
        if (!isAwaitingCapture) {
            return;
        }

        state = MinigameState.AwaitingUI;
        isAwaitingCapture = false;
        StopCoroutine(awaitingCapture);
        fish.Stop();
        line.gameObject.SetActive(false);
        MinigameAudio.i.Stop();
        AudioManager.i.PlayWinSound();
        Inventory.i.Add(fishData);
        EventManager.i.e_FishCaptured();

        StartCoroutine(fish.DoCaughtAnimation(() => {
            uiMinigame.OnWin();
            EventManager.i.e_MinigameBeginAnimatingOffscreen();
            StartCoroutine(WaitForMinigameAnimation(() => {
                state = MinigameState.SuccessModal;
                minigameContainer.SetActive(false);
                EventManager.i.e_SuccessModalShow();
                EventManager.i.e_MinigameOffscreen();
                uiSuccessModal.Show(fishData.name, fishData.portraitPath);
            }));
        }));
    }

    private void FishEscapes() {
        MinigameAudio.i.Stop();
        AudioManager.i.PlayLoseSound();
        state = MinigameState.Inactive;
        uiMinigame.OnLose();
        EventManager.i.e_MinigameBeginAnimatingOffscreen();
        StartCoroutine(WaitForMinigameAnimation(() => {
            minigameContainer.SetActive(false);
            MusicManager.i.UnpauseMusic();
            LostFishText.i.Show();
            EventManager.i.e_MinigameOffscreen();
        }));
    }

    private void CaptureOutOfTime() {
        EventManager.i.e_FishCaptureOutOfTime();
        isAwaitingCapture = false;
        FishEscapes();
    }

    private IEnumerator AwaitCapture() {
        isAwaitingCapture = true;
        EventManager.i.e_FishAwaitingCapture();
        float fishCaptureWindowSeconds = fish.fishData.captureWindowSeconds;

        float time = fishCaptureWindowSeconds;
        while (time > 0f) {
            time -= Time.deltaTime;
            progressPercent = time / fishCaptureWindowSeconds;
            yield return null;
        }

        CaptureOutOfTime();
    }

    private IEnumerator WaitForFishAnimation(Action callback) {
        yield return null;
    }

    private IEnumerator WaitForMinigameAnimation(Action callback) {
        while (uiMinigame.isAnimating) {
            yield return null;
        }

        callback();
    }

    private IEnumerator WaitForSuccessModalAnimation(Action callback) {
        while (uiSuccessModal.isAnimating) {
            yield return null;
        }

        callback();
    }

    public MinigameState GetState() {
        return state;
    }
}



