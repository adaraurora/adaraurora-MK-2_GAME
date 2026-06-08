using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("Level Setting")]
    public int levelNumber = 1;

    [Header("UI")]
    public GameObject lockIcon;
    public TMP_Text textNumber;
    public GameObject playButton;

    [Header("Sprite")]
    public Sprite lockedSprite;
    public Sprite unlockedSprite;

    [Header("Display")]
    public bool hideNumberWhenLocked = true;

    private Button button;
    private Image image;

    void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    void Start()
    {
        RefreshState();

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OpenLevel);
        }
    }

    public void RefreshState()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        bool unlocked = levelNumber <= unlockedLevel;

        if (textNumber != null)
        {
            textNumber.text = levelNumber.ToString();
            textNumber.alpha = 1f;
            textNumber.raycastTarget = false;
            textNumber.gameObject.SetActive(unlocked || !hideNumberWhenLocked);
        }

        if (lockIcon != null)
        {
            lockIcon.SetActive(!unlocked);
        }

        if (playButton != null)
        {
            playButton.SetActive(unlocked);
        }

        if (button != null)
        {
            button.interactable = unlocked;
        }

        if (image != null)
        {
            if (unlocked && unlockedSprite != null)
            {
                image.sprite = unlockedSprite;
            }
            else if (!unlocked && lockedSprite != null)
            {
                image.sprite = lockedSprite;
            }
        }

        // Urutan render UI: yang terakhir di hierarchy tampil paling atas.
        if (unlocked)
        {
            if (playButton != null) playButton.transform.SetAsLastSibling();
            if (textNumber != null) textNumber.transform.SetAsLastSibling();
        }
        else
        {
            if (lockIcon != null) lockIcon.transform.SetAsLastSibling();
        }
    }

    public void OpenLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (levelNumber > unlockedLevel)
        {
            Debug.Log("Level " + levelNumber + " masih terkunci.");
            return;
        }

        string sceneName = levelNumber == 1 ? "Level" : "Level" + levelNumber;

        Debug.Log("Membuka scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void UnlockAllForDemo()
    {
        PlayerPrefs.SetInt("UnlockedLevel", 5);
        PlayerPrefs.Save();
        RefreshState();
        Debug.Log("Semua level dibuka untuk demo.");
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.Save();
        RefreshState();
        Debug.Log("Progress direset. Hanya Level 1 yang terbuka.");
    }
}
