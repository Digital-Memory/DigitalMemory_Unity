using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacherWatch : Attacher
{
    [SerializeField] GameObject goBackInTimeArrow;

    private void Start()
    {
        if (goBackInTimeArrow != null) goBackInTimeArrow.SetActive(isAttached);
    }

    public override void OnAttach(IAttachable attachable)
    {
        base.OnAttach(attachable);
        if (goBackInTimeArrow != null) goBackInTimeArrow.SetActive(isAttached);
    }

    public override void OnDetach()
    {
        base.OnDetach();
        if (goBackInTimeArrow != null) goBackInTimeArrow.SetActive(isAttached);
    }
}
