using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerHandler : Singleton<VideoPlayerHandler>
{
    [SerializeField] VideoClip loading;
    [SerializeField] Material videoMaterial, loadingMaterial;
    VideoPlayer videoPlayer;
    VideoWall videoWall;
    bool isLoading;
    private void OnEnable()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    public void Play(string url, VideoWall videoWall)
    {
        videoWall.SetMaterial(loadingMaterial);
        this.videoWall = videoWall;

        isLoading = true;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;
        videoPlayer.Prepare();
    }

    private void Update()
    {
        if (isLoading && videoPlayer.isPrepared == true)
        {
            videoWall.SetMaterial(videoMaterial);
            videoPlayer.Play();
            isLoading = false;
        }
    }
}
