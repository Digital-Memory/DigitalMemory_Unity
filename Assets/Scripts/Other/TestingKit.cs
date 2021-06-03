using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingKit : MonoBehaviour
{
    [SerializeField] InventoryObjectData[] inventoryObjects;

    private void OnGUI()
    {
        int indexMax = inventoryObjects.Length - 1;
        int index = 0;

        while (index <= indexMax)
        {
            InventoryObjectData obj = inventoryObjects[index];
            if (GUI.Button(new Rect(10, 10 + 30 * index, 120, 20), obj.name))
                Game.UIHandler.InventoryAdder.MoveToInventory(obj, Input.mousePosition);

            index++;
        }
    }
}
