using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(30, 30, 30); 
        transform.LookAt(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
