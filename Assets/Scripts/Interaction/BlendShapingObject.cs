using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class BlendShapingObject : ChangingOverTimeObject
{
    [ShowNonSerializedField]
    SkinnedMeshRenderer skinnedMeshRenderer;

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }
#endif

    protected override void OnEnable()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        base.OnEnable();
    }

    protected override void UpdateChange(float progress)
    {
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(0,progress * 100f);
        }
    }
}