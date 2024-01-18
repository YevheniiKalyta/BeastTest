using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorFollowingItem : ItemHolder
{

    public RectTransform parentRectTransform;
    private void Update()
    {
        UpdatePosition();
    }
    private void UpdatePosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, Mouse.current.position.ReadValue(), null, out Vector2 localPoint);
        transform.localPosition = localPoint;
    }
}
