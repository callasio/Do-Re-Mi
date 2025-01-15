using System;
using Monotone;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GamePlay.UI.Guide
{
    public class GuideButton : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Sprite guideImage1;
        public Sprite guideImage2;
        public Sprite guideImage3;
        private GameObject _imageObject;

        public override void Start()
        {
            base.Start();
            var index = CurrentStage.Index + 1;
            if (index > 3) return;
            
            _imageObject = transform.GetChild(0).GetChild(1).gameObject;
            if (_imageObject is null) return;
            var image = _imageObject.GetComponent<Image>();

            image.sprite = index switch
            {
                1 => guideImage1,
                2 => guideImage2,
                3 => guideImage3,
                _ => image.sprite
            };
            
            _imageObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData) => _imageObject?.SetActive(true);

        public void OnPointerExit(PointerEventData eventData) => _imageObject?.SetActive(false);
    }
}