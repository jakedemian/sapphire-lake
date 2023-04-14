using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerInfo : MonoBehaviour {
    public static CanvasScalerInfo i;
    [SerializeField] private CanvasScaler canvasScaler;
    private Vector2 referenceResolution;
    public float currentScaleFactor { get; private set; }
    private Vector2 lastScreenSize;

    void Awake() {
        i ??= this;
        referenceResolution = canvasScaler.referenceResolution;
    }

    void Update() {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        if (screenSize == lastScreenSize) return;

        float widthRatio = screenSize.x / referenceResolution.x;
        float heightRatio = screenSize.y / referenceResolution.y;

        float match = canvasScaler.matchWidthOrHeight;
        currentScaleFactor = Mathf.Lerp(widthRatio, heightRatio, match);
        lastScreenSize = screenSize;
    }
}
