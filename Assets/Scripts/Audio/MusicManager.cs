using System.Collections;
using System;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    public static MusicManager i;
    public AudioClip morningMusic;
    public AudioClip afternoonMusic;
    public AudioClip eveningMusic;
    public float fadeSeconds;
    [SerializeField] private float morningMusicDelay;

    private AudioSource musicSource;
    private AudioSource waterAmbienceSource;

    public float masterVolume => Settings.ValueInstance.masterVolume;
    public float musicVolume => Settings.ValueInstance.musicVolume * masterVolume;
    public float ambienceVolume => Settings.ValueInstance.ambienceVolume * masterVolume;

    private void Awake() {
        i ??= this;
        AudioSource[] sources = GetComponents<AudioSource>();

        musicSource = sources[0];
        waterAmbienceSource = sources[1];

        EventManager.i.onAfternoonStart += OnAfternoonStart;
        EventManager.i.onEveningStart += OnEveningStart;
        EventManager.i.onVolumeChange += OnVolumeChange;
        EventManager.i.onGamePaused += OnGamePaused;
        EventManager.i.onGameUnpaused += OnGameUnpaused;
    }

    private void OnVolumeChange() {
        musicSource.volume = musicVolume;
        waterAmbienceSource.volume = ambienceVolume;

    }

    private void OnAfternoonStart() {
        StartAfternoon();
    }

    private void OnEveningStart() {
        StartEvening();
    }

    private void OnGamePaused() {
        HardPauseAll();
    }
    private void OnGameUnpaused() {
        HardUnpauseAll();
    }

    public void StartMorning() {
        musicSource.clip = morningMusic;
        StartCoroutine(Util.DoActionAfterDelay(morningMusicDelay, () => {
            StartCoroutine(FadeIn(musicSource, musicVolume));
        }));
        StartCoroutine(FadeIn(waterAmbienceSource, ambienceVolume));
    }

    public void StartAfternoon() {
        StartCoroutine(FadeOut(musicSource, musicVolume, () => {
            musicSource.Stop();
            musicSource.clip = afternoonMusic;
            musicSource.Play();
            StartCoroutine(FadeIn(musicSource, musicVolume));
        }));
    }

    public void StartEvening() {
        StartCoroutine(FadeOut(musicSource, musicVolume, () => {
            musicSource.Stop();
            musicSource.clip = eveningMusic;
            musicSource.Play();
            StartCoroutine(FadeIn(musicSource, musicVolume));
        }));
    }

    public void PauseMusic() {
        StartCoroutine(FadeOut(musicSource, musicVolume));
    }

    public void UnpauseMusic() {
        StartCoroutine(FadeIn(musicSource, musicVolume));
    }

    public void PauseAll(Action callback = null) {
        StartCoroutine(FadeOut(musicSource, musicVolume, callback));
        StartCoroutine(FadeOut(waterAmbienceSource, ambienceVolume)); // bit weird with the callback
    }

    public void UnpauseAll() {
        StartCoroutine(FadeIn(musicSource, musicVolume));
        StartCoroutine(FadeIn(waterAmbienceSource, ambienceVolume));
    }

    public void HardPauseMusic() {
        musicSource.Pause();
    }

    public void HardUnpauseMusic() {
        musicSource.UnPause();
    }

    public void HardPauseAmbience() {
        waterAmbienceSource.Pause();
    }

    public void HardUnpauseAmbience() {
        waterAmbienceSource.UnPause();
    }

    public void HardPauseAll() {
        HardPauseMusic();
        HardPauseAmbience();
    }

    public void HardUnpauseAll() {
        HardUnpauseMusic();
        HardUnpauseAmbience();
    }

    private IEnumerator FadeOut(AudioSource source, float startVolume, Action callback = null) {
        float endVolume = 0f;

        float timer = 0f;
        while (timer < fadeSeconds) {
            timer += Time.deltaTime;

            float t = timer / fadeSeconds;
            source.volume = Mathf.Lerp(startVolume, endVolume, t);

            yield return null;
        }
        source.Pause();

        if (callback != null) {
            callback();
        }
    }

    private IEnumerator FadeIn(AudioSource source, float endVolume, Action callback = null) {
        float startVolume = 0f;
        source.volume = startVolume;

        if (source.time != 0) {
            source.UnPause();
        } else {
            source.Play();
        }

        float timer = 0f;
        while (timer < fadeSeconds) {
            timer += Time.deltaTime;

            float t = timer / fadeSeconds;
            source.volume = Mathf.Lerp(startVolume, endVolume, t);

            yield return null;
        }
        source.volume = endVolume;

        if (callback != null) {
            callback();
        }
    }
}
