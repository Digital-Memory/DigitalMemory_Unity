using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryAdder : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] RectTransform inventoryRect;
    [CurveRange(0,0,1,1)]
    [SerializeField] AnimationCurve moveToInventoryCurve;

    [SerializeField] Image image;

    [SerializeField] InventoryObjectData dataTEMP;
   
    float time;
    float endTime;
    bool isMoving;
    Vector3 currentlyMovedStartPosition;
    InventoryObjectData currentlyMovedData;
    public void MoveToInventory(InventoryObjectData objectData, Vector3 positionInUISpace)
    {
        if (isMoving)
        {
            EndMoving(currentlyMovedData, hide: false);
        } else
        {
            SetHidden(false);
        }

        currentlyMovedStartPosition = positionInUISpace;
        currentlyMovedData = objectData;
        image.sprite = objectData.icon;
        isMoving = true;
        endTime = moveToInventoryCurve.keys[moveToInventoryCurve.length - 1].time;
        time = 0;
    }

    private void SetHidden(bool hidden)
    {
        transform.localScale = hidden ? Vector3.zero : Vector3.one;
    }

    public void EndMoving(InventoryObjectData currentlyMovedData, bool hide = true)
    {
        if (hide)
            SetHidden(true);

        inventory.AddItem(currentlyMovedData);
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(currentlyMovedStartPosition, inventoryRect.position,moveToInventoryCurve.Evaluate(time));
            time += Time.deltaTime;

            if (time > endTime)
            {
                isMoving = false;
                EndMoving(currentlyMovedData);
            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveToInventory(dataTEMP,Input.mousePosition);
        }
    }
}
