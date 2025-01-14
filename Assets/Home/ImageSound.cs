using UnityEngine;
using UnityEngine.EventSystems;

namespace Home
{
    public class ImageSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private GameObject backgroundGadient;

        private void Awake()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            audioSource.Stop();
            backgroundGadient.SetActive(false);
        }

        // 마우스가 이미지 위로 들어왔을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }

            if (backgroundGadient != null)
            {
                backgroundGadient.SetActive(true);
            }
            
        }

        // 마우스가 이미지에서 나갔을 때
        public void OnPointerExit(PointerEventData eventData)
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            if (backgroundGadient != null)
            {
                backgroundGadient.SetActive(false);
            }
        }
    }
}