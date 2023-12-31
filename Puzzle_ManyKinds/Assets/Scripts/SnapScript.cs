using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class SnapScript : MonoBehaviour
{
    public List<Transform> snapPoints;
    public List<DragScript> dragScripts;
    public float snapRange = 0.5f;
    void Start()
    {
        foreach (DragScript script in dragScripts)
        {
            script.dragEndedDelegate = SnapObject;
        }
    }

    public void SnapObject(Transform obj)
    {
        foreach (Transform point in snapPoints)
        {
            if (Vector2.Distance(point.position, obj.position) <= snapRange)
            {
                obj.position = point.position;
                return;
            }
        }
    }
}
