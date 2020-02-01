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
        source.PlayOneShot(music[audio]);
    }

    public void PlaySfx(int audio, float volume = 1)
    {
        source.PlayOneShot(sfx[audio], volume);
    }

    public void PlayCowSound(int audio)
    {
        source.PlayOneShot(cowSound[audio]);
    }
}
