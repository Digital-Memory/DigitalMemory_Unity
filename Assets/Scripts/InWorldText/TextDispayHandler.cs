using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDispayHandler : Singleton<TextDispayHandler>
{
    [SerializeField] InfoTextBox textBoxPrefab, currentTextBox;

    public void DisplayText(string text, Transform sourceTransform)
    {
        ClearTexts();
        currentTextBox = Instantiate(textBoxPrefab, sourceTransform.position, Quaternion.identity, sourceTransform);
        currentTextBox.Init(text);
    }

    public void ClearTexts()
    {
        if (currentTextBox != null)
            Destroy(currentTextBox.gameObject);
    }
}