using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSoundOnStart : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] sounds;

	void Start ()
    {
        source.clip = sounds[Random.Range(0, sounds.Length)];
        source.Play();
	}
}
