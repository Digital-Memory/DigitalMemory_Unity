using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextData : ScriptableObject
{
    [ResizableTextArea]
    public string text;
}
