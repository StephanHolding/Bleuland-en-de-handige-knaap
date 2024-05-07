using UnityEngine;

public abstract class Draggable : MonoBehaviour
{
    public bool draggable = true;

    protected bool dragging;
    protected Vector3 offset;

    protected void Update()
    {
        if (dragging && draggable)
        {
            Dragging();
        }
    }

    protected virtual void OnDraggingStart()
    {
    }

    protected virtual void OnDraggingEnd()
    {
    }

    public abstract void OnInteract(Vector2 screenPosition);
    public abstract void OnDeinteract();
    public abstract void Dragging();
    public abstract int GetLayerInfo();
}