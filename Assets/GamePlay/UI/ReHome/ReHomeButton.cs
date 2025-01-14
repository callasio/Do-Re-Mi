using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GamePlay.UI.ReHome
{
    public class ReHomeButton : UIBehaviour
    {
        // Image 컴포넌트를 참조합니다.
        private Button _button;
        
        public override void Start()
        {
            base.Start();
            _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(OnClick);
            Debug.Log("Image clicked");
        }
        private static void OnClick() => StageLoader.ReHome();
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                OnClick();
            }
        }
    }
}