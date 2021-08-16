using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class StartTextDisplayer : MonoBehaviour
{
    [SerializeField] TMP_Text textBox;
    [SerializeField] StartTextConfirmer startTextConfirmer;
    [SerializeField] StartTextConfirmer jaCustomConfirmer;
    [SerializeField] AudioSource writing;

    string textToDisplay;
    bool customConfirm;
    private bool isSkipable;

    [Button]
    public void Display(string textToDisplay, bool customConfirm = false)
    {
        StopAllCoroutines();
        this.textToDisplay = textToDisplay;
        this.customConfirm = customConfirm;
        StartCoroutine(DisplayRoutine());
    }

    private IEnumerator DisplayRoutine()
    {
        startTextConfirmer.gameObject.SetActive(false);
        writing.Play();

        string str = "";
        int length = textToDisplay.Length;

        for (int i = 0; i <= length; i++)
        {
            isSkipable = true;
            str = DisplayTextWithTypeEffect(length, i);
            yield return new WaitForSeconds(0.05f + Random.Range(-0.01f, 0.01f));
        }

        writing.Pause();

        yield return new WaitForSeconds(0.5f);
        EnableConfirmButton();
    }

    private void EnableConfirmButton()
    {
        (customConfirm ? (jaCustomConfirmer) : (startTextConfirmer)).gameObject.SetActive(true);
    }

    private void Update()
    {
        if (isSkipable && Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            writing.Pause();
            isSkipable = false;
            DisplayTextWithTypeEffect(textToDisplay.Length, textToDisplay.Length);
            EnableConfirmButton();
        }
    }

    private string DisplayTextWithTypeEffect(int amountOfCharactersTotal, int amountOfCharactersVisible)
    {
        string str = textToDisplay.Substring(0, amountOfCharactersVisible);
        str += "<alpha=#00>";
        str += textToDisplay.Substring(amountOfCharactersVisible, amountOfCharactersTotal - amountOfCharactersVisible);
        textBox.text = str;
        return str;
    }
}
