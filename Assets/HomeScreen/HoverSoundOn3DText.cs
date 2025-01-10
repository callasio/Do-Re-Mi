using UnityEngine;

public class MouseHoverSound : MonoBehaviour
{
    // public AudioSource audioSource;  // 소리를 재생할 AudioSource
    // public AudioClip hoverSound;     // 마우스를 올렸을 때 재생할 소리

    private void Start()
    {
        // AudioSource가 없으면 자동으로 추가
        // if (audioSource == null)
        // {
        //     audioSource = GetComponent<AudioSource>();
        // }
    }

    private void Update()
    {
        // 마우스 좌표로 Ray를 쏴서 해당 오브젝트가 맞는지 확인
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        RaycastHit hit;

        // Raycast가 오브젝트에 충돌하면
        if (Physics.Raycast(ray, out hit))
        {
            // 만약 마우스가 현재 이 오브젝트에 올려졌다면
            if (hit.transform == transform)
            {
                Debug.Log("Mouse is over the object! Playing sound.");
                Debug.Log("Hit: " + hit.transform.name);
                // // 소리가 아직 재생 중이 아니라면 소리를 재생
                // if (!audioSource.isPlaying)
                // {
                //     audioSource.PlayOneShot(hoverSound);
                //     Debug.Log("Mouse is over the object! Playing sound.");
                // }
            }
        }
    }
}