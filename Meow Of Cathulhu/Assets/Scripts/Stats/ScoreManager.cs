using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private MusicalNoteController musicalNoteController;

    private int score;
    private bool canScore;

    private void OnEnable()
    {
        musicalNoteController.onNoteChange += OnNoteChange;
    }

    private void OnDisable()
    {
        musicalNoteController.onNoteChange -= OnNoteChange;
    }

    private void OnNoteChange()
    {
        canScore = true;
    }

    public int AddScore(Direction direction)
    {
        if (!canScore) return -1;

        if (musicalNoteController.IsTheActualDirection(direction))
        {
            score++;
        }
        else
        {
            score = 0;
        }

        canScore = false;
        return score;
    }
}
