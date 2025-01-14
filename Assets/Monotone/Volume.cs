using UnityEngine;

namespace Monotone
{
    public static class Volume
    {
        private static float _masterVolume;
        private static float _backgroundVolume;
        private static float _noteVolume;

        // Property keys for PlayerPrefs
        private const string MasterVolumeKey = "MasterVolume";
        private const string BackgroundVolumeKey = "BackgroundVolume";
        private const string NoteVolumeKey = "NoteVolume";

        static Volume()
        {
            // Load preferences on initialization
            _masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1);
            _backgroundVolume = PlayerPrefs.GetFloat(BackgroundVolumeKey, 1);
            _noteVolume = PlayerPrefs.GetFloat(NoteVolumeKey, 1);
        }

        public static float MasterVolume
        {
            get => _masterVolume;
            set
            {
                _masterVolume = value;
                PlayerPrefs.SetFloat(MasterVolumeKey, _masterVolume);
                PlayerPrefs.Save();
            }
        }

        public static float BackgroundVolume
        {
            get => _backgroundVolume;
            set
            {
                _backgroundVolume = value;
                PlayerPrefs.SetFloat(BackgroundVolumeKey, _backgroundVolume);
                PlayerPrefs.Save();
            }
        }

        public static float NoteVolume
        {
            get => _noteVolume;
            set
            {
                _noteVolume = value;
                PlayerPrefs.SetFloat(NoteVolumeKey, _noteVolume);
                PlayerPrefs.Save();
            }
        }

        public static float GetBackgroundVolume() => MasterVolume * BackgroundVolume;
        public static float GetNoteVolume() => MasterVolume * NoteVolume;
    }
}