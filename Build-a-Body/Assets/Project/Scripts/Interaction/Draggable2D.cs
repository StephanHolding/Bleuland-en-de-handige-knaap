using UnityEngine;

public abstract class Draggable2D : Draggable
{
    public override void OnDeinteract()
    {
        dragging = false;
        OnDraggingEnd();
    }

    public override void OnInteract(Vector2 screenPosition)
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z)));
        offset = transform.position - worldPoint;
        dragging = true;
        OnDraggingStart();
    }

    public override void Dragging()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z)));
        transform.position = new Vector3(worldPoint.x, worldPoint.y, transform.position.z) + new Vector3(offset.x, offset.y, 0);
    }
}
