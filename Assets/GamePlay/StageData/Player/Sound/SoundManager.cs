using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay.StageData.Player.Sound
{
    public class SoundManager
    {
        private Player Player { get; }
        private StageElementData PlayerData => Player.Data;
        private StageElementData[] CurrentStageData => PlayerData.CurrentStageData;
        
        private HashSet<Note> _currentNotes = new();

        public HashSet<Note> RecordedNotes { get; private set; } = new();

        public SoundManager(Player player)
        {
            Player = player;
        }

        public void Record()
        {
            RecordedNotes = _currentNotes;
        }

        public void Update()
        {
            var newNotes = GetNotes();

            var playNotes = newNotes.Except(_currentNotes);
            var stopNotes = _currentNotes.Except(newNotes);
            _currentNotes = newNotes;

            foreach (var playNote in playNotes)
            {
                Debug.Log("Play: " + playNote);
            }
            foreach (var stopNote in stopNotes)
            {
                Debug.Log("Stop: " + stopNote);
            }
        }
        
        private HashSet<Note> GetNotes()
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