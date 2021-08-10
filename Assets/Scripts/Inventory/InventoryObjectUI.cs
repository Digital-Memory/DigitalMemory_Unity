using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryObjectUI : UnityEngine.UI.Button, IDragHandler, IBeginDragHandler
{
    public System.Action<InventoryObjectUI> OnStartDrag;

    [SerializeField] private Image imageElement;
    [SerializeField] private TMP_Text amountText;
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
        OnStartDrag?.Invoke(this);
        GameObject go = Instantiate(data.prefab);

        bool isZoomedIn = Game.ZoomInHandler.IsZoomedIn;

        float scaleMultiplier = isZoomedIn ? data.zoomInSceneScaleMultiplier : data.overviewSceneScaleMultiplier;
        float dragDistance = isZoomedIn ? data.zoomInSceneDistance : data.overviewSceneDistance;
        go.transform.localScale = scaleMultiplier * Vector3.one;
        IDragable dragable = go.GetComponent<IDragable>();
        if (dragable != null)
        {
            Game.DragHandler.StartDrag(new RaycastHit(), dragable, dragable as IAttachable, dragDistance);
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
