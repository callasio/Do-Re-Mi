using System.Collections.Generic;
using JetBrains.Annotations;

namespace GamePlay.StageData.Player.Sound
{
    public readonly struct Note
    {
        private static readonly List<string> NoteStrings = new ()
        {
            "C",
            "C#",
            "D",
            "D#",
            "E",
            "F",
            "F#",
            "G",
            "G#",
            "A",
            "A#",
            "B"
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
            return new Note(NoteIndex + 1);
        }

        public Note Lower()
        {
            return new Note(NoteIndex - 1);
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