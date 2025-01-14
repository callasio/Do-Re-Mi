using Monotone;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string Text { get; set; }
    private TextMeshProUGUI _textMesh;
    public int Index { get; set; }
    private Button _itemButton;
    void Start()
    {
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        _textMesh.text = Text;
        _itemButton = GetComponentInChildren<Button>();
        _itemButton.onClick.AddListener(OnClick);
    }
    
    private void OnClick()
    {
        CurrentStage.Index = Index;
        SceneManager.LoadScene("GamePlayScene");
    }
}
