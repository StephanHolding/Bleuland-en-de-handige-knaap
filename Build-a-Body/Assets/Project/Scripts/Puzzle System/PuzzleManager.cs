using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private PuzzlePiece[] allPieces;

    private void Awake()
    {
        allPieces = FindObjectsOfType<PuzzlePiece>();

        foreach (PuzzlePiece piece in allPieces)
        {
            Vector2 unitCircle = Random.insideUnitCircle;
            piece.transform.position = transform.position + new Vector3(unitCircle.x, unitCircle.y, 0);
        }
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
            GameStateManager.instance.PlayerCompletedTask();
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
            if (!piece.IsAtCorrectRelativePosition(out _))
            {
                return false;
            }
        }
        return true;
    }
}