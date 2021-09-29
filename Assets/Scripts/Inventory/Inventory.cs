using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static System.Action<InventoryObjectUI> OnAddToInventory;

    [SerializeField] InventoryObjectUI ItemPrefab;

    Dictionary<InventoryObjectData, InventoryObjectUI> content = new Dictionary<InventoryObjectData, InventoryObjectUI>();

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

        GameObject dragging = dragable.gameObject;
        IInventoryObject inventoryObject = dragging.GetComponent<IInventoryObject>();

        if (inventoryObject != null)
        {
            Destroy(dragging);
            AddItem(inventoryObject.GetData());
        }
    }

    public void RemoveLast(InventoryObjectData data)
    {
        if (content.ContainsKey(data))
        {
            content.Remove(data);
        }
    }

    public void AddItem(InventoryObjectData data)
    {
        if (content.ContainsKey(data))
        {
            content[data].ChangeAmount(1);
        }
        else
        {
            InventoryObjectUI uiElement = Instantiate(ItemPrefab, transform);
            uiElement.Init(this,data);
            content.Add(data, uiElement);
            OnAddToInventory?.Invoke(uiElement);
        }
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
