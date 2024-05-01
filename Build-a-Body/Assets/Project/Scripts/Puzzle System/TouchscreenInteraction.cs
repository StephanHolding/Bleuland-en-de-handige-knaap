using System.Collections.Generic;
using UnityEngine;

public class TouchscreenInteraction : MonoBehaviour
{

    private Camera mainCam;
    private Draggable currentlyInteracting;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot2DRaycast();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentlyInteracting != null)
            {
                currentlyInteracting.OnDeinteract();
                currentlyInteracting = null;
            }
        }
    }

    private void Shoot2DRaycast()
    {
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(mainCam.ScreenPointToRay(Input.mousePosition));
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
        currentlyInteracting.OnInteract(Input.mousePosition);
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

}
