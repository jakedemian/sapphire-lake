using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobberAoe : MonoBehaviour {
    public float fadeSeconds;
    public float maxAlpha;
    public GameObject target;

    private SpriteRenderer sr;
    public bool isShowing { get; private set; }
    private Coroutine coroutine;
    private float maxBobberRange;

    private Color transparent = new Color(1, 1, 1, 0);
    private Color showColor => new Color(1, 1, 1, maxAlpha);

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (isShowing && maxBobberRange != 0f && Input.GetMouseButton(0)) {
            Vector2 mouseWorldPosition =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!target.activeInHierarchy) {
                target.SetActive(true);
            }

            if (Vector2.Distance(mouseWorldPosition, transform.parent.position) <= maxBobberRange) {
                target.transform.position = mouseWorldPosition;
            } else {
                target.transform.position =
                    (Vector2)transform.parent.position +
                    (mouseWorldPosition - (Vector2)transform.parent.position).normalized
                    * maxBobberRange;
            }
        }

        if (!isShowing && target.activeInHierarchy) {
            target.SetActive(false);
        }
    }

    public void SetMaxRange(float _maxBobberRange) {
        maxBobberRange = _maxBobberRange;
    }

    public void Show() {
        isShowing = true;
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(DoShow());
    }

    public void Hide() {
        isShowing = false;
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(DoHide());
    }

    private IEnumerator DoShow() {
        float timer = 0f;
        while (timer < fadeSeconds) {
            timer += Time.deltaTime;

            sr.color = Color.Lerp(transparent, showColor, timer / fadeSeconds);
            yield return null;
        }
        sr.color = showColor;
    }

    private IEnumerator DoHide() {
        float timer = 0f;
        while (timer < fadeSeconds) {
            timer += Time.deltaTime;

            sr.color = Color.Lerp(showColor, transparent, timer / fadeSeconds);
            yield return null;
        }
        sr.color = transparent;
    }
}
