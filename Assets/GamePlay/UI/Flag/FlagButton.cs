using GamePlay.StageData.Player;
using GamePlay.StageData.Player.Sound;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay.UI.Flag
{
    public class FlagButton : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SoundManager.FlagHovered();
            } 
            if (Input.GetKeyUp(KeyCode.F))
            {
                SoundManager.FlagHoverEnded();
            }
        }

        public void OnPointerEnter(PointerEventData eventData) => SoundManager.FlagHovered();

        public void OnPointerExit(PointerEventData eventData) => SoundManager.FlagHoverEnded();
    }
}