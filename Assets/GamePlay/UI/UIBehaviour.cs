using UnityEngine;

namespace GamePlay.UI
{
    public abstract class UIBehaviour : MonoBehaviour
    {
        private const float Padding = 20;
        private const float Size = 100;
        private const float Gap = 20;
        
        public int Index { get; set; }
        private RectTransform _canvasRect;
        private RectTransform _contentRect;

        public virtual void Start()
        {
            _canvasRect = transform.GetChild(0).GetComponent<RectTransform>();
            _canvasRect.pivot = new Vector2(1, 1);
            _contentRect = _canvasRect.GetChild(0).GetComponent<RectTransform>();
            
            AdjustContentRect();
        }
        
        private void AdjustContentRect()
        {
            _contentRect.sizeDelta = new Vector2(Size, Size);
            _contentRect.anchoredPosition = new Vector2(-Padding - Size / 2, -Padding - (Size + Gap) * Index - Size / 2);
        }
    }
}