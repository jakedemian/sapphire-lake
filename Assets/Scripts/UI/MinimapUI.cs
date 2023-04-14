using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapUI : UIModal {
    [SerializeField] private Transform worldBottomLeft;
    [SerializeField] private Transform worldTopRight;
    [SerializeField] private RectTransform minimapBottomLeft;
    [SerializeField] private RectTransform minimapTopRight;

    [SerializeField] Transform player;
    [SerializeField] private RectTransform playerMarker;

    private Vector2 worldBottomLeftPosition;
    private Vector2 worldTopRightPosition;

    private Vector2 minimapBottomLeftPosition;
    private Vector2 minimapTopRightPosition;

    private bool onScreen;

    private new void Awake() {
        base.Awake();

        EventManager.i.onMinigameStart += OnMinigameStart;
        EventManager.i.onMinigameOffscreen += OnMinigameOffscreen;
        EventManager.i.onDayEnd += OnDayEnd;
        EventManager.i.onBlackScreenTransparent += OnBlackScreenGone;
    }

    private void OnGUI() {
        if (!onScreen) return;

        Vector2 playerWorldPosition = player.position;
        float relativeX = GetPercentBetweenValues(
            worldBottomLeftPosition.x, worldTopRightPosition.x, playerWorldPosition.x
        );
        float relativeY = GetPercentBetweenValues(
            worldBottomLeftPosition.y, worldTopRightPosition.y, playerWorldPosition.y
        );

        Vector2 relativePosition = new Vector2(relativeX, relativeY);

        playerMarker.anchoredPosition = GetPositionFromRelativePosition(
            minimapBottomLeftPosition, minimapTopRightPosition, relativePosition
        );
    }

    private float GetPercentBetweenValues(float a, float b, float x) {
        if (x < a || x > b) return -1f;

        float abDistance = b - a;
        float axDistance = x - a;

        return axDistance / abDistance;
    }

    private Vector2 GetPositionFromRelativePosition(Vector2 bottomLeft, Vector2 topRight, Vector2 relativePosition) {
        float x = bottomLeft.x + ((topRight.x - bottomLeft.x) * relativePosition.x);
        float y = bottomLeft.y + ((topRight.y - bottomLeft.y) * relativePosition.y);
        return new Vector2(x, y);
    }

    private void OnDayEnd() {
        onScreen = false;
        base.Hide();
    }

    private void OnMinigameStart() {
        onScreen = false;
        base.Hide();
    }

    private void OnMinigameOffscreen() {
        base.Show(() => {
            onScreen = true;
            UpdatePositionCache();
        });
    }

    private void OnBlackScreenGone() {
        base.Show(() => {
            onScreen = true;
            UpdatePositionCache();
        });
    }

    private void UpdatePositionCache() {
        worldBottomLeftPosition = worldBottomLeft.position;
        worldTopRightPosition = worldTopRight.position;

        minimapBottomLeftPosition = minimapBottomLeft.anchoredPosition;
        minimapTopRightPosition = minimapTopRight.anchoredPosition;
    }
}
