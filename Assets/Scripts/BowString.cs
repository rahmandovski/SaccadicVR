using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BowString : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform start;
    public Transform middle;
    public Transform end;

    private void Update()
    {
        lineRenderer.SetPosition(0, start.position);
        lineRenderer.SetPosition(1, middle.position);
        lineRenderer.SetPosition(2, end.position);
    }
}
