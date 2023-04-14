using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour {
    public static TitleScreenController i;
    public Image blackScreen;
    public AudioSource musicSource;
    public float fadeInSeconds;
    public float fadeOutSeconds;
    public float postFadeOutDelaySeconds;

    private void Awake() {
        i ??= this;
        StartCoroutine(FadeIn());
        EventManager.i.onVolumeChange += OnVolumeChange;
        musicSource.volume = Settings.ValueInstance.musicVolume * Settings.ValueInstance.masterVolume;
    }

    public void Play() {
        StartCoroutine(FadeOut(() => {
            SceneManager.LoadScene("Game");
        }));
    }

    private void OnVolumeChange() {
        musicSource.volume = Settings.ValueInstance.musicVolume * Settings.ValueInstance.masterVolume;
    }

    private IEnumerator FadeOut(Action callback) {
        Color transparent = new Color(0, 0, 0, 0);
        Color opaque = new Color(0, 0, 0, 1);
        float musicStart = musicSource.volume;

        float timer = 0f;
        while (timer < fadeOutSeconds) {
            timer += Time.deltaTime;

            blackScreen.color = Color.Lerp(transparent, opaque, timer / fadeOutSeconds);
            musicSource.volume = Mathf.Lerp(musicStart, 0f, timer / fadeOutSeconds);
            yield return null;
        }

        yield return new WaitForSeconds(postFadeOutDelaySeconds);
        callback();
    }

    private IEnumerator FadeIn() {
        Color transparent = new Color(0, 0, 0, 0);
        Color opaque = new Color(0, 0, 0, 1);
        float musicStart = musicSource.volume;

        float timer = 0f;
        while (timer < fadeInSeconds) {
            timer += Time.deltaTime;

            blackScreen.color = Color.Lerp(opaque, transparent, timer / fadeInSeconds);
            yield return null;
        }
    }
}
