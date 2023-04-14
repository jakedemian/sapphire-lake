using UnityEngine;

public abstract class UIMenu : MonoBehaviour {
    [SerializeField] protected GameObject menu;
    [SerializeField] protected MenuOption[] menuOptions;
    [SerializeField] private RectTransform menuPointer;

    private int currentIndex;

    private void Awake() {
        for (int i = 0; i < menuOptions.Length; i++) {
            if (menuOptions[i].pointerAnchor == null) {
                Debug.LogError("Pointer anchor for menu option " +
                $"with value '{menuOptions[i]}' is not set.");
            }
        }
    }

    private void Update() {
        void UpdatePointer() {
            if (currentIndex == -1) {
                menuPointer.gameObject.SetActive(false);
                return;
            }

            if (!menuPointer.gameObject.activeInHierarchy) {
                menuPointer.gameObject.SetActive(true);
            }

            menuPointer.position = menuOptions[currentIndex].pointerAnchor.position;
        }

        if (!menu.activeInHierarchy) return;

        int newIndex = -1;
        for (int i = 0; i < menuOptions.Length; i++) {
            if (RectTransformUtility.RectangleContainsScreenPoint(menuOptions[i].rectTransform, Input.mousePosition)) {
                newIndex = i;
                break;
            }
        }
        currentIndex = newIndex;
        UpdatePointer();

        if (Input.GetMouseButtonDown(0)) {
            if (currentIndex > -1) {
                EventManager.i.e_MenuOptionSelected(
                    menu.GetInstanceID(),
                    menuOptions[currentIndex].value
                );
            }
        }
    }


}
