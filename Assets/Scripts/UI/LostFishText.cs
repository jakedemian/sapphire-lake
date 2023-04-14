using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostFishText : MonoBehaviour {
    public static LostFishText i;
    public float showSeconds;
    public float fadeSeconds;

    private SpriteRenderer sr;

    private void Awake() {
        i ??= this;
        sr = GetComponent<SpriteRenderer>();
    }

    public void Show() {
        StartCoroutine(DoShow());
    }

    private IEnumerator DoShow() {
        Color transparent = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
        Color opaque = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        sr.color = transparent;

        //fade in
        float timer = 0f;
        while (timer < fadeSeconds) {
            timer += Time.deltaTime;

            float t = timer / fadeSeconds;
            sr.color = Color.Lerp(transparent, opaque, t);

            yield return null;
        }

        //wait
        yield return new WaitForSeconds(showSeconds);

        //fade out
        timer = 0f;
        while (timer < fadeSeconds) {
            timer += Time.deltaTime;

            float t = timer / fadeSeconds;
            sr.color = Color.Lerp(opaque, transparent, t);

            yield return null;
        }
    }
}
