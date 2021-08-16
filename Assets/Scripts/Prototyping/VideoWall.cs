using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoWall : ConditionedObject
{
    [SerializeField] string url;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] GameObject projectorEffects;

    bool playedVideo = false;

    public override bool Try(float progress)
    {
        if (base.Try(progress))
        {
            if (!playedVideo)
                PlayVideo();

            return true;
        }

        return false;
    }


    public override bool Try()
    {
        if (base.Try())
        {
            if (!playedVideo)
                PlayVideo();
            else
                PauseVideo();
            return true;
        }

        return false;
    }

    public override bool Try(bool on)
    {
        if (base.Try())
        {
            if (on)
                if (!playedVideo)
                    PlayVideo();
            else
                PauseVideo();
            return true;
        }

        return false;
    }

    private void PlayVideo()
    {
        playedVideo = true;
        Game.VideoPlayerHandler.Play(url, this);

        if (projectorEffects != null)
            projectorEffects.SetActive(true);
    }
    private void PauseVideo()
    {
        Game.VideoPlayerHandler.Pause(this);
    }

    internal void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }
}
