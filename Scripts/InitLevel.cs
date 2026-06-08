using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitLevel : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text smallTitleText;

    void Start()
    {
        int level = GetLevelNumber();
        if (titleText != null) titleText.text = "LEVEL " + level;
        if (smallTitleText != null) smallTitleText.text = "Level " + level;
    }

    int GetLevelNumber()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Level") return 1;

        string digits = "";
        foreach (char c in sceneName)
        {
            if (char.IsDigit(c)) digits += c;
        }

        if (int.TryParse(digits, out int number)) return number;
        return 1;
    }
}
