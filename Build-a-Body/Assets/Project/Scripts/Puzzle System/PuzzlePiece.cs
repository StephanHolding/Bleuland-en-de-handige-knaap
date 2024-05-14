using FMOD_AudioManagement;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : Draggable2D
{
    [System.Serializable]
    public class PieceDistance
    {
        public PieceDistance(string pieceName, Vector3 offsetToPiece)
        {
            this.pieceName = pieceName;

            this.offsetToPiece = offsetToPiece;
        }

        public string pieceName;
        public float distanceToPiece;
        public Vector3 offsetToPiece;
    }

    [SerializeField]
    private PieceDistance[] allPieceDistances;

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
                PieceDistance savedDistance = GetSavedDistance(piece.gameObject.name);

                if (Vector3.Distance(transform.position, piece.transform.position + savedDistance.offsetToPiece) <= snapThreshold)
                {
                    snapToWorldPosition = piece.transform.position + savedDistance.offsetToPiece;
                    return true;
                }
            }
        }

        snapToWorldPosition = Vector3.zero;
        return false;
    }

    public void SaveDistanceToOtherPieces()
    {
        List<PieceDistance> toReturn = new List<PieceDistance>();
        PuzzlePiece[] allPuzzlePieces = GameObject.FindObjectsOfType<PuzzlePiece>();
        foreach (PuzzlePiece piece in allPuzzlePieces)
        {
            if (piece != this)
            {
                toReturn.Add(new PieceDistance(piece.gameObject.name, transform.position - piece.transform.position));
            }
        }

        allPieceDistances = toReturn.ToArray();
    }

    private PieceDistance GetSavedDistance(string pieceName)
    {
        foreach (PieceDistance pieceDistance in allPieceDistances)
        {
            if (pieceDistance.pieceName == pieceName)
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
            return 0;
    }
}