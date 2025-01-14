using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Home
{
    public class ImageSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject backgroundGadient;

        private void Awake()
        {
            backgroundGadient.SetActive(false);
        }

        // 마우스가 이미지 위로 들어왔을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
                Debug.Log("audio source is playing");
            if (backgroundGadient != null)
            {
                backgroundGadient.SetActive(true);
            }
            
        }

        // 마우스가 이미지에서 나갔을 때
        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("audio source is stopped");
            if (backgroundGadient != null)
            {
                backgroundGadient.SetActive(false);
            }
        }
    }
}