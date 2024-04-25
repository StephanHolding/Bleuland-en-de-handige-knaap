using UnityEngine;

public class Draggable : MonoBehaviour
{
    public bool draggable;

    protected bool dragging;

    protected Vector3 lastCursorPosition = Vector3.zero; // only for mouse button

    protected void Update()
    {
        if (draggable)
        {
            if (Input.touchSupported && !Application.isEditor)
            {
                if (Input.touchCount > 0)
                {
                    if (!dragging)
                    {
                        OnDraggingStart();
                        dragging = true;
                    }

                    if (Input.touchCount == 1)
                    {
                        Touch touch = Input.GetTouch(0);
                        Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Mathf.Abs(Camera.main.transform.position.z)));
                        transform.position = new Vector3(screenToWorld.x, screenToWorld.y, transform.position.z);
                    }
                    else //touchcount is larger than 1
                    {
                        Touch touch = Input.GetTouch(0);
                        transform.rotation *= Quaternion.Euler(0, touch.deltaPosition.x, 0);
                    }
                }
                else //touchcount is 0 or negative
                {
                    if (dragging)
                    {
                        OnDraggingEnd();
                        dragging = false;
                    }
                }
            }
            else //DEBUG IN EDITOR ONLY
            {
                if (Input.GetMouseButton(0)) // left
                {
                    if (!dragging)
                    {
                        OnDraggingStart();
                        dragging = true;
                    }

                    Vector3 screenToWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z)));
                    transform.position = new Vector3(screenToWorld.x, screenToWorld.y, transform.position.z);
                }
                else if (Input.GetMouseButton(1)) // right
                {
                    if (!dragging)
                    {
                        OnDraggingStart();
                        dragging = true;
                    }

                    transform.rotation *= Quaternion.Euler(0, lastCursorPosition.x - Input.mousePosition.x, 0);
                }
                else
                {
                    if (dragging)
                    {
                        OnDraggingEnd();
                        dragging = false;
                    }
                }

                lastCursorPosition = Input.mousePosition;
            }
        }
    }

    protected virtual void OnDraggingStart()
    {

    }

    protected virtual void OnDraggingEnd()
    {

    }

}
