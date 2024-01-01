using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HariScript : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public delegate void DragEndedDelegate(Transform transform);
    public DragEndedDelegate dragEndedDelegate;
    public bool holding;

    public void OnDrag(PointerEventData eventData)
    {
        holding = true;
        transform.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        holding = false;
        dragEndedDelegate(this.transform);
    }
}
