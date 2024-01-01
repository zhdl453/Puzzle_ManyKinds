using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HariManager : MonoBehaviour
{
    public List<Transform> snapPoints;
    public List<HariScript> hariScripts;
    public float snapRange;
    void Start()
    {
        foreach (HariScript script in hariScripts)
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
                Debug.Log(point.position);
                Debug.Log(obj.position);
                obj.position = point.position;
                return;
            }
        }
    }
}
