using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoHome : MonoBehaviour
{
    // Image 컴포넌트를 참조합니다.
    public Image targetImage;
    private Button _button;
    
    private void Start()
    {
        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(OnImageClick);
        // _button.onClick.Invoke();
        Debug.Log("Image clicked");
    }

    // 클릭 시 실행되는 함수
    private void OnImageClick()
    {
        // 다음 씬 이름을 설정하세요.
        string nextSceneName = "HomeScene";
        
        // 씬을 로드합니다.
        SceneManager.LoadScene(nextSceneName);
    }
}
