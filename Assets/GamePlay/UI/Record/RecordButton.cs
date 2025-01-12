using System;
using DG.Tweening;
using GamePlay.StageData.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GamePlay.UI.Record
{
    public class RecordButton : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Button _button;
        
        public override void Start()
        {
            base.Start();
            _button = GetComponentInChildren<Button>();
            
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            Player.RecordClicked();
            Bump();
        }

        public void OnPointerEnter(PointerEventData eventData) => Player.RecordHovered();
        
        public void OnPointerExit(PointerEventData eventData) => Player.RecordHoverEnded();

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                OnClick();
            }
        }
    }
}
