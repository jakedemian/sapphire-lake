using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {
    public static CursorController i;
    public Texture2D cursorTexture;
    public Texture2D cursorPressedTexture;
    public CursorMode cursorMode = CursorMode.ForceSoftware;
    public Vector2 hotSpot = Vector2.zero;

    private Texture2D currentTexture;

    void Awake() {
        i ??= this;
        SetCursor(cursorTexture);
    }

    private void Update() {
        if (Input.GetMouseButton(0)) {
            SetCursor(cursorPressedTexture);
        } else {
            SetCursor(cursorTexture);
        }
    }

    private void SetCursor(Texture2D tex) {
        if (tex == currentTexture) return;

        currentTexture = tex;
        Cursor.SetCursor(tex, hotSpot, cursorMode);
    }
}
