using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class TimeOfDayTextUI : MonoBehaviour {
    public SpriteRenderer time;
    public SpriteRenderer haiku;

    public Sprite morning;
    public Sprite afternoon;
    public Sprite evening;
    public Sprite[] morningHaikus;
    public Sprite[] afternoonHaikus;
    public Sprite[] eveningHaikus;
    public float showHideFadeSeconds;
    public float showSeconds;

    private void Awake() {
        EventManager.i.onDayStart += OnDayStart;
        EventManager.i.onAfternoonStart += OnAfternoonStart;
        EventManager.i.onEveningStart += OnEveningStart;
    }

    private void OnDayStart() {
        StartCoroutine(Show(morning, morningHaikus));
    }

    private void OnAfternoonStart() {
        StartCoroutine(Show(afternoon, afternoonHaikus));
    }
    private void OnEveningStart() {
        StartCoroutine(Show(evening, eveningHaikus));
    }

    private IEnumerator Show(Sprite timeSprite, Sprite[] haikuSprites) {
        time.sprite = timeSprite;
        haiku.sprite = haikuSprites[UnityEngine.Random.Range(0, haikuSprites.Length)];

        Color opaque = new Color(1, 1, 1, 1);
        Color transparent = new Color(1, 1, 1, 0);

        float timer = 0f;
        while (timer < showHideFadeSeconds) {
            timer += Time.deltaTime;
            time.color = Color.Lerp(transparent, opaque, timer / showHideFadeSeconds);
            haiku.color = Color.Lerp(transparent, opaque, timer / showHideFadeSeconds);
            yield return null;
        }

        yield return new WaitForSeconds(showSeconds);

        timer = 0f;
        while (timer < showHideFadeSeconds) {
            timer += Time.deltaTime;
            time.color = Color.Lerp(opaque, transparent, timer / showHideFadeSeconds);
            haiku.color = Color.Lerp(opaque, transparent, timer / showHideFadeSeconds);
            yield return null;
        }
    }


}
