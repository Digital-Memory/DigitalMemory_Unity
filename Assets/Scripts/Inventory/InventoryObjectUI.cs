using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryObjectUI : UnityEngine.UI.Button, IDragHandler, IBeginDragHandler
{
    [SerializeField] private Image imageElement;
    [SerializeField] private Text amountText;
    private InventoryObjectData data;
    private Inventory inventory;

    [ShowNonSerializedField] int amount = 0;

    public void Init(Inventory inventory, InventoryObjectData data)
    {
        if (data != null)
        {
            this.inventory = inventory;
            this.data = data;

            ChangeAmount(1);

            if (imageElement != null)
                imageElement.sprite = data.icon;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject go = Instantiate(data.prefab);
        float scaleMultiplier = Game.ZoomInHandler.IsZoomedIn ? data.zoomInSceneScaleMultiplier : data.overviewSceneScaleMultiplier;
        go.transform.localScale = scaleMultiplier * Vector3.one;
        IDragable dragable = go.GetComponent<IDragable>();
        if (dragable != null)
        {
            Game.DragHandler.StartDrag(new RaycastHit(), dragable, dragable as IAttachable);
            ChangeAmount(-1);
        }
        else
        {
            Destroy(go);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Game.UIHandler.Tooltip.Show(gameObject, data.hoverText);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        Game.UIHandler.Tooltip.TryHide(gameObject);
    }

    internal void ChangeAmount(int change)
    {
        amount += change;

        if (amount <= 0)
        {
            inventory.RemoveLast(data);
            Game.UIHandler.Tooltip.TryHide(gameObject);
            Destroy(gameObject);
        }
        else
        {
            amountText.text = amount.ToString();
        }
    }
}
