using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugController : MonoBehaviour {
    public GameObject debugMenu;

    private void Awake() {
        debugMenu.SetActive(false);
    }

    public void ToggleMenu() {
        debugMenu.SetActive(!debugMenu.activeInHierarchy);
    }

}
