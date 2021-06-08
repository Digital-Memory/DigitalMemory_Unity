using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HiddenAttachable : SimpleAttachable
{
    ICloseupable hiddenIn;
    [SerializeField] bool isHidden;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (isHidden)
        {
            hiddenIn = transform.parent.GetComponentInParent<ICloseupable>();
            hiddenIn.OnStartCloseupEvent += OnParentStartCloseup;
            hiddenIn.OnEndCloseupEvent += OnParentEndCloseup;
        }
    }

    protected void OnDisable()
    {
        if (isHidden)
        {
            hiddenIn.OnStartCloseupEvent -= OnParentStartCloseup;
            hiddenIn.OnEndCloseupEvent -= OnParentEndCloseup;
        }
    }

    private void Start()
    {
        if (isHidden)
        {
            SetPhysicsActive(false);
            SetMouseRaycastable(false);
        }
    }

    void OnParentStartCloseup()
    {
        SetMouseRaycastable(true);
    }

    void OnParentEndCloseup()
    {
        if (isHidden)
        {
            SetMouseRaycastable(false);
        }
    }
}
