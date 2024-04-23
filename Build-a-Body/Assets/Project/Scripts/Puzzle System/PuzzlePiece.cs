using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : Draggable
{
    [System.Serializable]
    public class PieceDistance
    {
        public PieceDistance(string pieceName, float distance)
        {
            this.pieceName = pieceName;
            this.distance = distance;
        }
        
        public string pieceName;
        public float distance;
    }

    [SerializeField]
    private PieceDistance[] allPieceDistances;

    private const float DISTANCE_THRESHOLD = 0.5f;
    
    public bool IsAtCorrectRelativePosition()
    {
        PuzzlePiece[] allPuzzlePieces = GameObject.FindObjectsOfType<PuzzlePiece>();

        foreach (PuzzlePiece piece in allPuzzlePieces)
        {
            if (piece != this)
            {
                PieceDistance savedDistance = GetSavedDistance(piece.gameObject.name);
                
                float distanceToOtherPiece = Vector3.Distance(transform.position, piece.transform.position);
                if (Mathf.Abs(distanceToOtherPiece - savedDistance.distance) > DISTANCE_THRESHOLD)
                {
                    return false;
                }
            }
        }

        return true;
    }
    
    public void SaveDistanceToOtherPieces()
    {
        List<PieceDistance> toReturn = new List<PieceDistance>();
        PuzzlePiece[] allPuzzlePieces = GameObject.FindObjectsOfType<PuzzlePiece>();
        foreach (PuzzlePiece piece in allPuzzlePieces)
        {
            if (piece != this)
            {
                toReturn.Add(new PieceDistance(piece.gameObject.name, Vector3.Distance(transform.position, piece.transform.position)));
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
}