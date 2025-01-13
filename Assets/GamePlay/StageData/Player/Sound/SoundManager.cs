using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

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
        private AudioClip CSharpClip => Player.cSharpClip;
        private AudioMixerGroup ReverbMixerGroup => Player.reverbMixerGroup;
        
        public AudioSources AudioSource { get; set; }
        
        private HashSet<Note> _currentNotes = new();

        public HashSet<Note> RecordedNotes { get; private set; } = new();
        public HashSet<Note> GoalNotes { get; }

        private const float FadeDuration = 0.2f;
        private readonly Dictionary<Note, AudioSource> _activeSources = new();

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
                PlayNote(playNote);
            }
            foreach (var stopNote in stopNotes)
            {
                StopNote(stopNote);
            }
        }

        private void PlayNote(Note note)
        {
            var noteObject = new GameObject($"Note_{note}")
            {
                transform =
                {
                    parent = Player.gameObject.transform
                }
            };

            var audioSource = noteObject.AddComponent<AudioSource>();
            audioSource.clip = CSharpClip;
            audioSource.outputAudioMixerGroup = ReverbMixerGroup;
            audioSource.pitch = Mathf.Pow(2f, note.PitchFromCSharp() / 12f);
            audioSource.loop = true;
            audioSource.Play();
            
            _activeSources[note] = audioSource;
        }
        
        private void StopNote(Note note)
        {
            var source = _activeSources[note];
            _activeSources.Remove(note);
            source.Stop();
            Object.Destroy(source.gameObject);
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

                if (stageElementData.StageElementInstanceBehaviour is not Speaker.Speaker speaker) continue;
                var speakerNotes = speaker.PlayNote;
                if (speakerNotes.Count == 0) continue;
                
                var direction = PlayerData.Coordinates - stageElementData.Coordinates;
                if (direction != stageElementData.Direction)
                {
                    direction = Player.TargetCoordinates - stageElementData.Coordinates;
                    if (direction != stageElementData.Direction) continue;
                }

                if (Player.MovingDirection == direction)
                {
                    foreach (var note in speakerNotes)
                    {
                        notes.Add(note.Lower());
                    }
                } else if (Player.MovingDirection == -direction)
                {
                    foreach (var note in speakerNotes)
                    {
                        notes.Add(note.Higher());
                    }
                }
                else
                {
                    foreach (var note in speakerNotes)
                    {
                        notes.Add(note);
                    }
                }
            }

            return notes;
        }
    }
}