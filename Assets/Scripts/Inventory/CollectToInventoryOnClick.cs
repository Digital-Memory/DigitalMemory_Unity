using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectToInventoryOnClick : ConditionedObject, IClickable, IHoverable
{
    [SerializeField] InventoryObjectData dataToMoveToInventory;
    public bool IsNull => this == null;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;
    public event Action OnClickEvent;

    [TextArea] [SerializeField] string tooltipText;

    public void Click()
    {
        if (CheckAllConditionsForTrue())
        {
            OnClickEvent?.Invoke();
            Game.UIHandler.InventoryAdder.MoveToInventory(dataToMoveToInventory, Input.mousePosition);
            Game.HoverHandler.ForceEndHoverCurrent();
            Destroy(gameObject);
        }
    }

    public void EndHover()
    {
        OnEndHoverEvent?.Invoke();
    }

    public string GetTooltipText()
    {
        return tooltipText;
    }

    public bool IsClickable()
    {
        return true;
    }

    public void StartHover()
    {
        OnStartHoverEvent?.Invoke();
    }
}
