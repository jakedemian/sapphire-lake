using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : MonoBehaviour {
    [SerializeField] private GameObject creditsMenu;

    public void OnClickedBack() {
        creditsMenu.SetActive(false);
    }
}
