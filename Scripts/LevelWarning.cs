using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWarning : MonoBehaviour
{
    public GameObject warningPanel;

    private string targetScene;

    public void ShowWarning(string sceneName)
    {
        targetScene = sceneName;

        warningPanel.SetActive(true);
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(targetScene);
    }
}