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

    public void AddItem(InventoryObjectData data)
    {
        Instantiate(ItemPrefab, transform).Init(data);
    }

    public void OnMouseEnter()
    {
        Debug.Log("Start Hover Inventory");

        Game.HoverHandler.EnterUI();
        mouseAbove = true;
    }

    public void OnMouseExit()
    {
        Debug.Log("End Hover Inventory");

        Game.HoverHandler.ExitUI();
        mouseAbove = false;
    }
}
