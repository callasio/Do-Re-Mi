using System;
using GamePlay.StageData;
using GamePlay.UI;
using Unity.VisualScripting;
using UnityEngine;

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
        private StageData.StageData CurrentStageData
        {
            set => _uiLoader.ReloadUI(value.Configuration);
        }

        // Start is called before the first frame update
        void Start()
        {
            _camera = Camera.main;
            _stages = new Stages(this);
            _uiLoader = GetComponentInChildren<UILoader>();
            OnStage(2);
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
            _stages.DestroyCurrentStage();
            CurrentStageData = _stages.InitStage(stageIndex);
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
