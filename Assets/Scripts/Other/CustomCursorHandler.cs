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
}

public class CustomCursorHandler : Image
{
    Sprite before;
    CustomCursorType beforeType;

    protected override void Start()
    {
        base.Start();
        Cursor.visible = false;
    }

    CustomCursorType currentType;
    [SerializeField] Sprite defaultCursor, dragableCursor, dragggingCursor;

    public void SetCursorType(CustomCursorType customCursorType, Sprite sprite = default(Sprite), float size = 1f)
    {
        if (currentType != customCursorType)
        {
            Sprite cursorSprite = customCursorType == CustomCursorType.MANUAL ? sprite : GetCursorTextureByType(customCursorType);
            SetCursor(customCursorType, cursorSprite, size);
        }
    }

    private Sprite GetCursorTextureByType(CustomCursorType customCursorType)
    {
        switch (customCursorType)
        {
            case CustomCursorType.DRAGABLE:
                return dragableCursor;
                break;

            case CustomCursorType.DRAGGING:
                return dragggingCursor;
                break;
        }

        return defaultCursor;
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void ResetCursor(params CustomCursorType[] allowedTypes)
    {
        if (allowedTypes.Contains(beforeType))
            SetCursor(beforeType ,before);
    }

    private void SetCursor(CustomCursorType type, Sprite sprite, float size = 1f)
    {
        if (this.sprite != sprite)
        {
            before = this.sprite;
            beforeType = this.currentType;

            this.sprite = sprite;
            currentType = type;

            transform.localScale = Vector3.one * size;
        }
    }
}
