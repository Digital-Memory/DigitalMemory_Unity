using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHandler : Singleton<UIHandler>
{
    public CustomCursorHandler CustomCursor;
    public InventoryAdder InventoryAdder;
    public QuoteDisplayer QuoteDisplayer;
    public TooltipDisplayer Tooltip;
    public EventSystem EventSystem;
}
