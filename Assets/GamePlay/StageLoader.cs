using System;
using GamePlay.StageData;
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
        
        // Start is called before the first frame update
        void Start()
        {
            _camera = Camera.main;
            _stages = new Stages(this);
            OnStage(1);
        }

        void OnStageSelect()
        {
            _stages.DestroyCurrentStage();
            _stages.InitStageSelect();
        }

        void OnStage(int stageIndex)
        {
            _stages.DestroyCurrentStage();
            _stages.InitStage(stageIndex);
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
            var layerMask = 1 << LayerMask.NameToLayer("Default");
            
            if (Physics.Raycast(ray, out var hit, float.MaxValue, layerMask))
            {
                var hitCollider = hit.collider.transform.parent.GameObject();
                if (hitCollider.TryGetComponent<StageElementBehaviour>(out var stageElement))
                {
                    stageElement.OnClicked();
                }
            }
        }
    }
}
