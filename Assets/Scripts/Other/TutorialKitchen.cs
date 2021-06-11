using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialKitchen : MonoBehaviour
{
    [SerializeField] CollectToInventoryOnClick meat;
    [SerializeField] Attacher meatAttacher;

    [SerializeField] List<GameObject> tutorialFrames;
    private void OnEnable()
    {
        SetTutorialFrame(0);
        meat.InteractEvent += OnInteractWithMeat;
        meatAttacher.OnChangeAttached += OnAttachMeat;
    }

    private void OnInteractWithMeat()
    {
        meat.InteractEvent -= OnInteractWithMeat;
        SetTutorialFrame(1);
    }
    private void OnAttachMeat(bool isAttached, string attachment)
    {
        if (isAttached)
        {
            meatAttacher.OnChangeAttached -= OnAttachMeat;
            SetTutorialFrame(2);
        }
    }

    private void SetTutorialFrame(int index)
    {
        for (int i = 0; i < tutorialFrames.Count; i++)
        {
            tutorialFrames[i].SetActive(i == index);
        }
    }
}
