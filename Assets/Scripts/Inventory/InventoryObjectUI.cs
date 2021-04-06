using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryObjectUI : UnityEngine.UI.Button, IDragHandler
{
    [SerializeField]
    private Image imageElement;
    private InventoryObjectData data;

    public void Init(InventoryObjectData data)
    {
        if (data != null)
        {
            this.data = data;
            if (imageElement != null)
                imageElement.sprite = data.icon;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        GameObject go = Instantiate(data.prefab);
        IDragable dragable = go.GetComponent<IDragable>();
        if (dragable != null)
        {
            Game.DragHandler.StartDrag(new RaycastHit(), dragable, dragable as IAttachable);
            Destroy(gameObject);
        } else
        {
            Destroy(go);
        }
    }
}
