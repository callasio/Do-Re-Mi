using System.Collections.Generic;
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
            item.Text = nameList[i];
            item.Index = i;
        }
    }
    string[] nameList = new []
    {
        "Doe, a deer, a female deer",
        "Ray(Re), a drop of golden sun",
        "Me(Mi), a name I call myself",
        "Far(Fa), a long, long way to run",
        "Sew(So), a needle pulling thread",
        "La(La), a note to follow Sew",
        "Tea(Ti), a drink with jam and bread",
    };
}


