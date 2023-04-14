using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPrompt : MonoBehaviour {
    public Vector2 topScreenPosition;
    public float topScreenThreshold;

    private Transform bobber;
    private void Awake() {
        EventManager.i.onDayEnd += OnDayEnd;
    }

    private void OnDestroy() {
        EventManager.i.onDayEnd -= OnDayEnd;
    }

    private void Start() {
        bobber = transform.parent;
        if (Camera.main.WorldToViewportPoint(bobber.position).y > topScreenThreshold) {
            transform.position = (Vector2)bobber.position + topScreenPosition;
        }
    }

    private void OnDayEnd() {
        gameObject.SetActive(false);
    }
}
