using System.Collections.Generic;
using System.Linq;
using Common.Sound;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

namespace Home
{
    public class ImageSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IAudioPlayer
    {
        [SerializeField] private GameObject backgroundGadient;
        public List<string> notes;
        private List<PlayingNote> _playingNote;
        
        public GameObject GameObject => gameObject;
        public AudioClip cClip;
        public AudioMixerGroup reverbMixerGroup;
        public AudioClip CClip => cClip;
        public AudioMixerGroup ReverbMixerGroup => reverbMixerGroup;

        public void Start()
        {
            _playingNote = notes.Select(note => new PlayingNote(new Note(note), 0, this)).ToList();
        }

        private void Awake()
        {
            if (backgroundGadient != null) backgroundGadient.SetActive(false);
        }

        // 마우스가 이미지 위로 들어왔을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            _playingNote.ForEach(note => note.PlayNote());
            if (backgroundGadient != null)
            {
                backgroundGadient.SetActive(true);
            }
        }

        // 마우스가 이미지에서 나갔을 때
        public void OnPointerExit(PointerEventData eventData)
        {
            _playingNote.ForEach(note => note.StopNote());
            if (backgroundGadient != null)
            {
                backgroundGadient.SetActive(false);
            }
        }
    }
}