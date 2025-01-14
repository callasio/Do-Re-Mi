using System;
using GamePlay.StageData;
using GamePlay.UI;
using Monotone;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class StageLoader : MonoBehaviour
    {
        public GameObject playerPrefab;
        public GameObject tilePrefab;
        public GameObject speakerPrefab;
        private Stages _stages;
        private Camera _camera;
        private UILoader _uiLoader;
        private int? _currentStageIndex;
        private StageData.StageData CurrentStageData
        {
            set => _uiLoader.ReloadUI(value.Configuration);
        }

        private static event Action OnFinish;
        private static event Action OnRestart;
        public static void Finish() => OnFinish?.Invoke();
        public static void Restart() => OnRestart?.Invoke();
        
        void Start()
        {
            _camera = Camera.main;
            _stages = new Stages(this);
            _uiLoader = GetComponentInChildren<UILoader>();
            _currentStageIndex = null;
            
            OnFinish += FinishedHandler;
            OnRestart += RestartHandler;
            
            OnStage(CurrentStage.Index);
        }

        private void OnDestroy()
        {
            OnFinish -= FinishedHandler;
            OnRestart -= RestartHandler;
        }

        void OnHome()
        {
            _stages.DestroyCurrentStage();
            CurrentStageData = _stages.InitHome();
        }

        void OnStageSelect()
        {
            _stages.DestroyCurrentStage();
            CurrentStageData = _stages.InitStageSelect();
        }

        void OnStage(int stageIndex)
        {
            _currentStageIndex = stageIndex;
            _stages.DestroyCurrentStage();
            CurrentStageData = _stages.InitStage(stageIndex);
        }
        
        private void FinishedHandler()
        {
            if (_currentStageIndex == null) return;
            PlayerPrefs.SetInt("StageIndex", _currentStageIndex.Value);
            SceneManager.LoadScene("GamePlayScene");
        }
        
        private void RestartHandler()
        {
            if (_currentStageIndex == null) return;
            OnStage(_currentStageIndex.Value);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseDown();
            }
        }
        
        private void OnMouseDown()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            var layerMask = 1 << LayerMask.NameToLayer("Stage Element");
            
            if (Physics.Raycast(ray, out var hit, float.MaxValue, layerMask))
            {
                var hitCollider = hit.collider.transform.parent;
                if (hitCollider.TryGetComponent<StageElementBehaviour>(out var stageElement))
                {
                    stageElement.OnClicked(hit.normal);
                } else if (hitCollider.parent.TryGetComponent(out stageElement))
                {
                    stageElement.OnClicked(hit.normal);
                }
            }
        }
    }
}
