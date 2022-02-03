using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioTrack
{
    public string name;
    public AudioSource source;
    [Range(0, 1)]
    public float volume = 1f;
}
