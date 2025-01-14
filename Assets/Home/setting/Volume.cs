using UnityEngine;
using UnityEngine.UI; // Required for working with UI elements

namespace Home.setting
{
    public class Volume : MonoBehaviour
    {
        private const int Master = 0;
        private const int Background = 1;
        private const int Note = 2;

        public int type; // Define which type of volume this script controls
        private Slider _volumeSlider; // Reference to the UI Slider component

        void Start()
        {
            _volumeSlider = GetComponentInChildren<Slider>();
            // Initialize the slider value based on the volume type
            if (_volumeSlider != null)
            {
                switch (type)
                {
                    case Master:
                        _volumeSlider.value = Monotone.Volume.MasterVolume;
                        break;
                    case Background:
                        _volumeSlider.value = Monotone.Volume.BackgroundVolume;
                        break;
                    case Note:
                        _volumeSlider.value = Monotone.Volume.NoteVolume;
                        break;
                }

                // Add listener for value changes on the slider
                _volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
            }
        }

        // Called when the slider value changes
        private void OnSliderValueChanged(float value)
        {
            switch (type)
            {
                case Master:
                    Monotone.Volume.MasterVolume = value;
                    break;
                case Background:
                    Monotone.Volume.BackgroundVolume = value;
                    break;
                case Note:
                    Monotone.Volume.NoteVolume = value;
                    break;
            }
        }

        // Optional: Clean up listeners when the object is destroyed
        private void OnDestroy()
        {
            if (_volumeSlider != null)
            {
                _volumeSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
            }
        }
    }
}
