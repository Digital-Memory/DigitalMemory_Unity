using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoWall : ConditionedObject
{
    [SerializeField] Material materialTEMP;
    [SerializeField] VideoPlayer videoPlayer;

    public override bool Try(bool on)
    {
        if (base.Try(on))
        {
            PlayVideo();
            Debug.Log("Play");
            return true;
        }

        Debug.Log("Dont Play");
        return false;
    }

    private void PlayVideo()
    {
        videoPlayer.Play();
    }
}
