using Monotone;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GamePlay.StageData.Clear
{
    public class NextStage: MonoBehaviour
    {
        private int _currentStageIndex;
        private Button _nextButton;
        private void Start()
        {
            _nextButton = GetComponentInChildren<Button>();
            _nextButton.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            if (CurrentStage.Index + 1 < CurrentStage.StageCount)
            {
                CurrentStage.Index += 1;
                SceneManager.LoadScene("GamePlayScene");
            }
            else
            {
                Debug.Log("last stage");
            }
        }
    }
}



