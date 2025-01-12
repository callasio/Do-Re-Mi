using System;
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
        public static event Action OnRecordClicked;
        public static event Action OnRecordHovered;
        public static event Action OnRecordHoverEnded;
        public static event Action OnFlagHovered;
        public static event Action OnFlagHoverEnded;
        public static void RecordClicked() => OnRecordClicked?.Invoke();
        public static void RecordHovered() => OnRecordHovered?.Invoke();
        public static void RecordHoverEnded() => OnRecordHoverEnded?.Invoke();
        public static void FlagHovered() => OnFlagHovered?.Invoke();
        public static void FlagHoverEnded() => OnFlagHoverEnded?.Invoke();
        
        
        private Player Player { get; }
        private StageElement PlayerData => Player.Data;
        private StageElement[] CurrentStageData => PlayerData.CurrentStageElements;
        public AudioSources AudioSource { get; set; }
        
        private HashSet<Note> _currentNotes = new();

        public HashSet<Note> RecordedNotes { get; private set; } = new();
        public HashSet<Note> GoalNotes { get; }

        public SoundManager(Player player, AudioSources audioSource)
        {
            Player = player;
            AudioSource = audioSource;
            GoalNotes = player.Data.CurrentStageData.Configuration.Goal;
            
            OnRecordClicked += RecordClickedHandler;
            OnRecordHovered += RecordHoveredHandler;
            OnRecordHoverEnded += RecordHoverEndedHandler;
            OnFlagHovered += FlagHoveredHandler;
            OnFlagHoverEnded += FlagHoverEndedHandler;
        }

        public void RecordClickedHandler() => RecordedNotes = GetPlayerNotes();
        private void RecordHoveredHandler() => AudioSource = AudioSources.Record;
        private void RecordHoverEndedHandler() => AudioSource = AudioSources.Player;
        private void FlagHoveredHandler() => AudioSource = AudioSources.Goal;
        private void FlagHoverEndedHandler() => AudioSource = AudioSources.Player;

        public void OnDestroy()
        {
            OnRecordClicked -= RecordClickedHandler;
            OnRecordHovered -= RecordHoveredHandler;
            OnRecordHoverEnded -= RecordHoverEndedHandler;
            OnFlagHovered -= FlagHoveredHandler;
            OnFlagHoverEnded -= FlagHoverEndedHandler;
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
                AudioSources.Goal => GoalNotes,
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