using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartTextDisplayer : MonoBehaviour
{
    [SerializeField] TMP_Text textBox;
    [SerializeField] StartTextConfirmer startTextConfirmer;
    [SerializeField] StartTextConfirmer jaCustomConfirmer;
    [SerializeField] AudioSource writing;

    [Button]
    public void Display(string textToDisplay, bool customConfirm = false)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayRoutine(textToDisplay, customConfirm));
    }

    private IEnumerator DisplayRoutine(string textToDisplay, bool customConfirm)
    {
        startTextConfirmer.gameObject.SetActive(false);
        writing.Play();

        string str = "";
        int length = textToDisplay.Length;

        for (int i = 0; i <= length; i++)
        {
            str = textToDisplay.Substring(0, i);
            str += "<alpha=#00>";
            str += textToDisplay.Substring(i, length - i);
            textBox.text = str;
            yield return new WaitForSeconds(0.05f + Random.Range(-0.01f,0.01f));
        }
        writing.Pause();
        yield return new WaitForSeconds(0.5f);

        (customConfirm ? (jaCustomConfirmer) : (startTextConfirmer)).gameObject.SetActive(true);
    }
}
