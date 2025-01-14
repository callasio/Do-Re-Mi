using TMPro;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string Text { get; set; }
    private TextMeshProUGUI _textMesh;
    void Start()
    {
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        _textMesh.text = Text;
    }

    // Update is called once per frame
    void Update()
    {
        _textMesh.text = Text;
    }
}
