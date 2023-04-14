using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider ambienceSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake() {
        masterSlider.onValueChanged.AddListener(delegate { MasterValueChanged(); });
        musicSlider.onValueChanged.AddListener(delegate { MusicValueChanged(); });
        ambienceSlider.onValueChanged.AddListener(delegate { AmbienceValueChanged(); });
        sfxSlider.onValueChanged.AddListener(delegate { SfxValueChanged(); });
        EventManager.i.onGameUnpaused += OnGameUnpaused;
    }

    private void Start() {
        masterSlider.value = Settings.ValueInstance.masterVolume;
        musicSlider.value = Settings.ValueInstance.musicVolume;
        ambienceSlider.value = Settings.ValueInstance.musicVolume;
        sfxSlider.value = Settings.ValueInstance.sfxVolume;
    }

    private void OnGameUnpaused() {
        optionsMenu.SetActive(false);
    }

    private void MasterValueChanged() {
        Settings.ValueInstance.masterVolume = masterSlider.value;
        EventManager.i.e_VolumeChanged();
    }

    private void MusicValueChanged() {
        Settings.ValueInstance.musicVolume = musicSlider.value;
        EventManager.i.e_VolumeChanged();
    }

    private void AmbienceValueChanged() {
        Settings.ValueInstance.ambienceVolume = ambienceSlider.value;
        EventManager.i.e_VolumeChanged();
    }

    private void SfxValueChanged() {
        Settings.ValueInstance.sfxVolume = sfxSlider.value;
        EventManager.i.e_VolumeChanged();
    }

    public void OnClickedBack() {
        optionsMenu.SetActive(false);

        if (pauseMenu != null) {
            pauseMenu.SetActive(true);
        }
    }
}
