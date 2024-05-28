using FMOD_AudioManagement;
using System.Collections.Generic;
using UnityEngine;
public class PuzzleManager : MonoBehaviour
{
    private PuzzlePiece[] allPieces;
    private bool puzzleCompleted;
    private int spawnOrderTracker = 0;

    private void Awake()
    {
        RandomizePiecePosition();
        EnablePiecesOfSpawnOrder(spawnOrderTracker);
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
        if (puzzleCompleted) return;

        PuzzlePiece[] incorrectPieces = GetIncorrectPuzzlePieces();

        if (incorrectPieces.Length == 0)
        {
            puzzleCompleted = true;
            GameStateManager.instance.PlayerCompletedTask();
        }
        else
        {
            print("Puzzle failed");
            FMODAudioManager.instance.PlayOneShot("Incorrect");

            foreach (PuzzlePiece piece in incorrectPieces)
            {
                piece.HighlightPuzzlePiece();
            }
        }
    }

    public void OnPuzzlePiecePlaced()
    {
        PuzzlePiece[] incorrectPieces = GetIncorrectPuzzlePieces();

        if (incorrectPieces.Length == 0)
        {
            puzzleCompleted = true;
            GameStateManager.instance.PlayerCompletedTask();
        }
        else
        {
            if (AllPiecesAreLocked())
            {
                spawnOrderTracker++;
                EnablePiecesOfSpawnOrder(spawnOrderTracker);
            }
        }
    }

    private PuzzlePiece[] GetIncorrectPuzzlePieces()
    {
        List<PuzzlePiece> toReturn = new List<PuzzlePiece>();

        foreach (PuzzlePiece piece in allPieces)
        {
            if (!piece.IsAtCorrectRelativePosition(out _))
            {
                toReturn.Add(piece);
            }
        }

        return toReturn.ToArray();
    }

    private void RandomizePiecePosition()
    {
        allPieces = FindObjectsOfType<PuzzlePiece>();

        foreach (PuzzlePiece piece in allPieces)
        {
            if (piece.locked) continue;

            Vector2 unitCircle = Random.insideUnitCircle;
            piece.transform.position = transform.position + new Vector3(unitCircle.x, unitCircle.y, 0);
        }
    }

    private void EnablePiecesOfSpawnOrder(int order)
    {
        foreach (PuzzlePiece piece in allPieces)
        {
            if (piece.spawnOrder <= order)
            {
                piece.gameObject.SetActive(true);
            }
            else
            {
                piece.gameObject.SetActive(false);
            }
        }
    }

    private bool AllPiecesAreLocked()
    {
        foreach (PuzzlePiece piece in allPieces)
        {
            if (piece.gameObject.activeInHierarchy && !piece.locked)
            {
                return false;
            }
        }

        return true;
    }
}