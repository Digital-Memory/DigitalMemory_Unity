using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragHandler : Singleton<DragHandler>
{
    IDragable currentDrag;
    IAttachable currentAttachable;

    public bool IsDragging { get => (currentDrag != null); }
    public bool IsDraggingAttachable { get => (currentAttachable != null); }

    public event System.Action<IDragable, RaycastHit> OnStartDrag;
    public event System.Action OnEndDrag;

    public void UpdateDrag(RaycastHit hit, Ray ray)
    {
        IAttacher attacher = null;

        if (hit.collider != null)
            attacher = hit.collider.GetComponent<IAttacher>();

        if (IsDraggingAttachable && attacher != null && attacher.CanAttach(currentAttachable.GetAttachment()))
        {
            //attachment preview
            currentDrag.UpdateDragPosition(hit.point, attacher.GetPreviewPosition(hit.point) + Game.Settings.AttachPreviewOffset * (3.5f + Mathf.Sin(Time.time * 2f) * 0.5f));
        }
        else
        {
            //regular drag
            float dragDistance = Vector3.Distance(ray.origin, hit.point) - Game.Settings.DragDistanceToFloor;
            currentDrag.UpdateDragPosition(hit.point, ray.GetPoint(dragDistance));
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (IsDraggingAttachable && attacher != null && attacher.CanAttach(currentAttachable.GetAttachment()))
            {
                //attach
                Attach(currentAttachable, attacher);
            }
            else
            {
                //drop
                EndDrag(currentDrag, hit.point);
            }
        }
    }

    public void StartDrag(RaycastHit hit, IDragable dragable, IAttachable attachable)
    {
        currentDrag = dragable;
        currentAttachable = attachable;

        IAttacher attacher = (attachable == null ? null : attachable.GetCurrentAttached());
        if (attacher != null)
            attacher.OnDetach();

        dragable.StartDrag();

        OnStartDrag?.Invoke(dragable, hit);

    }

    private void EndDrag(IDragable dragable, Vector3 point)
    {
        currentAttachable = null;
        currentDrag = null;

        dragable.EndDrag(point + dragable.GetEndDragYOffset() * Vector3.up);

        OnEndDrag?.Invoke();
    }

    private void Attach(IAttachable attachable, IAttacher attacher)
    {
        currentAttachable = null;
        currentDrag = null;

        attacher.OnAttach(attachable);
        attachable.Attach(attacher);


        OnEndDrag?.Invoke();
    }

    public void ForceEndDrag()
    {
        EndDrag(currentDrag, Vector3.zero);
    }
}
