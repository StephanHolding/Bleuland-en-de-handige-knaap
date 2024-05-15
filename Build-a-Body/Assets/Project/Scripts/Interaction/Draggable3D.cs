using UnityEngine;

public abstract class Draggable3D : Draggable
{
    public override void OnInteract(Vector2 screenPosition)
    {
        Vector3 worldPoint = CreatePointOnPlane();
        offset = transform.position - worldPoint;
        dragging = true;
        OnDraggingStart();
    }

    public override void OnDeinteract()
    {
        dragging = false;
        OnDraggingEnd();
    }

    public override void Dragging()
    {
        transform.position = CreatePointOnPlane() + offset;
    }

    private Vector3 CreatePointOnPlane()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(TouchscreenInteraction.GetScreenInputPosition());
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 raypoint = ray.GetPoint(enter);
            return raypoint;
        }

        return Vector3.zero;
    }
}
