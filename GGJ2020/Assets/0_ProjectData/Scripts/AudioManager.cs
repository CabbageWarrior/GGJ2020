using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource source;

    public AudioClip[] music;
    public AudioClip[] sfx;
    public AudioClip[] cowSound;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(int audio)
    {
        source.PlayOneShot(music[audio]);
    }
}
