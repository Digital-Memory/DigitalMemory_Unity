using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundPlayer : Singleton<SoundPlayer>
{
    public bool Muted { get; set; }

    List<PlayingAudio> currentlyPlaying = new List<PlayingAudio>();
    public void Play(AudioClip clip, GameObject toPlayFrom = null, float volume = 1, float randomPitchRange = 0, bool playOnlyIfFinished = false, float pitch = -1f)
    {
        if (clip == null || Muted)
            return;

        if (playOnlyIfFinished && ClipIsBeingPlayed(clip))
            return;

        PlayingAudio newAudio = new PlayingAudio();

        if (toPlayFrom != null)
        {
            StopAllSoundsFromSource(toPlayFrom);
            newAudio.origin = toPlayFrom;
        }

        newAudio.source = gameObject.AddComponent<AudioSource>();
        newAudio.source.clip = clip;
        newAudio.source.volume = volume;
        newAudio.source.pitch = (pitch == -1) ? (1f + UnityEngine.Random.Range(-randomPitchRange, randomPitchRange)) : pitch;
        newAudio.source.Play();
        newAudio.coroutine = StartCoroutine(WaitForEndOfClipRoutine(newAudio));
        currentlyPlaying.Add(newAudio);

    }

    private bool ClipIsBeingPlayed(AudioClip clip)
    {
        foreach (var playing in currentlyPlaying)
        {
            if (playing != null && playing.source != null && playing.source.clip == clip)
                return true;
        }

        return false;
    }

    private void StopAllSoundsFromSource(GameObject toPlayFrom)
    {
        foreach (var playing in currentlyPlaying.Where(g => g.origin == toPlayFrom))
        {
            Destroy(playing.source);
            StopCoroutine(playing.coroutine);
        }
    }

    private IEnumerator WaitForEndOfClipRoutine(PlayingAudio playingAudio)
    {
        yield return new WaitForSeconds(playingAudio.source.clip.length);
        Destroy(playingAudio.source);
        currentlyPlaying.Remove(playingAudio);
    }
}

public class PlayingAudio
{
    public GameObject origin;
    public Coroutine coroutine;
    public AudioSource source;
}
