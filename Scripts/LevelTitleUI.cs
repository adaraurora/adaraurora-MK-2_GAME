using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTitleUI : MonoBehaviour
{
    public TMP_Text levelText;
    public CanvasGroup canvasGroup;
    public float showDuration = 1.6f;
    public string prefix = "LEVEL ";

    void Start()
    {
        ApplyTitle();
        StartCoroutine(FadeOut());
    }

    public void ApplyTitle()
    {
        if (levelText == null)
            levelText = GetComponentInChildren<TMP_Text>();

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        int number = DetectLevelNumber();
        if (levelText != null)
            levelText.text = prefix + number;

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;
    }

    int DetectLevelNumber()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Level") return 1;

        string digits = "";
        foreach (char c in sceneName)
        {
            if (char.IsDigit(c)) digits += c;
        }

        if (int.TryParse(digits, out int number))
            return number;

        return 1;
    }

    IEnumerator FadeOut()
    {
        if (canvasGroup == null) yield break;

        yield return new WaitForSeconds(showDuration);

        float t = 0f;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / 0.5f);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
