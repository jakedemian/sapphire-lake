using UnityEngine;

public class MenuOption : MonoBehaviour {
    public RectTransform pointerAnchor;
    public string value;

    public RectTransform rectTransform { get; private set; }
    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }
}
