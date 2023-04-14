using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
    public RectTransform uiProgressBar;

    public Color lowPercentColor;
    public Color midPercentColor;
    public Color highPercentColor;
    public Color capturingColor;

    private Image progressBarImage;
    private RectTransform rectTransform;
    private float uiProgressBarStartingHeight;
    private float uiProgressBarStartingWidth;

    private float progressPercent => MinigameController.i.progressPercent;
    private bool fishAwaitingCapture;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        progressBarImage = uiProgressBar.GetComponent<Image>();
        uiProgressBarStartingHeight = rectTransform.rect.height;
        uiProgressBarStartingWidth = rectTransform.rect.width;

        EventManager.i.onFishAwaitingCapture += OnFishAwaitingCapture;
        EventManager.i.onMinigameStart += OnMiningameStart;
    }

    private void OnGUI() {
        uiProgressBar.sizeDelta = new Vector2(
            uiProgressBarStartingWidth,
            uiProgressBarStartingHeight * progressPercent
        );

        if (!fishAwaitingCapture) {
            LerpBarColor();
        }
    }

    private void LerpBarColor() {
        progressBarImage.color = progressPercent < 0.5f ?
                    Color.Lerp(lowPercentColor, midPercentColor, progressPercent * 2f) :
                    Color.Lerp(midPercentColor, highPercentColor, (progressPercent - 0.5f) * 2f);
    }

    private void OnFishAwaitingCapture() {
        fishAwaitingCapture = true;
        progressBarImage.color = capturingColor;
    }

    private void OnMiningameStart() {
        fishAwaitingCapture = false;
        LerpBarColor();
    }
}

