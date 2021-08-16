using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShowTooltipOnHover : MonoBehaviour, IHoverable
{
    public bool IsNull => this == null;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

    [SerializeField] private string tooltipText;

    public void EndHover()
    {
        OnEndHoverEvent?.Invoke();
    }

    public string GetTooltipText()
    {
        return tooltipText;
    }

    public void StartHover()
    {
        OnStartHoverEvent?.Invoke();
    }
}
