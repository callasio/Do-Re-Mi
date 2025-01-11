using System;
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
        
        private static void OnClick() => Player.RecordClicked();

        public void OnPointerEnter(PointerEventData eventData) => Player.RecordHovered();
        
        public void OnPointerExit(PointerEventData eventData) => Player.RecordHoverEnded();
        
    }
}
