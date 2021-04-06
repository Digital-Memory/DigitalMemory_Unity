using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObject : MonoBehaviour, IInventoryObject
{
    [Expandable]
    [SerializeField]
    InventoryObjectData inventoryObjectData;

    public InventoryObjectData GetData()
    {
        if (inventoryObjectData != null)
        {
            return inventoryObjectData;
        }

        Debug.LogError("No InventoryObjectData selected on " + gameObject.name);
        return null;
    }
}
