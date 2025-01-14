using Monotone;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GamePlay.StageData.Clear
{
    public class NextStage: MonoBehaviour
    {
        [SerializeField] private GameObject background;
        private Button _nextButton;
        private void Start()
        {
            if (CurrentStage.Index != CurrentStage.StageCount - 1)
            {
                _nextButton = GetComponentInChildren<Button>();
                _nextButton.onClick.AddListener(OnClick);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void OnClick()
        {
            if (CurrentStage.Index + 1 >= CurrentStage.StageCount) return;
            CurrentStage.Index += 1;
            SceneManager.LoadScene("GamePlayScene");
        }
    }
}



