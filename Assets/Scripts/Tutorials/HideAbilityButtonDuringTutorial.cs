using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideAbilityButtonDuringTutorial : MonoBehaviour {
    [SerializeField] private List<int> stepsToHide;
    private Image image;
    private int tutorialStep => TutorialController.i.tutorialStep;
    private bool shouldHide => stepsToHide.Contains(tutorialStep);

    private readonly Color opaque = new Color(1, 1, 1, 1);
    private readonly Color transparent = new Color(1, 1, 1, 0);

    private void Awake() {
        image = GetComponent<Image>();
        EventManager.i.onMinigameStart += OnMinigameStart;
        EventManager.i.onMinigameOffscreen += OnMinigameOffscreen;

        // this has to be here for the first time, Awake hasn't happened the first
        // time the minigame loads up
        if (shouldHide) {
            image.color = transparent;
        }
    }

    private void OnDestroy() {
        EventManager.i.onMinigameOffscreen -= OnMinigameOffscreen;
        EventManager.i.onMinigameStart -= OnMinigameStart;
    }

    private void OnMinigameStart() {
        if (shouldHide) {
            image.color = transparent;
        }

        // IF RACE CONDITION
        // try getting below to work
        //
        // StartCoroutine(Util.DoActionEndOfFrame(() => {
        //     if (shouldHide) {
        //         image.color = transparent;
        //     }
        // }));
    }

    private void OnMinigameOffscreen() {
        if (shouldHide) {
            image.color = opaque;
        }
    }
}