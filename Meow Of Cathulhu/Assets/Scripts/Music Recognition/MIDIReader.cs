using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NAudio.Midi;
using UnityEngine.AI;

public struct MIDI
{
    public string MidiName;
    public float NoteVelocity;
}

public class MIDIReader : MonoBehaviour
{
    [SerializeField] private int _mainTrackIndex;

    private MidiFile _midiFile;
    private List<double> _noteTimesInSeconds = new List<double>();

    private List<Vector2> _notePositions = new List<Vector2>();

    private long _lastTempoPulses = 0;
    private double _lastTempoSecond = 0.0;
    private double _lastTempoValue = 120;
    
    public List<Vector2> GetNotesPositionMap(MIDI midi)
    {
        _midiFile = GetMIDIFile(midi);
        
        for (var n = 0; n < _midiFile.Tracks; n++)
        {
            foreach (var midiEvent in _midiFile.Events[n])
            {
                if (MidiEvent.IsNoteOff(midiEvent)) continue;
                if (!(midiEvent is TempoEvent)) continue;
                
                var tempoEvent = (TempoEvent)midiEvent;
                _lastTempoValue = tempoEvent.Tempo;
                _lastTempoPulses = tempoEvent.AbsoluteTime;
            }
        }
        
        foreach (var midiEvent in _midiFile.Events[_mainTrackIndex])
        {
            if (MidiEvent.IsNoteOff(midiEvent)) continue;
            var mNewSecond = GetSeconds(_midiFile.DeltaTicksPerQuarterNote, _lastTempoValue, midiEvent.AbsoluteTime - _lastTempoPulses, 
                _lastTempoSecond);

            _noteTimesInSeconds.Add(mNewSecond);
        }

        SetNotePositionList(midi);

        return _notePositions;
    }

    private MidiFile GetMIDIFile(MIDI midi)
    {
        return new MidiFile(Application.streamingAssetsPath + "/MIDI Files/" + midi.MidiName + ".mid", true);
    }
    
    private void SetNotePositionList(MIDI midi)
    {
        foreach (var spawnPosition in _noteTimesInSeconds.Select(time => new Vector3((float) (time * midi.NoteVelocity), transform.position.y, 0)))
        {
            _notePositions.Add(spawnPosition);
        }
    }
    
    private static double GetSeconds(int division, double tempo, long pulse, double sec = 0.0)
    {
        return ((60.0 / tempo) * ((double)(pulse) / division)) + sec;
    }
}
