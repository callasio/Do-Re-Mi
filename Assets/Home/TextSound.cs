using System.Collections.Generic;
using Common.Sound;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

namespace Home
{
    public class TextSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IAudioPlayer
    {
        public GameObject GameObject => gameObject;
        public AudioClip cClip;
        public AudioMixerGroup reverbMixerGroup;
        public AudioClip CClip => cClip;
        public AudioMixerGroup ReverbMixerGroup => reverbMixerGroup;
        private List<PlayingNote> _playingNote;

        public void Start()
        {
            _playingNote = new List<PlayingNote> {
                new PlayingNote(new Note("C#1"), 0, this),
                new PlayingNote(new Note("F1"), 0, this),
                new PlayingNote(new Note("G#1"), 0, this),
                new PlayingNote(new Note("B1"), 0, this),
                new PlayingNote(new Note("D#2"), 0, this),
            };
        }
        
        // 마우스가 텍스트 위로 들어왔을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            _playingNote.ForEach(note => note.PlayNote());
        }

        // 마우스가 텍스트에서 나갔을 때
        public void OnPointerExit(PointerEventData eventData)
        {
            _playingNote.ForEach(note => note.StopNote());
        }
    }
}