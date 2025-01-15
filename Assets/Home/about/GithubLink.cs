using UnityEngine;
using UnityEngine.UI;

public class GithubLink : MonoBehaviour
{
    public Button button;
    public string url = "https://github.com/callasio/Do-Re-Mi.git";

    void Start()
    {
        button.onClick.AddListener(OpenLink);
    }

    void OpenLink()
    {
        Application.OpenURL(url);
    }
}


