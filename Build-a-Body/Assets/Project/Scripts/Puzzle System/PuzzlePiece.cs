using FMOD_AudioManagement;
using UnityEngine;

public class PuzzlePiece : Draggable2D
{
    [System.Serializable]
    public class PuzzlePieceReference
    {
        public Draggable2D otherPiece;
        public Vector3 offsetToPiece;
    }

    [SerializeField]
    private PuzzlePieceReference[] allPieceDistances;

    public float snapThreshold = 0.25f;

    private SpriteRenderer spriteRenderer;
    private Collider2D collider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    protected override void OnDraggingEnd()
    {

        if (IsAtCorrectRelativePosition(out Vector3 snapToWorldPosition))
        {
            Snap(snapToWorldPosition);
        }
    }

    public bool IsAtCorrectRelativePosition(out Vector3 snapToWorldPosition)
    {
        PuzzlePiece[] allPuzzlePieces = GameObject.FindObjectsOfType<PuzzlePiece>();

        foreach (PuzzlePiece piece in allPuzzlePieces)
        {
            if (piece != this)
            {
                PuzzlePieceReference savedDistance = GetSavedDistance(piece.gameObject.name);
                if (savedDistance != null)
                {
                    if (Vector3.Distance(transform.position, piece.transform.position + savedDistance.offsetToPiece) <= snapThreshold)
                    {
                        snapToWorldPosition = piece.transform.position + savedDistance.offsetToPiece;
                        return true;
                    }
                }
            }
        }

        snapToWorldPosition = Vector3.zero;
        return false;
    }

    public void SaveDistanceToOtherPieces()
    {
        foreach (PuzzlePieceReference otherPieces in allPieceDistances)
        {
            otherPieces.offsetToPiece = transform.position - otherPieces.otherPiece.transform.position;
        }
    }

    private PuzzlePieceReference GetSavedDistance(string pieceName)
    {
        foreach (PuzzlePieceReference pieceDistance in allPieceDistances)
        {
            if (pieceDistance.otherPiece.gameObject.name == pieceName)
            {
                return pieceDistance;
            }
        }

        return null;
    }

    private void Snap(Vector3 targetPosition)
    {
        transform.position = targetPosition;

        FMODAudioManager.instance.PlayOneShot("Puzzle snap");
        ParticleEffectHandler.instance.PlayParticle("Puzzle Snap Particle", collider.bounds.center, Quaternion.identity);
    }

    public override int GetLayerInfo()
    {
        if (spriteRenderer != null)
            return spriteRenderer.sortingOrder;
        else
            return 99;
    }
}