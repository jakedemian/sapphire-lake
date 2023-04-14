using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour {
    public static PauseController i;
    public bool isPaused { get; private set; }

    private void Awake() {
        i ??= this;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // we want the pause signal to go out before timescale is set to 0
            if (!isPaused) {
                EventManager.i.e_GamePaused();
            }

            StartCoroutine(DoPauseEndOfFrame());
        }
    }

    // theoretically this gives all objects time to process the event?
    private IEnumerator DoPauseEndOfFrame() {
        yield return new WaitForEndOfFrame();

        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (!isPaused) {
            EventManager.i.e_GameUnpaused();
        }
    }

    public void Pause() {
        if (!isPaused) {
            EventManager.i.e_GamePaused();
        } else {
            Debug.LogError("Pause() called when game was already paused.");
        }
        StartCoroutine(DoPauseEndOfFrame());
    }

    public void Unpause() {
        if (isPaused) {
            EventManager.i.e_GameUnpaused();
            isPaused = false;
            Time.timeScale = 1f;
        } else {
            Debug.LogError("Unpause() called when game was not paused.");
        }
    }
}
