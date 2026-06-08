using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryRushMenuPolish : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text subtitleText;
    public Button playButton;
    public GameObject settingsPanel;
    public GameObject levelsPanel;
    public GameObject infoPanel;

    void Start()
    {
        if (titleText != null) titleText.text = "MEMORY RUSH";
        if (subtitleText != null) subtitleText.text = "Match the cards. Clear the level. Don't embarrass yourself.";
        CloseAllPanels();
    }

    public void OpenLevels()
    {
        CloseAllPanels();
        if (levelsPanel != null) levelsPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        CloseAllPanels();
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void OpenInfo()
    {
        CloseAllPanels();
        if (infoPanel != null) infoPanel.SetActive(true);
    }

    public void CloseAllPanels()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (levelsPanel != null) levelsPanel.SetActive(false);
        if (infoPanel != null) infoPanel.SetActive(false);
    }
}
