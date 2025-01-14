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

    public Image ItemImage => _image;
    private Image _image;
    void Start()
    {
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        _textMesh.text = Text;
        _itemButton = GetComponentInChildren<Button>();
        _image = GetComponentInChildren<Image>();
        _itemButton.onClick.AddListener(OnClick);
        
        string stageKey = $"StageClear_{Index - 1}";
        if (PlayerPrefs.GetInt(stageKey, 0) == 1 || Index == 0)
        {
            ItemImage.color = Color.white;
            Debug.Log(stageKey);
        }
        else
        {
            ItemImage.color = Color.gray;
        }
    }
    
    private void OnClick()
    {
        CurrentStage.Index = Index;
        string stageKey = $"StageClear_{Index -1}";
        if (Index == 0 || PlayerPrefs.GetInt(stageKey, 0) == 1)
        {
            SceneManager.LoadScene("GamePlayScene");
        }
    }
}
