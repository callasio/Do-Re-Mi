using System.Collections.Generic;
using JetBrains.Annotations;

namespace GamePlay.StageData.Player.Sound
{
    public readonly struct Note
    {
        private static readonly List<string> NoteStrings = new ()
        {
            "C0",
            "C#0",
            "D0",
            "D#0",
            "E0",
            "F0",
            "F#0",
            "G0",
            "G#0",
            "A0",
            "A#0",
            "B0",
            "C1",
            "C#1",
            "D1",
            "D#1",
            "E1",
            "F1",
            "F#1",
            "G1",
            "G#1",
            "A1",
            "A#1",
            "B1",
            "C2",
            "C#2",
            "D2",
            "D#2",
            "E2",
            "F2",
            "F#2",
            "G2",
            "G#2",
            "A2",
            "A#2",
            "B2",
        };
        
        private static readonly int NoteCounts = NoteStrings.Count;
        
        public int NoteIndex { get; }
        
        private Note(int noteIndex)
        {
            NoteIndex = noteIndex;
        }
        
        public Note([NotNull] string noteString)
        {
            var index = NoteStrings.IndexOf(noteString);
            if (index == -1)
            {
                throw new KeyNotFoundException($"Note {noteString} not found.");
            }
            NoteIndex = index;
        }

        public Note Higher()
        {
            return new Note((NoteIndex + 1) % NoteCounts);
        }

        public Note Lower()
        {
            return new Note((NoteIndex + NoteCounts - 1) % NoteCounts);
        }
        
        public override int GetHashCode()
        {
            return NoteIndex;
        }
        
        public override bool Equals(object obj)
        {
            return obj is Note note && NoteIndex == note.NoteIndex;
        }
        
        public static bool operator ==(Note left, Note right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(Note left, Note right)
        {
            return !(left == right);
        }
        
        public override string ToString()
        {
            return NoteStrings[(NoteIndex + NoteCounts) % NoteCounts];
        }
    }
}