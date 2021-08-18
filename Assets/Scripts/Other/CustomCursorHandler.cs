using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum CustomCursorType
{
    DEFAULT,
    DRAGABLE,
    DRAGGING,
    MANUAL,
    X,
    ZOOMIN,
    ZOOMOUT,
}

public class CustomCursorHandler : Image
{
    Sprite before;
    CustomCursorType beforeType;

    bool cursorIsFree = true;
    bool rotateToLookAwayFromCenter = false;

    protected override void Start()
    {
        base.Start();
        Cursor.visible = false;
    }

    CustomCursorType currentType;
    [SerializeField] CustomCursorData[] cursors;

    public void SetCursorType(CustomCursorType customCursorType, Sprite sprite = default(Sprite), float size = 1f)
    {
        if (currentType != customCursorType)
        {
            CustomCursorData data = GetCursorDataByType(customCursorType);
            Sprite cursorSprite = customCursorType == CustomCursorType.MANUAL ? sprite : data.sprite;
            rotateToLookAwayFromCenter = customCursorType == CustomCursorType.ZOOMOUT;
            Vector2 pivot = new Vector2(data.offset.x / rectTransform.sizeDelta.x,1 + (data.offset.y / rectTransform.sizeDelta.y));
            SetCursor(customCursorType, cursorSprite, size, pivot);
        }
    }

    private CustomCursorData GetCursorDataByType(CustomCursorType customCursorType)
    {
        foreach (CustomCursorData data in cursors)
        {
            if (data.type == customCursorType)
                return data;
        }

        return default(CustomCursorData);
    }

    private void Update()
    {
        if (cursorIsFree)
            transform.position = Input.mousePosition;

        var vectorAwayFromCenter = ((Vector2)Input.mousePosition - new Vector2(Screen.width / 2, Screen.height / 2)).normalized;

        transform.up = rotateToLookAwayFromCenter ? (Vector3)vectorAwayFromCenter : Vector3.up;
    }

    public void ResetCursor(params CustomCursorType[] allowedTypes)
    {
        if (allowedTypes.Contains(beforeType))
            SetCursor(beforeType, before);
    }

    private void SetCursor(CustomCursorType type, Sprite sprite, float size = 1f, Vector2 pivot = default)
    {
        if (this.sprite != sprite)
        {
            //REMOVE ME LATER
            Cursor.visible = false;

            before = this.sprite;
            beforeType = this.currentType;

            this.sprite = sprite;
            rectTransform.pivot = pivot;
            currentType = type;

            rotateToLookAwayFromCenter = currentType == CustomCursorType.ZOOMOUT;

            transform.localScale = Vector3.one * size;
        }
    }

    public void SetCursorForcedState(bool isForced)
    {
        cursorIsFree = !isForced;
    }

    internal void ForceCursorPosition(Vector3 point)
    {
        if (cursorIsFree)
        {
            Debug.LogWarning("Tried forcing the cursor position while it was free. Please use SetCursorForcedState() before and after.");
        }
        else
        {
            transform.position = Game.CameraController.Camera.WorldToScreenPoint(point);
        }
    }



#if UNITY_EDITOR
    [SerializeField] CustomCursorType previewType;
    [SerializeField] bool preview;

    private void OnDrawGizmos()
    {
        if (preview)
        {
            CustomCursorData data = GetCursorDataByType(previewType);
            this.sprite = data.sprite;
            Gizmos.DrawWireSphere(transform.position + (Vector3)data.offset, 10f);
        }
    }

    internal bool IsType(CustomCursorType type)
    {
        return currentType == type;
    }

#endif
}

[System.Serializable]
public class CustomCursorData
{
    public CustomCursorType type;
    public Sprite sprite;
    public Vector2 offset;
}
