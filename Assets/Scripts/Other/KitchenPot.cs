using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenPot : Hatch
{
    [SerializeField] InventoryObjectData potData;
    [SerializeField] AnimatingObject cart;

    protected override void EndAnimating()
    {
        base.EndAnimating();
        StartCoroutine(FinishPotRoutine());
    }

    private IEnumerator FinishPotRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        Vector2 pos = Game.CameraController.Camera.WorldToScreenPoint(transform.position);
        Game.UIHandler.InventoryAdder.MoveToInventory(potData,pos);
        cart.Try(false);
        Destroy(gameObject);
    }
}
