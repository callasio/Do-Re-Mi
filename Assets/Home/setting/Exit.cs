using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Home.setting
{
    public class Exit : MonoBehaviour
    {
        // Image 컴포넌트를 참조합니다.
        private Button _button;
    
        private void Start()
        {
            _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(OnImageClick);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                OnImageClick();
            }
        }

        // 클릭 시 실행되는 함수
        private void OnImageClick()
        {
            // 다음 씬 이름을 설정하세요.
            Application.Quit();
        }
    }
}