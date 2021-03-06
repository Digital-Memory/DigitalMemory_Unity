using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipDisplayer : MonoBehaviour
{
    [SerializeField] TMP_Text tooltipText;
    [SerializeField] Image tooltipBackground;
    GameObject toolTipSource;

    internal void Show(GameObject source, string hoverText)
    {
        if (tooltipText.enabled == false)
            tooltipText.enabled = true;

        if (tooltipBackground.enabled == false)
            tooltipBackground.enabled = true;

        toolTipSource = source;
        tooltipText.text = hoverText;

        bool alignLeft = (Input.mousePosition.x / (float)Screen.width) < 0.5f;
        (transform as RectTransform).pivot = alignLeft ? new Vector2(0, 0.5f ) : new Vector2(1, 0.5f);
        (transform as RectTransform).anchoredPosition = Vector2.zero;
        tooltipText.alignment = alignLeft ? TextAlignmentOptions.MidlineLeft : TextAlignmentOptions.MidlineRight;
    }

    internal void TryHide(GameObject source)
    {
        if (toolTipSource == source){
            toolTipSource = null;
            tooltipText.text = "";
            tooltipText.enabled = false;
            tooltipBackground.enabled = false;
        }
    }
}
