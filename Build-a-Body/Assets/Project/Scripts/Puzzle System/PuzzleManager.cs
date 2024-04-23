using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private PuzzlePiece[] allPieces;

    private void Awake()
    {
        allPieces = FindObjectsOfType<PuzzlePiece>();
    }

    [ContextMenu("Save Puzzle Piece Positions")]
    public void SaveAllPiecePositions()
    {
        allPieces = FindObjectsOfType<PuzzlePiece>();
        foreach (PuzzlePiece piece in allPieces)
        {
            piece.SaveDistanceToOtherPieces();
        }
    }

    [ContextMenu("Check Puzzle")]
    public void CheckPuzzleCompletion()
    {
        if (AreAllPiecesCorrect())
        {
            print("Puzzle Finished");
        }
        else
        {
            print("Puzzle failed");
        }
    }

    private bool AreAllPiecesCorrect()
    {
        foreach (PuzzlePiece piece in allPieces)
        {
            if (!piece.IsAtCorrectRelativePosition())
            {
                return false;
            }
        }
        return true;
    }
}