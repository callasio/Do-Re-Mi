using Common.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GamePlay.UI.Back
{
    public class BackButton: UIBehaviour
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
            SceneManager.LoadScene("SelectScene");
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnClick();
            }
        }
    }
}