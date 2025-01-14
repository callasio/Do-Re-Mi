using UnityEngine;
using UnityEngine.EventSystems;

namespace Home
{
    public class ImageSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }

        // 마우스가 이미지 위로 들어왔을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        // 마우스가 이미지에서 나갔을 때
        public void OnPointerExit(PointerEventData eventData)
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}