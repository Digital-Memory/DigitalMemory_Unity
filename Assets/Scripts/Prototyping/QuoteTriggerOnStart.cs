using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuoteTriggerOnStart : MonoBehaviour
{
    [ResizableTextArea]
    [SerializeField] string quote;

    // Start is called before the first frame update
    void Start()
    {
        Game.UIHandler.QuoteDisplayer.Display(quote);
    }
}
