using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI.Restart
{
    public class RestartButton : UIBehaviour
    {
        private Button _button;
        
        public override void Start()
        {
            base.Start();
            _button = GetComponentInChildren<Button>();
            
            _button.onClick.AddListener(OnClick);
        }

        private static void OnClick() => StageLoader.Restart();

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                OnClick();
            }
        }
    }
}
