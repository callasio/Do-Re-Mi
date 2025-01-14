using Monotone;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GamePlay.StageData.Clear
{
    public class NextStage: MonoBehaviour
    {
        private int _currentStageIndex;
        [SerializeField] private GameObject background;
        private Button _nextButton;
        private void Start()
        {
            if (_currentStageIndex != CurrentStage.StageCount - 1)
            {
                _nextButton = GetComponentInChildren<Button>();
                _nextButton.onClick.AddListener(OnClick);
            }
            else
            {
                Destroy(this);
            }
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
            }
        }
    }
}



