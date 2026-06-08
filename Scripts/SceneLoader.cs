using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Kosongkan kalau tombol ini bukan untuk pindah scene")]
    public string sceneName;
    public int levelNumbers;

    public void LoadScene()
    {
        if (!string.IsNullOrWhiteSpace(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            return;
        }

        if (levelNumbers > 0)
        {
            SceneManager.LoadScene("Level" + levelNumbers);
            return;
        }

        Debug.LogWarning("SceneLoader: sceneName/levelNumbers belum diisi.");
    }

    public void LoadSceneByName(string targetScene)
    {
        if (!string.IsNullOrWhiteSpace(targetScene))
            SceneManager.LoadScene(targetScene);
    }

    public void LoadLevel(int level)
    {
        if (level <= 1) SceneManager.LoadScene("Level");
        else SceneManager.LoadScene("Level" + level);
    }

    public void Home()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = index + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
