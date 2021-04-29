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
        Game.VideoPlayerHandler.Play(url, this);
        projectorEffects.SetActive(true);
    }

    internal void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }
}
