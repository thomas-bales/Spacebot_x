using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewAudioClipSO", menuName = "ScriptableObjects/AudioClipSO")]
public class AudioClipSO : ScriptableObject
{
    public AudioClip clip;
}
