using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
[RequireComponent(typeof(AudioSource))]

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] float fadeOutTime = 1f;
    public List<AudioTrack> tracks;

    AudioTrack activeTrack;
    AudioSource clipSource;

    bool isPlayingMusic = false;

    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        isPlayingMusic = false;
    }

    public void ToggleMusic()
    {
        Debug.Log("Music was toggled");
        EventManager.RaiseOnTriggerAudio(Music.Greenzone);
    }

    private void OnEnable()
    {
        EventManager.OnTriggerAudio += HandleTriggerAudio;
    }

    private void OnDisable()
    {
        EventManager.OnTriggerAudio -= HandleTriggerAudio;
    }

    private void Start()
    {
        clipSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip(AudioClipSO clipSO)
    {
        clipSource.PlayOneShot(clipSO.clip);
    }

    void HandleTriggerAudio(Music track)
    {
            if (isPlayingMusic)
            {
                FadeOutAudioTrack();
                isPlayingMusic = false;
            }
            else
            {
                StartAudioTrack(tracks[0]);
                isPlayingMusic = true;
            }
    }

    void StartAudioTrack(AudioTrack track)
    {
        Debug.Log("Starting music...");
        activeTrack = track;
        track.source.volume = track.volume;
        track.source.Play();
    }

    void FadeOutAudioTrack()
    {
        Debug.Log("EndingMusic");
        //StartCoroutine(FadeOutAudioTrackSequence());

        activeTrack.source.Stop();
    }

    IEnumerator FadeOutAudioTrackSequence()
    {
        float startingVolume = activeTrack.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutTime)
        {
            activeTrack.source.volume = Mathf.Lerp(startingVolume, 0f, elapsedTime / fadeOutTime);

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        activeTrack.source.Stop();
    }
}

public enum Music
{ 
    Greenzone,
    Redzone,
    Bluezone,
    Boss,
    Title,
    Credits
};
