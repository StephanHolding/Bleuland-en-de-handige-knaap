using System;
using UnityEngine;

public class Draggable : MonoBehaviour
{

    public bool draggable;

    protected bool dragging;
    private Vector3 offset;

    protected void Update()
    {
        if (dragging && draggable)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z)));
            transform.position = new Vector3(worldPoint.x, worldPoint.y, transform.position.z) + offset;
        }
    }

    protected void OnMouseDown()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z)));
        offset = transform.position - worldPoint;
        dragging = true;
        OnDraggingStart();
    }

    protected void OnMouseUp()
    {
        dragging = false;
        OnDraggingEnd();
    }

    protected virtual void OnDraggingStart()
    {

    }

    protected virtual void OnDraggingEnd()
    {

    }

}
