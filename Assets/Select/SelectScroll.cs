using System.Collections.Generic;
using System.Linq;
using GamePlay.StageData;
using Home;
using Monotone;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    [SerializeField] private GameObject prefab;  // 사용할 Prefab
    private void Start()
    {
        for (int i = 0; i < CurrentStage.StageCount; i++)
        {
            var newObject = Instantiate(prefab, transform, true);
            newObject.name = "Item " + (i+1);  // 오브젝트 이름 지정
            newObject.SetActive(true);  // 필요할 경우 활성화

            // 텍스트 설정
            var item = newObject.GetComponent<NewMonoBehaviourScript>();
            item.Text = nameList[i];
            item.Index = i;

            var sound = newObject.GetComponent<ImageSound>();
            if (Stages.StageConfigurations.Length <= i) continue;
            sound.notes = Stages.StageConfigurations[i].Goal?.Select(note => note.ToString()).ToList() ?? new List<string>();
        }
    }
    string[] nameList = new []
    {
        "Do",
        "Re",
        "Mi",
        "Fa",
        "So",
        "La",
        "Ti",
    };
}


