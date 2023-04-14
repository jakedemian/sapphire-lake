using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour {
    private void Awake() {
        MusicManager.i.StartMorning();
    }

    private void Start() {
        if (Globals.ValueInstance.dayCount > 1) {
            TutorialController.i?.UnloadAllTutorials();
        }
    }
}
