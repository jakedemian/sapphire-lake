using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenMenu : UIMenu {
    private readonly string PLAY = "PLAY";
    private readonly string COLLECTION = "COLLECTION";
    private readonly string OPTIONS = "OPTIONS";
    private readonly string CREDITS = "CREDITS";

    [SerializeField] private RectTransform menuRectTransform;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject collectionMenu;
    [SerializeField] private Vector2 onScreenPosition;
    [SerializeField] private Vector2 offScreenPosition;
    [SerializeField] private float animationSeconds;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float delayAnimationSeconds;

    private void Awake() {
        EventManager.i.onMenuOptionSelected += OnMenuOptionSelected;
        StartCoroutine(DoAnimation(offScreenPosition, onScreenPosition, animationSeconds, true));
    }

    private void OnMenuOptionSelected(int menuInstanceId, string value) {
        if (menuInstanceId != menu.GetInstanceID()) return;

        if (value.Equals(PLAY)) {
            TitleScreenController.i.Play();
            StartCoroutine(DoAnimation(onScreenPosition, offScreenPosition, animationSeconds, false, () => {
                menu.SetActive(false);
            }));
        } else if (value.Equals(COLLECTION)) {
            collectionMenu.SetActive(true);
            GetComponent<CollectionMenu>().Init();
        } else if (value.Equals(OPTIONS)) {
            optionsMenu.SetActive(true);
        } else if (value.Equals(CREDITS)) {
            creditsMenu.SetActive(true);
        }
    }

    private IEnumerator DoAnimation(Vector2 start, Vector2 end, float seconds, bool delayStart, Action callback = null) {
        if (delayStart) {
            yield return new WaitForSeconds(delayAnimationSeconds);
        }

        float timer = 0f;
        while (timer < seconds) {
            timer += Time.deltaTime;
            float t = timer / seconds;
            menuRectTransform.anchoredPosition = start + ((end - start) *
                new Vector2(animationCurve.Evaluate(t), animationCurve.Evaluate(t)));
            yield return null;
        }

        if (callback != null) {
            callback();
        }
    }
}
