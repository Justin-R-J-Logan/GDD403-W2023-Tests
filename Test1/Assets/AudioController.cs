using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    //Singleton instance of this controller. Multiples will glitch the system.
    public static AudioController Instance;

    //Audio Clips and Source
    public AudioClip[] clips;
    private AudioSource source;

    public void Start()
    {
        Instance = this;
        source = GetComponent<AudioSource>();

        //Find automagic load this somehow later if need be
        //clips = new AudioClips[5];
        //
        //clips[(int)CLIPS.WIN] = Resources.Load<AudioClip>("Path To Clip");
        //The above code didn't work, leaving example incase I can get working later.
    }

    //Plays selected audio clips
    public void PlaySound(CLIPS c)
    {
        source.clip = clips[(int)c];
        source.Play();
    }
}

//Enum of all sounds in game.
public enum CLIPS
{
    WIN = 0,
    LOSE = 1,
    SHUFFLE = 2,
    MATCH = 3,
    MATCH_FAIL = 4,
}
