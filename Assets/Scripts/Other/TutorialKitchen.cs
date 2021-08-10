using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialKitchen : MonoBehaviour
{
    [SerializeField] CollectToInventoryOnClick meat;
    [SerializeField] Attacher[] kitchenPotFoodAttachers;
    [SerializeField] Button[] kitchenButtons;
    [SerializeField] FloatSender kitchenLever;

    [SerializeField] List<TutorialFrame> tutorialFrames;

    [SerializeField] GameObject buttonBack, buttonDone, buttonContinue;

    [SerializeField] private Canvas myCanvas;
    [SerializeField] ZoomIn KitchenTableZoomIn, KitchenZoomInTemp;

    private int currentFrame = 0, unlockedFrames = 0;
    private void Start()
    {
        for (int i = 0; i < tutorialFrames.Count; i++)
        {
            tutorialFrames[i].gameObject.SetActive(true);
        }

        SetTutorialFrame(0);
        meat.OnClickEvent += OnInteractWithMeat;
        foreach (var foodAttacher in kitchenPotFoodAttachers)
        {
            foodAttacher.OnChangeAttached += OnAttachMeat;
        }

        Inventory.OnAddToInventory += OnAddToInventory;

        foreach (var button in kitchenButtons)
        {
            button.OnClickEvent += OnUseButton;
        }

        kitchenLever.OnSendInputValue += OnUseLever;
    }

    private void OnAddToInventory(InventoryObjectUI inventoryObject)
    {
        inventoryObject.OnStartDrag += OnStartDragInventoryObject;
        Inventory.OnAddToInventory -= OnAddToInventory;
    }

    private void OnStartDragInventoryObject(InventoryObjectUI inventoryObject)
    {
        inventoryObject.OnStartDrag -= OnStartDragInventoryObject;
        SetTutorialFrame(2);
        unlockedFrames = 2;
    }

    private void OnInteractWithMeat()
    {
        meat.OnClickEvent -= OnInteractWithMeat;
        SetTutorialFrame(1);
        unlockedFrames = 1;
    }
    private void OnAttachMeat(bool isAttached, string attachment)
    {
        if (isAttached && unlockedFrames == 2)
        {
            foreach (var foodAttacher in kitchenPotFoodAttachers)
            {
                foodAttacher.OnChangeAttached -= OnAttachMeat;
            }

            KitchenTableZoomIn.Try();
            SetTutorialFrames(new int[] {3, 4});
            unlockedFrames = 3;
        }
    }

    private void OnUseButton()
    {
        tutorialFrames[3].FadeOut();

        foreach (var button in kitchenButtons)
        {
            button.OnClickEvent -= OnUseButton;
        }
    }

    private void OnUseLever(float value)
    {
        if (value == 1)
            return;

        kitchenLever.OnSendInputValue -= OnUseLever;
        tutorialFrames[4].FadeOut();
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

    private void SetTutorialFrames(int[] indexes)
    {
        for (int i = 0; i < tutorialFrames.Count; i++)
        {
            if (indexes.Contains(i))
            {
                tutorialFrames[i].gameObject.SetActive(true);
                tutorialFrames[i].FadeIn(duration: 1, delay: 2);
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
        SetTutorialFrame(currentFrame - 1);
        Debug.Log(currentFrame);
    }

    public void GoFrameForward()
    {
        SetTutorialFrame(currentFrame + 1);
    }

    private void UpdateButtons()
    {
        buttonBack.SetActive(currentFrame > 0);

        buttonContinue.SetActive(currentFrame < unlockedFrames);

        buttonDone.SetActive(currentFrame == tutorialFrames.Count - 1);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 100, 50), "Skip Tutorial"))
        {
            KitchenZoomInTemp.Try();
            Destroy(gameObject);
        }
    }
}
