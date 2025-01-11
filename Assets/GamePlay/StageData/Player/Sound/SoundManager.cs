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
        
        public HashSet<Note> Notes => _previousNotes;
        
        private HashSet<Note> _previousNotes = new();

        public SoundManager(Player player)
        {
            Player = player;
        }

        public void Update()
        {
            var currentNotes = GetNotes();

            var newNotes = currentNotes.Except(_previousNotes);
            var oldNotes = _previousNotes.Except(currentNotes);
            _previousNotes = currentNotes;

            foreach (var newNote in newNotes)
            {
                Debug.Log("Play: " + newNote);
            }
            foreach (var oldNote in oldNotes)
            {
                Debug.Log("stop: " + oldNote);
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