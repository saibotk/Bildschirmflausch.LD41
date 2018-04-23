﻿
using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioControl : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource maintheme;
    public AudioSource menutheme;
    public AudioSource gameovers;
    public AudioSource[] soundeffects;

    public enum Sfx { shoot, explosion, mobattack, door, faster, slower, driving, slowdriving };

    private const float lvlbgm = 6.79f;
    private const float gobgm = 14.406f;
    private const float loopdelay = 0.05f;
    private bool menu;

    // Use this for initialization
    void Start()
    {
        maintheme.loop = true;
        menutheme.loop = true;
        gameovers.loop = true;
    }

    private void Update()
    {
        if (maintheme.time >= maintheme.clip.length - loopdelay)
            maintheme.time = lvlbgm;
        if (gameovers.time >= gameovers.clip.length - loopdelay)
            gameovers.time = gobgm;
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
        menutheme.Play();
    }

    public void LevelBgm()
    {
        maintheme.Play();
    }

    public void SfxPlay(int sound)
    {
        soundeffects[sound].Play();
    }

	public void SfxPlay(Sfx sound) {
		Debug.Log("Playing " + sound + " " + (int)sound);
		SfxPlay((int)sound);
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

