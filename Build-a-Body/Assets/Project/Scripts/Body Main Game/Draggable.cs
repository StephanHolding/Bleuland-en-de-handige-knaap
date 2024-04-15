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
            if(Input.touchSupported && !Application.isEditor)
            {
                if (Input.touchCount>1) // rotate
                {
                    dragging = true;
                    Touch touch = Input.GetTouch(0);
                    transform.rotation *= Quaternion.Euler(0, touch.deltaPosition.x, 0);
                }
                else if (Input.touchCount == 1) // locate
                {
                    dragging = true;
                    Touch touch = Input.GetTouch(0);
                    transform.position = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Mathf.Abs(Camera.main.transform.position.z)));
                }
                else
                    dragging = false;
            }
            else
            {
                if (Input.GetMouseButton(0)) // left
                {
                    dragging = true;
                    transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z)));
                }
                else if (Input.GetMouseButton(1)) // right
                {
                    dragging = true;
                    transform.rotation *= Quaternion.Euler(0, lastCursorPosition.x - Input.mousePosition.x, 0);
                }
                else
                    dragging = false;
                lastCursorPosition = Input.mousePosition;
            }
        }
    }

}
