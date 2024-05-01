using UnityEngine;

interface ITouchable
{
    public void OnInteract(Vector2 screenPosition);
    public void OnDeinteract();
    public int GetLayerNumber();
}