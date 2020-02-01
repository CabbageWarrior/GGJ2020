using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    private AudioSource source;

    public AudioClip[] music;
    public AudioClip[] sfx;
    public AudioClip[] cowSound;
    [Space]
    public AudioSource[] musicAudioSources = null;
    public AudioSource[] sfxAudioSources = null;
    public AudioSource[] cowsAudioSources = null;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayMusic(int audio)
    {
        //source.PlayOneShot(music[audio]);
        musicAudioSources[audio].Play();
    }

    public void PlaySfx(int audio)
    {
        PlaySfx(audio, 1);
    }
    public void PlaySfx(int audio, float volume)
    {
        //source.PlayOneShot(sfx[audio], volume);
        sfxAudioSources[audio].volume = volume;
        sfxAudioSources[audio].Play();
    }

    public void PlayCowSound(int audio)
    {
        //source.PlayOneShot(cowSound[audio]);
        cowsAudioSources[audio].Play();
    }
}
