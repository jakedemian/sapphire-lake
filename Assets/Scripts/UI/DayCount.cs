using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayCount : MonoBehaviour {
    [SerializeField] private float fadeSeconds;
    [SerializeField] private float displaySeconds;

    private int dayCount;
    private TextMeshProUGUI textMeshProText;

    private readonly Color transparent = new Color(1, 1, 1, 0);
    private readonly Color opaque = new Color(1, 1, 1, 1);

    private void Awake() {
        textMeshProText = GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        dayCount = Globals.ValueInstance.dayCount;
        textMeshProText.text = $"Day {dayCount.ToString()}";
        StartCoroutine(Fade(transparent, opaque, fadeSeconds, () => {
            StartCoroutine(Util.DoActionAfterDelay(displaySeconds, () => {
                StartCoroutine(Fade(opaque, transparent, fadeSeconds));
            }));
        }));
    }

    private IEnumerator Fade(Color start, Color end, float seconds, Action callback = null) {
        float timer = 0f;
        while (timer < seconds) {
            timer += Time.deltaTime;

            textMeshProText.color = Color.Lerp(start, end, timer / seconds);
            yield return null;
        }

        if (callback != null) {
            callback();
        }
    }
}
