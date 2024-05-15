using System.Collections.Generic;
using UnityEngine;

public class TouchscreenInteraction : MonoBehaviour
{

    private Camera mainCam;
    private Draggable currentlyInteracting;

    private bool wasTouching;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        if (Input.touchCount > 0)
        {
            if (currentlyInteracting == null)
            {
                Shoot2DRaycast();
                Shoot3DRaycast();
            }

            wasTouching = true;
        }

        if (Input.touchCount == 0 && wasTouching)
        {
            if (currentlyInteracting != null)
            {
                currentlyInteracting.OnDeinteract();
                currentlyInteracting = null;
            }

            wasTouching = false;
        }

#else

        if (Input.GetMouseButtonDown(0))
        {
            if (currentlyInteracting == null)
            {
                Shoot2DRaycast();
                Shoot3DRaycast();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentlyInteracting != null)
            {
                currentlyInteracting.OnDeinteract();
                currentlyInteracting = null;
            }
        }
#endif
    }

    private void Shoot2DRaycast()
    {
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(mainCam.ScreenPointToRay(GetScreenInputPosition()));

        if (hits.Length > 0)
        {
            List<Draggable> draggables = new List<Draggable>();
            foreach (RaycastHit2D hit in hits)
            {
                Draggable draggable = hit.transform.GetComponent<Draggable>();
                if (draggable != null)
                {
                    draggables.Add(draggable);
                }
            }

            Draggable highestDraggable = GetHighestLayerNumber(draggables);
            currentlyInteracting = highestDraggable;
            currentlyInteracting.OnInteract(GetScreenInputPosition());
        }
    }


    private void Shoot3DRaycast()
    {
        if (Physics.Raycast(mainCam.ScreenPointToRay(GetScreenInputPosition()), out RaycastHit hit))
        {
            Draggable draggable = hit.transform.GetComponent<Draggable>();
            if (draggable != null)
            {
                currentlyInteracting = draggable;
                currentlyInteracting.OnInteract(GetScreenInputPosition());
            }
        }
    }
    private Draggable GetHighestLayerNumber(List<Draggable> draggables)
    {
        int currentHighestNumber = -1;
        int listIndex = 0;

        for (int i = 0; i < draggables.Count; i++)
        {
            int layerNumber = draggables[i].GetLayerInfo();

            if (layerNumber > currentHighestNumber)
            {
                currentHighestNumber = layerNumber;
                listIndex = i;
            }
        }

        return draggables[listIndex];
    }

    public static Vector3 GetScreenInputPosition()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return Input.GetTouch(0).position;
            case RuntimePlatform.WindowsPlayer:
                return Input.mousePosition;
            case RuntimePlatform.WindowsEditor:
                return Input.mousePosition;
            default:
                return Input.GetTouch(0).position;
        }
    }

}
