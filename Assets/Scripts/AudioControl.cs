
using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioControl : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioMixerSnapshot maintheme;
    public AudioMixerSnapshot end;
    public AudioSource gameovers;
    public AudioSource[] soundeffects;

    public enum Sfx { shoot, explosion, mobattack};

    private float m_TransitionIn;
    private float m_TransitionOut;
    private bool endstate;

    // Use this for initialization
    void Start()
    {
        m_TransitionIn = 1.2f;
        m_TransitionOut = 0.3f;
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
}
