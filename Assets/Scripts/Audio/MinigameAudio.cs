using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameAudio : MonoBehaviour {
    public static MinigameAudio i;
    public AudioClip reeling;
    public float reelPitchIncrease;
    public float yankPitchIncrease;

    private AudioSource reelingSource;
    private float basePitch;

    private float masterVolume => Settings.ValueInstance.masterVolume;
    private float sfxVolume => masterVolume * Settings.ValueInstance.sfxVolume;

    private void Awake() {
        i ??= this;
        reelingSource = GetComponent<AudioSource>();
        reelingSource.loop = true;
        reelingSource.clip = reeling;
        basePitch = reelingSource.pitch;
        EventManager.i.onVolumeChange += OnVolumeChange;
        EventManager.i.onGamePaused += OnGamePaused;
        EventManager.i.onGameUnpaused += OnGameUnpaused;
    }

    private void OnVolumeChange() {
        reelingSource.volume = sfxVolume;
    }

    private void OnGamePaused() {
        reelingSource.Pause();
    }

    private void OnGameUnpaused() {
        reelingSource.UnPause();
    }

    public void PlayReeling() {
        reelingSource.Play();
    }

    public void DoReelPitchIncrease() {
        StartCoroutine(IncreasePitch(0.1f, 0.5f, reelPitchIncrease));
    }

    public void DoYankPitchIncrease() {
        StartCoroutine(IncreasePitch(0.1f, 0.5f, yankPitchIncrease));
    }

    public void Stop() {
        reelingSource.Stop();
    }

    public void Pause() {
        reelingSource.Pause();
    }

    public void Unpause() {
        reelingSource.UnPause();
    }

    private IEnumerator IncreasePitch(float rampUpSeconds, float rampDownSeconds, float pitchIncrease) {
        float timer = 0f;
        float peakPitch = reelingSource.pitch * pitchIncrease;

        while (timer < rampUpSeconds) {
            timer += Time.deltaTime;
            reelingSource.pitch = Mathf.Lerp(basePitch, peakPitch, timer / rampUpSeconds);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        timer = 0f;

        while (timer < rampDownSeconds) {
            timer += Time.deltaTime;
            reelingSource.pitch = Mathf.Lerp(peakPitch, basePitch, timer / rampDownSeconds);
            yield return null;
        }
        reelingSource.pitch = basePitch;
    }
}
