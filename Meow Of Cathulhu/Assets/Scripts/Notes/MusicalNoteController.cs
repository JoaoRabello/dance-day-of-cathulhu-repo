using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class MusicalNoteController : MonoBehaviour
{
    private List<Direction> directions = new List<Direction>();
    [SerializeField] private List<NoteRenderer> notes = new List<NoteRenderer>();
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform destroyPosition;
    [SerializeField] private MIDIReader midiReader;

    [SerializeField] private string actualSongName;
    [SerializeField] private float actualSongNoteSpeed;
    
    private Direction actualDirection;
    private NoteRenderer actualNote;
    private int actualDirectionIndex = 0;
    private int directionIndex = 0;
    private bool canSpawn = true;
    public delegate void OnNoteChangeDelegate();
    public event OnNoteChangeDelegate onNoteChange;

    public TextMeshProUGUI text;

    void Start()
    {
        var NotePositionsMap = midiReader.GetNotesPositionMap(GenerateNewMidiByActualSong());
        SetDirectionList(NotePositionsMap.Count);
        SetNoteList(NotePositionsMap);
    }

    private MIDI GenerateNewMidiByActualSong()
    {
        return new MIDI
        {
            MidiName = actualSongName,
            NoteVelocity = actualSongNoteSpeed
        };
    }
    
    private void SetDirectionList(int directionAmount)
    {
        int randomIndex = 0;
        Direction tempDirection = Direction.LEFT;

        for (int i = 0; i < directionAmount; i++)
        {
            randomIndex = Random.Range(0, 4);

            switch (randomIndex)
            {
                case 0:
                    tempDirection = Direction.LEFT;
                    break;
                case 1:
                    tempDirection = Direction.RIGHT;
                    break;
                case 2:
                    tempDirection = Direction.UP;
                    break;
                case 3:
                    tempDirection = Direction.DOWN;
                    break;
            }

            directions.Add(tempDirection);
        }

        actualDirection = directions[actualDirectionIndex];
    }

    private void SetNoteList(List<Vector2> notePositionsMap)
    {
        Direction tempDirection;

        for (int i = 0; i < notePositionsMap.Count; i++)
        {
            tempDirection = directions[i];
            SpawnNote(tempDirection, notePositionsMap[i]);
        }
    }

    void Update()
    {
        if (GameStateManager.Instance.ActualGameState == GameState.PAUSE ||
            GameStateManager.Instance.ActualGameState == GameState.DEFEAT ||
            GameStateManager.Instance.ActualGameState == GameState.VICTORY) return;
        
        if (actualDirectionIndex >= directions.Count)
        {
            text.SetText("Acabou!");
            return;
        }

        SetActualNote();
    }

    private void SetActualNote()
    {
        float xOffset = 0;
        switch (GameStateManager.Instance.ActualGameState)
        {
            case GameState.NORMAL:
                xOffset = 0.6f;
                break;
            case GameState.REWIND:
                xOffset = -0.6f;
                break;
        }

        CheckActualNote(xOffset);

        ShowActualNote();
    }

    private void CheckActualNote(float xOffset)
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + xOffset, transform.position.y), Vector2.up);

        if (!hit.collider) return;

        if (!IsTheSameNote(hit.collider.gameObject.GetComponent<NoteRenderer>()))
        {
            onNoteChange?.Invoke();
        }

        if (actualNote) actualNote.IsActualNote = false;

        actualNote = hit.collider.gameObject.GetComponent<NoteRenderer>();
        actualNote.IsActualNote = true;
        actualDirection = actualNote.NoteDirection;
    }

    private bool IsTheSameNote(NoteRenderer note)
    {
        return note.Equals(actualNote);
    }

    private void ShowActualNote()
    {
        text.SetText(actualDirection.ToString());
    }

    private void SpawnNote(Direction direction, Vector2 position)
    {
        NoteRenderer note = null;
        switch (direction)
        {
            case Direction.LEFT:
                note = Instantiate(notes[0], position, Quaternion.identity);
                break;
            case Direction.RIGHT:
                note = Instantiate(notes[1], position, Quaternion.identity);
                break;
            case Direction.UP:
                note = Instantiate(notes[2], position, Quaternion.identity);
                break;
            case Direction.DOWN:
                note = Instantiate(notes[3], position, Quaternion.identity);
                break;
        }

        if (!(note is null))
        {
            note.NoteDirection = direction;
            note.SetWaypoints(spawnPosition, destroyPosition);
        }
    }

    public void SpawnNextNote()
    {
        if (GameStateManager.Instance.ActualGameState != GameState.NORMAL) return;

        if (!canSpawn) return;
        
        Direction tempDirection = Direction.LEFT;
        var randomIndex = Random.Range(0, 4);

        switch (randomIndex)
        {
            case 0:
                tempDirection = Direction.LEFT;
                break;
            case 1:
                tempDirection = Direction.RIGHT;
                break;
            case 2:
                tempDirection = Direction.UP;
                break;
            case 3:
                tempDirection = Direction.DOWN;
                break;
        }
        SpawnNote(tempDirection, spawnPosition.position);
        directionIndex++;
        StartCoroutine(SpawnCooldownCount());
    }
    
    public bool IsTheActualDirection(Direction direction)
    {
        return direction == actualDirection;
    }

    private IEnumerator SpawnCooldownCount()
    {
        canSpawn = false;
        yield return new WaitForSeconds(0.6f);
        canSpawn = true;
    }
}
