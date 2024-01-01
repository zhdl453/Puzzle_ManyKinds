using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour
{
    public delegate void DragEndedDelegate(Transform transform);
    public DragEndedDelegate dragEndedDelegate;
    Vector3 pos;
    bool holding;
    void Update()
    {
        if (holding)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;
            transform.position = pos;
        }
    }
    private void OnMouseDown()
    {
        holding = true;
    }
    private void OnMouseUp()
    {
        holding = false;
        dragEndedDelegate(this.transform);
    }
}
