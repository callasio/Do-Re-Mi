using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.StageData;
using GamePlay.StageData.Player;

namespace Common.Sound
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

        private AudioSources AudioSource { get; set; }
        
        private List<PlayingNote> _currentNotes = new();
        private AudioSources _currentAudioSource = AudioSources.Player;

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

        private void RecordClickedHandler() => RecordedNotes = new HashSet<Note>(
            GetPlayerNotes().Select(playingNote => playingNote.GetNote()));
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
            var unChangedNotes = _currentNotes
                .Intersect(newNotes)
                .ToList();
            
            if (AudioSource != AudioSources.Player || _currentAudioSource != AudioSources.Player)
            {
                playNotes.ForEach(playingNote => playingNote.PlayNote());
                stopNotes.ForEach(playingNote => playingNote.StopNote());
                _currentNotes = unChangedNotes.Concat(playNotes).ToList();
            }
            else
            {
                var newPlayNotes = playNotes
                    .Where(playingNote => stopNotes.All(stopNote => playingNote.Note != stopNote.Note))
                    .ToList();
                
                var newStopNotes = stopNotes
                    .Where(stopNote => playNotes.All(playingNote => stopNote.Note != playingNote.Note))
                    .ToList();
                
                var pitchChangedNotes = stopNotes
                    .Where(stopNote => playNotes.Any(playingNote =>
                        playingNote.Note == stopNote.Note && playingNote.PitchDelta != stopNote.PitchDelta))
                    .ToList();
                
                newPlayNotes.ForEach(playingNote => playingNote.PlayNote());
                newStopNotes.ForEach(playingNote => playingNote.StopNote());
                pitchChangedNotes.ForEach(stopNote =>
                {
                    var playingNote = playNotes.First(playingNote =>
                        playingNote.Note == stopNote.Note && playingNote.PitchDelta != stopNote.PitchDelta);
                    stopNote.AdjustPitch(playingNote.PitchDelta);
                });
                 
                _currentNotes = newPlayNotes
                    .Concat(pitchChangedNotes)
                    .Concat(unChangedNotes)
                    .ToList();
            }
            
            _currentAudioSource = AudioSource;
        }

        private List<PlayingNote> GetNotes()
        {
            return AudioSource switch
            {
                AudioSources.Player => GetPlayerNotes(),
                AudioSources.Record => new List<PlayingNote>(
                    RecordedNotes.Select(note => new PlayingNote(note, 0, Player))),
                AudioSources.Goal => new List<PlayingNote>(
                    GoalNotes.Select(note => new PlayingNote(note, 0, Player))),
                _ => null
            };
        }
        
        private List<PlayingNote> GetPlayerNotes()
        {
            var notes = new List<PlayingNote>();
            foreach (var stageElementData in CurrentStageData)
            {
                if (stageElementData.Type != StageElementType.Speaker) continue;

                if (stageElementData.StageElementInstanceBehaviour is not GamePlay.StageData.Speaker.Speaker speaker) continue;
                var speakerNotes = speaker.PlayNote;
                if (speakerNotes.Count == 0) continue;
                
                var direction = Player.TargetCoordinates - stageElementData.Coordinates;
                if (direction != stageElementData.Direction)
                {
                    direction = Player.Data.Coordinates - stageElementData.Coordinates;
                    if (direction != stageElementData.Direction) continue;
                }

                if (Player.MovingDirection == direction)
                {
                    notes.AddRange(speakerNotes.Select(note => new PlayingNote(note, -1, Player)));
                } else if (Player.MovingDirection == -direction)
                {
                    notes.AddRange(speakerNotes.Select(note => new PlayingNote(note, 1, Player)));
                }
                else
                {
                    notes.AddRange(speakerNotes.Select(note => new PlayingNote(note, 0, Player)));
                }
            }

            return notes;
        }
    }
}