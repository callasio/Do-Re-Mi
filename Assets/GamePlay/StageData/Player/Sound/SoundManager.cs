using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay.StageData.Player.Sound
{
    public enum AudioSources
    {
        Player,
        Record,
        Goal,
    }
    
    public class SoundManager
    {
        private Player Player { get; }
        private StageElement PlayerData => Player.Data;
        private StageElement[] CurrentStageData => PlayerData.CurrentStageElements;
        public AudioSources AudioSource { get; set; }
        
        private HashSet<Note> _currentNotes = new();

        public HashSet<Note> RecordedNotes { get; private set; } = new();
        // public HashSet<Note> GoalNotes { get; private set; } = new();

        public SoundManager(Player player, AudioSources audioSource)
        {
            Player = player;
            AudioSource = audioSource;
        }

        public void Record()
        {
            RecordedNotes = GetPlayerNotes();
        }

        public void Update()
        {
            var newNotes = GetNotes();

            var playNotes = newNotes.Except(_currentNotes).ToList();
            var stopNotes = _currentNotes.Except(newNotes).ToList();
            _currentNotes = newNotes;

            foreach (var playNote in playNotes)
            {
                Debug.Log("Play: " + playNote);
            }
            foreach (var stopNote in stopNotes)
            {
                Debug.Log("Stop: " + stopNote);
            }

            if (playNotes.Count == 0 && stopNotes.Count == 0) return;
            
            var currentNotesString = string.Join(", ", _currentNotes.Select(note => note.ToString()));
            Debug.Log("Current notes: " + currentNotesString);
        }

        private HashSet<Note> GetNotes()
        {
            return AudioSource switch
            {
                AudioSources.Player => GetPlayerNotes(),
                AudioSources.Record => RecordedNotes,
                _ => null
            };
        }
        
        private HashSet<Note> GetPlayerNotes()
        {
            var notes = new HashSet<Note>();
            foreach (var stageElementData in CurrentStageData)
            {
                if (stageElementData.Type != StageElementType.Speaker) continue;
                
                var noteString = stageElementData.Metadata["note"];
                if (noteString == null) continue;
                var note = new Note(noteString);
                
                var direction = PlayerData.Coordinates - stageElementData.Coordinates;
                if (direction != stageElementData.Direction)
                {
                    direction = Player.TargetCoordinates - stageElementData.Coordinates;
                    if (direction != stageElementData.Direction) continue;
                }

                if (Player.MovingDirection == direction)
                {
                    notes.Add(note.Lower());
                } else if (Player.MovingDirection == -direction)
                {
                    notes.Add(note.Higher());
                }
                else
                {
                    notes.Add(note);
                }
            }

            return notes;
        }
    }
}