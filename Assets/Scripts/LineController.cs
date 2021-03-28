using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public LineRenderer lr;
    public Transform[] points;
    void Start()
    {
        lr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(lr.enabled)
        {
            for (int i = 0; i < points.Length; i++)
            {
                lr.SetPosition(i, points[i].position);
            }
        }
    }

    public void SetupLine(Transform[] points)
    {
        lr.positionCount = points.Length;
        this.points = points;
    }
}
