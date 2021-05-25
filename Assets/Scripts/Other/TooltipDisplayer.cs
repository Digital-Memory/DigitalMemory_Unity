using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipDisplayer : MonoBehaviour
{
    [SerializeField] TMP_Text tooltipText;
    GameObject toolTipSource;

    internal void Show(GameObject source, string hoverText)
    {
        if (tooltipText.enabled == false)
            tooltipText.enabled = true;

        toolTipSource = source;
        tooltipText.text = hoverText;

        bool alignLeft = (Input.mousePosition.x / (float)Screen.width) < 0.5f;
        tooltipText.rectTransform.pivot = alignLeft ? new Vector2(0, 0.5f ) : new Vector2(1, 0.5f);
        tooltipText.alignment = alignLeft ? TextAlignmentOptions.MidlineLeft : TextAlignmentOptions.MidlineRight;
    }

    internal void TryHide(GameObject source)
    {
        if (toolTipSource == source){
            toolTipSource = null;
            tooltipText.text = "";
            tooltipText.enabled = false;
        }
    }
}
