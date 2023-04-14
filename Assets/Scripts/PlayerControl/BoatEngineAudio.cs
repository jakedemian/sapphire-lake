using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEngineAudio : MonoBehaviour {
    public AudioClip engineSound;
    public float pitchScaleFactor;

    private Rigidbody2D rb;
    private AudioSource source;

    private float masterVolume => Settings.ValueInstance.masterVolume;
    private float sfxVolume => masterVolume * Settings.ValueInstance.sfxVolume;
    public float currentSpeed => rb.velocity.magnitude;
    private bool gameIsPaused;

    private void Awake() {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        source.clip = engineSound;
        source.loop = true;

        EventManager.i.onVolumeChange += OnVolumeChange;
        EventManager.i.onGamePaused += OnGamePaused;
        EventManager.i.onGameUnpaused += OnGameUnpaused;
    }

    private void OnVolumeChange() {
        source.volume = sfxVolume;
    }

    private void OnGamePaused() {
        source.Pause();
        gameIsPaused = true;
    }

    private void OnGameUnpaused() {
        source.UnPause();
        gameIsPaused = false;
    }

    private void Update() {
        if (gameIsPaused) return;

        if (currentSpeed > 0f && !source.isPlaying) {
            source.Play();
        } else if (currentSpeed == 0f && source.isPlaying) {
            source.Stop();
        }

        if (source.isPlaying) {
            source.pitch = currentSpeed * pitchScaleFactor;
        }
    }
}
