using GamePlay.StageData;
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
            item.Text = "Stage " + (i+1);
            item.Index = i;
        }
        // 레이아웃 리빌드
        // LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent);
    }
}


