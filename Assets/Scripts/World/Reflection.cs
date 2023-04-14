using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflection : MonoBehaviour {
    [SerializeField] private Sprite[] objectSprites;
    [SerializeField] private Sprite[] reflectionSprites;
    public float xOffsetFromObjectMax;
    public float yOffsetFromObject;
    public Consts.TIMES_IN_MINUTES startOfMaxLightingMinutes;
    public Consts.TIMES_IN_MINUTES endOfMaxLightingMinutes;
    public float minimumAlpha;
    public float maximumAlpha;

    private TimeOfDay noon = new TimeOfDay(Consts.TIMES_IN_MINUTES.NOON);
    private TimeOfDay startOfMaxLighting;
    private TimeOfDay endOfMaxLighting;
    private Transform parent;
    private SpriteRenderer parentSpriteRenderer;
    private SpriteRenderer sr;

    // undecided if I want a shadow or reflection, so this code allows me to more quickly
    // switch between the two
    public bool isShadow;
    private float r;
    private float g;
    private float b;

    private void Awake() {
        if (reflectionSprites.Length != objectSprites.Length) {
            Debug.LogError("Reflection sprite arrays contain different numbers of sprites!");
        }

        parent = transform.parent;
        startOfMaxLighting = new TimeOfDay(startOfMaxLightingMinutes);
        endOfMaxLighting = new TimeOfDay(endOfMaxLightingMinutes);
        sr = GetComponent<SpriteRenderer>();
        parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        r = sr.color.r;
        g = sr.color.g;
        b = sr.color.b;
    }

    private void LateUpdate() {
        UpdatePosition();
        UpdateAlpha();
        if (reflectionSprites.Length == 1) return;

        for (int i = 0; i < objectSprites.Length; i++) {
            if (objectSprites[i] == parentSpriteRenderer.sprite) {
                sr.sprite = reflectionSprites[i];
                break;
            }
        }
    }

    private void UpdatePosition() {
        Vector3 parentPosition = parent.position;
        transform.rotation = parent.rotation;

        float newY = parentPosition.y + yOffsetFromObject;

        Vector2 from, to;
        float t;
        if (DayNightCycle.i.time.allMinutes <= noon.allMinutes) {
            from = new Vector2(isShadow ? parentPosition.x - xOffsetFromObjectMax : parent.position.x, newY);
            to = new Vector2(parentPosition.x, newY);
            t = DayNightCycle.i.time.GetPercentToTime(DayNightCycle.i.time.startOfDay, noon);
        } else {
            from = new Vector2(parentPosition.x, newY);
            to = new Vector2(isShadow ? parentPosition.x + xOffsetFromObjectMax : parent.position.x, newY);
            t = DayNightCycle.i.time.GetPercentToTime(noon, DayNightCycle.i.time.endOfDay);
        }

        transform.position = Vector2.Lerp(from, to, t);
    }

    private void UpdateAlpha() {
        float t;
        Color start, end;
        if (DayNightCycle.i.time.allMinutes <= startOfMaxLighting.allMinutes) {
            t = DayNightCycle.i.time.GetPercentToTime(
                DayNightCycle.i.time.startOfDay,
                startOfMaxLighting
            );
            start = new Color(r, g, b, minimumAlpha);
            end = new Color(r, g, b, maximumAlpha);

            sr.color = Color.Lerp(start, end, t);
        } else if (DayNightCycle.i.time.allMinutes >= endOfMaxLighting.allMinutes) {
            t = DayNightCycle.i.time.GetPercentToTime(
                endOfMaxLighting,
                DayNightCycle.i.time.endOfDay
            );
            start = new Color(r, g, b, maximumAlpha);
            end = new Color(r, g, b, minimumAlpha);

            sr.color = Color.Lerp(start, end, t);
        } else if (sr.color.a != maximumAlpha) {
            sr.color = new Color(r, g, b, maximumAlpha);
        }
    }
}
