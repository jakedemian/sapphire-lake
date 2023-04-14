using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager i { get; private set; }
    public AudioClip onCooldown;
    public AudioClip fatigue;
    public AudioClip splashing;
    public AudioClip energized;
    public AudioClip bobberHitWater;
    public AudioClip cast;
    public AudioClip hook;
    public AudioClip lose;
    public AudioClip win;
    public AudioClip successModal;

    private List<AudioSource> channels;

    private float masterVolume => Settings.ValueInstance.masterVolume;
    private float sfxVolume => masterVolume * Settings.ValueInstance.sfxVolume;

    private void Awake() {
        i ??= this;
        channels = new List<AudioSource>();

        EventManager.i.onVolumeChange += OnVolumeChange;
    }

    private void OnVolumeChange() {
        foreach (AudioSource channel in channels) {
            channel.volume = sfxVolume;
        }
    }

    private void PlaySound(AudioClip sound) {
        foreach (AudioSource channel in channels) {
            if (!channel.isPlaying) {
                channel.PlayOneShot(sound);
                return;
            }
        }

        AudioSource newChannel = gameObject.AddComponent<AudioSource>();
        channels.Add(newChannel);
        newChannel.volume = sfxVolume;
        newChannel.PlayOneShot(sound);
    }

    public void PlayOnCooldownSound() {
        PlaySound(onCooldown);
    }

    public void PlayFatigueSound() {
        PlaySound(fatigue);
    }

    public void PlaySplashingSound() {
        PlaySound(splashing);
    }

    public void PlayEnergizedSound() {
        PlaySound(energized);
    }

    public void PlayBobberHitWaterSound() {
        PlaySound(bobberHitWater);
    }

    public void PlayCastSound() {
        PlaySound(cast);
    }

    public void PlayHookSound() {
        PlaySound(hook);
    }

    public void PlayLoseSound() {
        PlaySound(lose);
    }

    public void PlayWinSound() {
        PlaySound(win);
    }

    public void PlaySuccessModalSound() {
        PlaySound(successModal);
    }
}
