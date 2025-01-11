using System;
using GamePlay.StageData.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GamePlay.UI.Start
{
    public class StartButton : UIBehaviour, IPointerClickHandler
    {
        private Button _button;
        
        public override void Start()
        {
            base.Start();
            _button = GetComponentInChildren<Button>();
            
            _button.onClick.AddListener(OnClick);
        }
        
        private static void OnClick() => Player.StartClicked();
        public void OnPointerClick(PointerEventData eventData) => Player.StartClicked();
    }
}
