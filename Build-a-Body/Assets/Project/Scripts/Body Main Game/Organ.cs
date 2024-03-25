using UnityEngine;

public class Organ : Draggable
{

    public Vector3 targetPosition;

    public string organName;

    [Header("PLACEHOLDER")]
    public Sprite organRepresentation;

    private const float THRESHOLD = 1f;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (string.IsNullOrEmpty(organName))
        {
            organName = gameObject.name;
        }

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = organRepresentation;
    }


    [ContextMenu("Save Position")]
    public void SaveLocation()
    {
        targetPosition = transform.localPosition;
    }

    public bool IsInCorrectPosition()
    {
        return Vector2.Distance(transform.position, targetPosition) <= THRESHOLD;
    }
}
