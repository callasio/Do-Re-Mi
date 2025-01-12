using GamePlay.StageData.Player;
using GamePlay.StageData.Player.Sound;
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
            SoundManager.RecordClicked();
            Bump();
        }

        public void OnPointerEnter(PointerEventData eventData) => SoundManager.RecordHovered();
        
        public void OnPointerExit(PointerEventData eventData) => SoundManager.RecordHoverEnded();

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                OnClick();
            }
        }
    }
}
