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

        private static void OnClick()
        {
            // 다음 씬 이름을 설정하세요.
            string nextSceneName = "HomeScene";
            // 씬을 로드합니다.
            SceneManager.LoadScene(nextSceneName);
        }
    }
}