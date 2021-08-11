using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroFlow : MonoBehaviour
{
    [SerializeField] ZoomIn zoomInAfterFinishIntro;
    [SerializeField] GameObject TutorialCanavas;

    private bool lightActive;
    [SerializeField] public bool LightActive;

    [SerializeField] Light directionalLight;
    [SerializeField] GameObject lamp, cone;
    [SerializeField] GameObject offLightPostProcessing;


    private void Awake()
    {
        Game.MouseInteractor.InteractionIsBlocked = true;
        Game.SoundPlayer.Muted = true;
    }

    private void SetLightActive(bool value)
    {
        directionalLight.enabled = value;
        lamp.SetActive(value);
        cone.SetActive(value);
        offLightPostProcessing.SetActive(!value);
    }

    private void DestroyTutorial()
    {
        TutorialCanavas.SetActive(true);
        Destroy(gameObject);
    }

    void FinishedIntro()
    {
        //zoomOverview.gameObject.SetActive(true);
        zoomInAfterFinishIntro.DoZoomIn();
        Game.MouseInteractor.InteractionIsBlocked = false;
        Game.SoundPlayer.Muted = false;
    }

    private void Update()
    {
        if (lightActive != LightActive)
        {
            SetLightActive(lightActive);
            lightActive = LightActive;
        }
    }
    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 100, 50), "Skip Intro"))
        {
            FinishedIntro();
            DestroyTutorial();
        }
    }
}
