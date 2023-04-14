using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : UIMenu {
    private readonly string RESUME = "RESUME";
    private readonly string OPTIONS = "OPTIONS";
    private readonly string QUIT = "QUIT";

    [SerializeField] private GameObject optionsMenu;

    private void Awake() {
        EventManager.i.onGamePaused += OnGamePaused;
        EventManager.i.onGameUnpaused += OnGameUnpaused;
        EventManager.i.onMenuOptionSelected += OnMenuOptionSelected;
    }

    private void OnMenuOptionSelected(int menuInstanceId, string value) {
        if (menuInstanceId != menu.GetInstanceID()) return;

        if (value.Equals(RESUME)) {
            PauseController.i.Unpause();
        } else if (value.Equals(OPTIONS)) {
            optionsMenu.SetActive(true);
            menu.SetActive(false);
        } else if (value.Equals(QUIT)) {
            Debug.Log("QUIT");
            Application.Quit();
        }
    }

    private void OnGamePaused() {
        menu.SetActive(true);
    }

    private void OnGameUnpaused() {
        menu.SetActive(false);
    }
}
