using UnityEngine;

public class PuzzlePieceChild : Draggable2D
{

    private PuzzlePiece piece;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        piece = GetComponentInParent<PuzzlePiece>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void OnInteract(Vector2 screenPosition)
    {
        piece.OnInteract(screenPosition);
    }

    public override void OnDeinteract()
    {
        piece.OnDeinteract();
    }

    public override int GetLayerInfo()
    {
        return spriteRenderer.sortingOrder;
    }

}
