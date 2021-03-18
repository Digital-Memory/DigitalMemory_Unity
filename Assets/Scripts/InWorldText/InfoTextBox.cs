using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using NaughtyAttributes;

public class InfoTextBox : MonoBehaviour
{
    //[ResizableTextArea] public string titleText;
    [ResizableTextArea] public string contentText;
    [SerializeField] int titleFontSize, contentFontSize;
    [SerializeField] Quaternion rotation;

    [SerializeField] TextMeshPro titleBox, contentBox;


    public void Init(string text)
    {
        contentText = text;
        //UpdateTextAndFont(titleBox, titleText, titleFontSize);
        UpdateTextAndFont(contentBox, contentText, contentFontSize);
    }

    private void Update()
    {
        transform.rotation = rotation;
    }

    void UpdateTextAndFont(TextMeshPro box, string text, int fontsize) {

        box.text = text;
        box.fontSize = fontsize;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
