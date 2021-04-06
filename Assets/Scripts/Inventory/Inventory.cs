using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] InventoryObjectUI ItemPrefab;
    bool mouseAbove;

    private void OnEnable()
    {
        Game.DragHandler.OnEndDrag += OnEndDragAny;
    }

    private void OnDisable()
    {
        Game.DragHandler.OnEndDrag -= OnEndDragAny;
    }

    private void OnEndDragAny(IDragable dragable)
    {
        if (!mouseAbove)
            return;

        GameObject dragging = dragable.GetGameObject();
        IInventoryObject inventoryObject = dragging.GetComponent<IInventoryObject>();

        if (inventoryObject != null)
        {
            Destroy(dragging);
            AddItem(inventoryObject.GetData());
        }
    }

    private void AddItem(InventoryObjectData data)
    {
        Instantiate(ItemPrefab, transform).Init(data);
    }

    public void OnMouseEnter()
    {
        Game.HoverHandler.EnterUI();
        mouseAbove = true;
    }

    public void OnMouseExit()
    {
        Game.HoverHandler.ExitUI();
        mouseAbove = false;
    }
}
