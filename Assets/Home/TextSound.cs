using UnityEngine;
using UnityEngine.EventSystems;

namespace Home
{
    public class TextSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // 마우스가 텍스트 위로 들어왔을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Audio Source is playing");
        }

        // 마우스가 텍스트에서 나갔을 때
        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Audio Source is stopping");
        }
    }
}