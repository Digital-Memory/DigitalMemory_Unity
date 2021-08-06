using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialKitchen : MonoBehaviour
{
    [SerializeField] CollectToInventoryOnClick meat;
    [SerializeField] Attacher meatAttacher;

    [SerializeField] List<TutorialFrame> tutorialFrames;

    [SerializeField] GameObject buttonBack, buttonDone, buttonContinue;

    [SerializeField] private Canvas myCanvas;
    [SerializeField] ZoomIn KitchenZoomInTemp;

    private int currentFrame = 0, unlockedFrames = 0;
    private void Start()
    {
        SetTutorialFrame(0);
        meat.InteractEvent += OnInteractWithMeat;
        meatAttacher.OnChangeAttached += OnAttachMeat;

    }

    private void OnInteractWithMeat()
    {
        meat.InteractEvent -= OnInteractWithMeat;
        SetTutorialFrame(1);
        unlockedFrames = 1;
    }
    private void OnAttachMeat(bool isAttached, string attachment)
    {
        if (isAttached)
        {
            meatAttacher.OnChangeAttached -= OnAttachMeat;
            SetTutorialFrame(2);
            unlockedFrames = 2;
        }
    }

    private void SetTutorialFrame(int index)
    {
        for (int i = 0; i < tutorialFrames.Count; i++)
        {
            if (index == i)
            {
                tutorialFrames[i].gameObject.SetActive(true);
                tutorialFrames[i].FadeIn();
                currentFrame = i;
            } 
            else
            {
                tutorialFrames[i].FadeOut();
            }
        }

        UpdateButtons();
    }

    public void ToggleTutorial()
    {
        Debug.LogWarning($"myCanvas is null: {myCanvas == null}");
        myCanvas.enabled = !myCanvas.enabled;
    }

    public void GoFrameBack()
    {
        SetTutorialFrame(currentFrame-1);
        Debug.Log(currentFrame);
    }

    public void GoFrameForward()
    {
        SetTutorialFrame(currentFrame+1);
    }

    private void UpdateButtons()
    {
        buttonBack.SetActive(currentFrame > 0);

        buttonContinue.SetActive(currentFrame < unlockedFrames);

        buttonDone.SetActive(currentFrame == tutorialFrames.Count-1);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(50,50,100,50) ,"Skip Tutorial"))
        {
            KitchenZoomInTemp.Try();
            Destroy(gameObject);
        }
    }
}
