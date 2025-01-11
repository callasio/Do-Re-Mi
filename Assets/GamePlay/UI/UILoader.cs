using System.Collections.Generic;
using System.Linq;
using GamePlay.StageData;
using GamePlay.UI.Record;
using GamePlay.UI.Start;
using UnityEngine;

namespace GamePlay.UI
{
    public class UILoader : MonoBehaviour
    {
        public GameObject startButtonPrefab;
        public GameObject recordButtonPrefab;
    
        public void ReloadUI(StageConfiguration stageConfiguration)
        {
            DisposeUI();
            CreateUI(stageConfiguration);
        }

        private void DisposeUI()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        
        private void CreateUI(StageConfiguration stageConfiguration)
        {
            var uiTypes = stageConfiguration.UIs;
            
            for (var i = 0; i < uiTypes.Count; i++)
            {
                switch (uiTypes.ElementAt(i))
                {
                    case UIType.Start:
                        var startButton = Instantiate(startButtonPrefab, transform);
                        startButton.GetComponent<StartButton>().Index = i;
                        break;
                    case UIType.Record:
                        var recordButton = Instantiate(recordButtonPrefab, transform);
                        recordButton.GetComponent<RecordButton>().Index = i;
                        break;
                    case UIType.Goal:
                    default:
                        break;
                }
            }
        }
    }
}