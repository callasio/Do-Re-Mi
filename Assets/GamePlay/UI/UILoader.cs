using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.StageData;
using GamePlay.UI.Flag;
using GamePlay.UI.Record;
using GamePlay.UI.Restart;
using UnityEngine;

namespace GamePlay.UI
{
    public class UILoader : MonoBehaviour
    {
        public GameObject recordButtonPrefab;
        public GameObject flagButtonPrefab;
        public GameObject restartButtonPrefab;
    
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
                    case UIType.Record:
                        var recordButton = Instantiate(recordButtonPrefab, transform);
                        recordButton.GetComponent<RecordButton>().Index = i;
                        break;
                    case UIType.Goal:
                        var flagButton = Instantiate(flagButtonPrefab, transform);
                        flagButton.GetComponent<FlagButton>().Index = i;
                        break;
                    case UIType.Restart:
                        var restartButton = Instantiate(restartButtonPrefab, transform);
                        restartButton.GetComponent<RestartButton>().Index = i;
                        break;
                }
            }
        }
    }
}