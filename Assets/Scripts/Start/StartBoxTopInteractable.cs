using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartBoxTopInteractable : MonoBehaviour, IClickable, IHoverable
{
    public bool IsNull => this == null;

    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;
    public event Action OnClickEvent;

    private bool hasMoved = false;
    [SerializeField] private Vector3 moveForce;
    [SerializeField] private Vector3 moveTorque;
    [SerializeField] private AudioClip woosh;

    [TextArea] [SerializeField] string tooltipText;

    public UnityEvent <StartBoxTopInteractable> OnMove;

    public void Click()
    {
        hasMoved = true;
        Move();
        OnClickEvent?.Invoke();
    }

    private void Move()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(moveForce, ForceMode.Impulse);
        rb.AddTorque(moveTorque, ForceMode.Impulse);
        Game.SoundPlayer.Play(woosh, volume: 0.5f, randomPitchRange: 0.25f);
        OnMove?.Invoke(this);
    }

    public void EndHover()
    {
        OnEndHoverEvent?.Invoke();
    }

    public bool IsClickable()
    {
        return !hasMoved;
    }

    public void StartHover()
    {
        OnStartHoverEvent?.Invoke();
    }

    public string GetTooltipText()
    {
        return tooltipText;
    }
}
