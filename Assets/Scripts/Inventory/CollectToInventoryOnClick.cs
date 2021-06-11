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

    public event Action InteractEvent;

    public void Click()
    {
        if (CheckAllConditionsForTrue())
        {
            InteractEvent?.Invoke();
            Game.UIHandler.InventoryAdder.MoveToInventory(dataToMoveToInventory, Input.mousePosition);
            Game.HoverHandler.ForceEndHoverCurrent();
            Destroy(gameObject);
        }
    }

    public void EndHover()
    {
        OnEndHoverEvent?.Invoke();
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
