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
            transform.position = new Vector3(worldPoint.x, worldPoint.y, transform.position.z) + new Vector3(offset.x, offset.y, 0);
        }
    }

    protected virtual void OnDraggingStart()
    {
    }

    protected virtual void OnDraggingEnd()
    {
    }

    public virtual void OnInteract(Vector2 screenPosition)
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z)));
        offset = transform.position - worldPoint;
        dragging = true;
        OnDraggingStart();
    }

    public virtual void OnDeinteract()
    {
        dragging = false;
        OnDraggingEnd();
    }

    public virtual int GetLayerInfo()
    {
        return 0;
    }
}