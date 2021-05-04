using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuoteDisplayer : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Animator animator;

    public void Display(string quoute)
    {
        text.text = quoute;
        animator.SetTrigger("Play");
    }
}
