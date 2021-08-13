using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IAttacher : IHoverable
{
    bool AllowsDetach { get; }

    bool CanAttach(string attachBehaviour);
    Transform GetTransform();
    void OnAttach(IAttachable attachable);
    void OnDetach();
    Vector3 GetPreviewPosition(Vector3 point);
    Vector3 GetPreviewDirectionVector();
    bool ResetPositionOnAttach();
    bool ResetOrientationOnAttach();
    Vector3 GetAttachOffset();
    void HandleTransformOnAttach(Transform transformAttached);
}

public class Attacher : MonoBehaviour, IAttacher
{
    public string attachmentName;
    [SerializeField] protected bool isAttached;
    [SerializeField] Vector3 attachmentOffset;

    [Dropdown("VectorValues")]
    public Vector3 attachPreviewVector = Vector3.up;
    private Vector3[] VectorValues = new Vector3[] { Vector3.right, Vector3.left, Vector3.forward, Vector3.back, Vector3.up, Vector3.down };
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private AnimationCurve blendShapeCurve = AnimationCurve.EaseInOut(0, 0, 1, 50);

    public event System.Action<bool, string> OnChangeAttached;
    public event Action OnStartHoverEvent;
    public event Action OnEndHoverEvent;

    public bool IsAttached => isAttached;

    public bool allowsDetach = false;
    public bool destroyColliderOnAttachment = false;

    [TextArea] [SerializeField] string tooltipText;

    [HideInInspector] public bool AllowsDetach => allowsDetach;

    public bool IsNull => this == null;

    protected void Start ()
    {
        //Need to Improve this at some point
        gameObject.layer = 7;
        IAttachable attachable = GetComponentInChildren<IAttachable>();
        isAttached = attachable != null;
        OnChangeAttached?.Invoke(isAttached, attachable != null ? attachable.GetAttachment() : "");
    }

    private void OnEnable()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        if (!isAttached)
            SparkleEffectHandler.OnAttacherDetach(this);

    }

    protected void OnDisable()
    {
        SparkleEffectHandler.OnAttacherAttach(this);
    }

    public bool CanAttach(string attachmentName)
    {
        return (this.attachmentName == attachmentName || this.attachmentName == "") && !isAttached;
    }

    public Vector3 GetAttachOffset()
    {
        return attachmentOffset;
    }

    public virtual Vector3 GetPreviewPosition(Vector3 point)
    {
        return transform.position;
    }

    public Vector3 GetPreviewDirectionVector()
    {
        return attachPreviewVector;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public virtual void OnAttach(IAttachable attachable)
    {
        isAttached = true;
        OnChangeAttached?.Invoke(isAttached, attachable.GetAttachment());
        SparkleEffectHandler.OnAttacherAttach?.Invoke(this);
        StopAllCoroutines();
        StartCoroutine(AnimateBlendShape(speed: 3f, target: 1f));
    }

    public virtual void OnDetach()
    {
        isAttached = false;
        OnChangeAttached?.Invoke(isAttached, "");
        SparkleEffectHandler.OnAttacherDetach?.Invoke(this);
        StopAllCoroutines();
        StartCoroutine(AnimateBlendShape(speed: -3f, target: 0f));
    }
    private IEnumerator AnimateBlendShape(float speed, float target)
    {
        float current = 1 - target;
        while (speed > 0 ? (current < target) : (current > target))
        {
            current += Time.deltaTime * speed;
            skinnedMeshRenderer.SetBlendShapeWeight(0, blendShapeCurve.Evaluate(current));
            yield return null;
        }
    }

    public bool ResetOrientationOnAttach()
    {
        return true;
    }

    public bool ResetPositionOnAttach()
    {
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + attachmentOffset, 0.5f * Game.Settings.CurrentZoomLevel);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + attachPreviewVector);
    }

    public void HandleTransformOnAttach(Transform transformToAttach)
    {
        transformToAttach.parent = transform;
        transformToAttach.localPosition = attachmentOffset;
        transformToAttach.localRotation = Quaternion.identity;

        if (destroyColliderOnAttachment)
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                Destroy(collider);
            }
        }
    }

    public void StartHover()
    {
        OnStartHoverEvent?.Invoke();
    }

    public void EndHover()
    {
        OnEndHoverEvent?.Invoke();
    }

    public string GetTooltipText()
    {
        return tooltipText;
    }
}
