
using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioControl : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource maintheme;
    public AudioSource gameovers;
    public AudioSource[] soundeffects;

    public enum Sfx { shoot, explosion, mobattack };

    private const float lvlbgm = 37.879f;
    private bool menu;

    // Use this for initialization
    void Start()
    {
        maintheme.loop = true;
        gameovers.loop = true;
        maintheme.Play();
    }

    private void Update()
    {
        if (maintheme.time >= 99.0f)
            maintheme.time = lvlbgm;
        if (gameovers.time >= 22.85f)
            gameovers.time = 3;
    }

    public void GameOverBgm()
    {
        maintheme.Stop();
        gameovers.Play();
    }

    public void MenuBgm()
    {
        if (gameovers.isPlaying)
            gameovers.Stop();
        maintheme.Play();
    }

    public void LevelBgm()
    {
        maintheme.time = lvlbgm;
    }

    public void SfxPlay(int sound)
    {
        soundeffects[sound].Play();
    }

    public void SfxStop(int sound)
    {
        soundeffects[sound].Stop();
    }
    public bool SfxPlaying(int sound)
    {
        return soundeffects[sound].isPlaying;
    }

    public void SetMasterVolume(float nvol)
    {
        mixer.SetFloat("masterVolume", Mathf.Clamp(nvol, -80f, 20f));
    }

    public void SetBgmVolume(float nvol)
    {
        mixer.SetFloat("bgmVolume", Mathf.Clamp(nvol, -80f, 20f));
    }

    public void SetSfxVolume(float nvol)
    {
        mixer.SetFloat("sfxVolume", Mathf.Clamp(nvol, -80f, 20f));
    }
}

