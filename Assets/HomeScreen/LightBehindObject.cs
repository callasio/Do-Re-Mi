using UnityEngine;

public class LightBehindObject : MonoBehaviour
{
    public Light pointLight; // Light 객체
    public Transform target; // 대상 오브젝트

    void Start()
    {
        // 대상 오브젝트의 뒤쪽으로 Point Light 배치
        if (pointLight != null && target != null)
        {
            Vector3 behindTarget = target.position - target.forward * 5f; // 오브젝트 뒤로 5 단위 이동
            pointLight.transform.position = behindTarget;
        }
    }
}

