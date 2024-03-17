using UnityEngine;

public class Draggable : MonoBehaviour
{
    public bool draggable;

    protected bool dragging;

    protected void Update()
    {
        if (dragging && draggable)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z)));
            transform.position = new Vector3(worldPoint.x, worldPoint.y, transform.position.z);
        }
    }

    protected void OnMouseDown()
    {
        dragging = true;
    }

    protected void OnMouseUp()
    {
        dragging = false;
    }

}
