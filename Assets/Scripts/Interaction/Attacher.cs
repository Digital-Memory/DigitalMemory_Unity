using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacher
{
    bool CanAttach(string attachBehaviour);
    Transform GetTransform();
    void OnAttach(IAttachable attachable);
    void OnDetach();
    Vector3 GetPreviewPosition(Vector3 point);
    bool ResetPositionOnAttach();
    bool ResetOrientationOnAttach();
    Vector3 GetAttachOffset();
}

public class Attacher : MonoBehaviour, IAttacher
{
    public string attachmentName;
    [SerializeField] protected bool isAttached;
    [SerializeField] Vector3 attachmentOffset;
    public event System.Action<bool, string> OnChangeAttached;
    public bool IsAttached => isAttached;

    protected void Start()
    {
        IAttachable attachable = GetComponentInChildren<IAttachable>();
        isAttached = attachable != null;
        OnChangeAttached?.Invoke(isAttached, attachable != null ? attachable.GetAttachment() : "");
    }

    public bool CanAttach(string attachmentName)
    {
        return this.attachmentName == attachmentName || this.attachmentName == "";
    }

    public Vector3 GetAttachOffset()
    {
        return attachmentOffset;
    }

    public virtual Vector3 GetPreviewPosition(Vector3 point)
    {
        return transform.position;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public virtual void OnAttach(IAttachable attachable)
    {
        isAttached = true;
        OnChangeAttached?.Invoke(isAttached, attachable.GetAttachment());
    }

    public virtual void OnDetach()
    {
        isAttached = false;
        OnChangeAttached?.Invoke(isAttached, "");
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
        Gizmos.DrawWireSphere(transform.position + attachmentOffset, 0.5f);
    }
}
