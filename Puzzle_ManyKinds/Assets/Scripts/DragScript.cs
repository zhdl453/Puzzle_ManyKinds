using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour
{
    public delegate void DragEndedDelegate(Transform transform);
    public DragEndedDelegate dragEndedDelegate;
    Camera cam;
    Vector2 pos;
    bool isholding;
    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        if (isholding)
        {
            pos = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = pos;
        }
    }
    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown() is working");
        isholding = true;
    }
    private void OnMouseUp()
    {
        Debug.Log("OnMouseUp() is working");
        isholding = false;
        dragEndedDelegate(this.transform);
    }
}
