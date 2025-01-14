using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay.StageData.Player;
using JetBrains.Annotations;
using Monotone;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

namespace Common.Sound
{
    public class PlayingNote : IEquatable<PlayingNote>
    {
        public readonly Note Note;
        public int PitchDelta;
        private IAudioPlayer Player { get; set; }
        private AudioClip CClip => Player.CClip;
        private AudioMixerGroup ReverbMixerGroup => Player.ReverbMixerGroup;
        private AudioSource _source;

        private const float FadeDuration = 0.5f;
        private const float PitchInterpolationDuration = 0.1f;
        private const float MaxVolume = 0.2f;
        private bool _isPlaying = false;
        
        public PlayingNote(Note note, int pitchDelta, IAudioPlayer player)
        {
            Note = note;
            PitchDelta = pitchDelta;
            _source = null;
            Player = player;
        }

        public Note GetNote() => new (Note.NoteIndex + PitchDelta);
        
        public void PlayNote()
        {
            _isPlaying = true;
            Player.StartCoroutine(PlayNoteWithFade());
        }

        public void StopNote()
        {
            _isPlaying = false;
            Player.StartCoroutine(StopNoteWithFade());
        }

        public void AdjustPitch(int pitchDelta)
        {
            PitchDelta = pitchDelta;
            Player.StartCoroutine(AdjustPitchWithInterpolation());
        }
        
        private IEnumerator PlayNoteWithFade()
        {
            var noteObject = new GameObject($"Note_{Note}")
            {
                transform =
                {
                    parent = Player.GameObject.transform
                }
            };

            var audioSource = _source;
            var startVolume = 0f;

            if (_source is null || _source.IsDestroyed())
            {
                audioSource = noteObject.AddComponent<AudioSource>();
                audioSource.clip = CClip;
                audioSource.outputAudioMixerGroup = ReverbMixerGroup;
                audioSource.pitch = Mathf.Pow(2f, GetNote().PitchFromC() / 12f);
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                startVolume = _source.volume;
                Object.Destroy(_source.gameObject);
            }

            _source = audioSource;
            
            yield return Player.StartCoroutine(FadeVolume(startVolume, MaxVolume));
        }
        
        private IEnumerator StopNoteWithFade()
        {
            if (_source is null || _source.IsDestroyed()) yield break;
            yield return Player.StartCoroutine(FadeVolume(_source.volume * Volume.GetNoteVolume(), 0));
            if (_source is null || _source.IsDestroyed()) yield break;
            if (_isPlaying) yield break;
            
            _source.Stop();
            Object.Destroy(_source.gameObject);
        }

        private IEnumerator FadeVolume(float from, float to)
        {
            var startTime = Time.time;
            while (Time.time - startTime < FadeDuration)
            {
                var t = (Time.time - startTime) / FadeDuration;
                if (_source is null || _source.IsDestroyed()) break;
                _source.volume = Mathf.Lerp(from, to, t) * Volume.GetNoteVolume();
                yield return null;
            }

            if (_source is not null && !_source.IsDestroyed())
                _source.volume = to * Volume.GetNoteVolume();
        }
        
        private IEnumerator AdjustPitchWithInterpolation()
        {
            var from = _source.pitch;
            var to = Mathf.Pow(2f, GetNote().PitchFromC() / 12f);
            yield return InterpolatePitch(from, to);
        }
        
        private IEnumerator InterpolatePitch(float from, float to)
        {
            var startTime = Time.time;
            while (Time.time - startTime < PitchInterpolationDuration)
            {
                var t = (Time.time - startTime) / PitchInterpolationDuration;
                _source.pitch = Mathf.Lerp(from, to, t);
                yield return null;
            }
            _source.pitch = to;
        }

        public bool Equals(PlayingNote other) => Note.Equals(other?.Note) && PitchDelta == other?.PitchDelta;

        public override int GetHashCode() => Note.GetHashCode();

        public override string ToString() => $"{Note}({PitchDelta})";
    }   
    
    public readonly struct Note : IEquatable<Note>
    {
        private static readonly List<string> NoteStrings = new ()
        {
            "A0",
            "A#0",
            "B0",
            "C0",
            "C#0",
            "D0",
            "D#0",
            "E0",
            "F0",
            "F#0",
            "G0",
            "G#0",
            "A1",
            "A#1",
            "B1",
            "C1",
            "C#1",
            "D1",
            "D#1",
            "E1",
            "F1",
            "F#1",
            "G1",
            "G#1",
            "A2",
            "A#2",
            "B2",
            "C2",
            "C#2",
            "D2",
            "D#2",
            "E2",
            "F2",
            "F#2",
            "G2",
            "G#2",
        };
        
        private static readonly int NoteCounts = NoteStrings.Count;

        public static Color GetColor(HashSet<Note> notes) => Color.HSVToRGB(GetHue(notes), 1, 1);

        private static float GetHue(HashSet<Note> notes)
        {
            if (notes.Count == 0)
            {
                return 0;
            }
            return notes.Average(note => note.GetHue());
        }
        private float GetHue() => NoteIndex % 12 / 12f;

        public int NoteIndex { get; }
        
        public Note(int noteIndex) => NoteIndex = noteIndex;

        public Note([NotNull] string noteString)
        {
            var index = NoteStrings.IndexOf(noteString);
            if (index == -1)
            {
                throw new KeyNotFoundException($"Note {noteString} not found.");
            }
            NoteIndex = index;
        }

        public int PitchFromC() => NoteIndex - new Note("C1").NoteIndex;
        
        public override int GetHashCode() => NoteIndex;

        public override bool Equals(object obj) => obj is Note note && NoteIndex == note.NoteIndex;

        public static bool operator ==(Note left, Note right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(Note left, Note right)
        {
            return !(left == right);
        }
        
        public override string ToString() => NoteStrings[(NoteIndex + NoteCounts) % NoteCounts];

        public bool Equals(Note other) => NoteIndex == other.NoteIndex;
    }
}