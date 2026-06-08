using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Card Setup")]
    public Card cardPrefab;
    public Transform gridParent;
    public Sprite[] cardSprites;

    [Header("Score")]
    public int score;
    public TMP_Text scoreText;

    [Header("Level")]
    public int currentLevel = 1;
    public int totalMatching;
    public int maxLevel = 5;

    [Header("Panels")]
    public GameObject winPanel;
    public GameObject gameOverPanel;

    [Header("Timer")]
    public TMP_Text timerText;
    public float timeLeft = 60f;
    public bool useTimer = false;

    [Header("Special Level Settings")]
    public int wrongPenalty = 0;
    public float checkDelay = 0.45f;

    private Card firstCard;
    private Card secondCard;
    private int matchedPairs;
    private bool gameEnded;

    public bool IsChecking { get; private set; }

    private void Start()
    {
        ApplyLevelDifficulty();
        StartGame();
    }

    private void Update()
    {
        if (!useTimer) return;
        if (gameEnded) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            UpdateTimerText();
            GameOver();
            return;
        }

        UpdateTimerText();
    }

    private void ApplyLevelDifficulty()
    {
        // Default Level 1-3
        useTimer = false;
        wrongPenalty = 0;
        checkDelay = 0.45f;

        // Level 4 special
        if (currentLevel == 4)
        {
            useTimer = true;
            timeLeft = 45f;
            wrongPenalty = 5;
            checkDelay = 0.4f;
        }

        // Level 5 special
        if (currentLevel == 5)
        {
            useTimer = true;
            timeLeft = 35f;
            wrongPenalty = 10;
            checkDelay = 0.3f;
        }

        UpdateTimerText();
    }

    public void StartGame()
    {
        score = 0;
        matchedPairs = 0;
        IsChecking = false;
        gameEnded = false;
        firstCard = null;
        secondCard = null;

        Time.timeScale = 1f;

        if (winPanel != null)
            winPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateScoreText();
        UpdateTimerText();
        CreateCards();
    }

    private void CreateCards()
    {
        if (cardPrefab == null)
        {
            Debug.LogError("Card Prefab belum diisi di GameManager.");
            return;
        }

        if (gridParent == null)
        {
            Debug.LogError("Grid Parent belum diisi di GameManager.");
            return;
        }

        if (cardSprites == null || cardSprites.Length == 0)
        {
            Debug.LogError("Card Sprites masih kosong. Isi gambar depan kartu di GameManager.");
            return;
        }

        for (int i = gridParent.childCount - 1; i >= 0; i--)
        {
            Destroy(gridParent.GetChild(i).gameObject);
        }

        List<int> ids = new List<int>();

        for (int i = 0; i < cardSprites.Length; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        Shuffle(ids);
        totalMatching = cardSprites.Length;

        foreach (int id in ids)
        {
            Card card = Instantiate(cardPrefab, gridParent);
            card.gameObject.SetActive(true);
            card.Init(this, id, cardSprites[id]);
        }
    }

    public void SelectCard(Card card)
    {
        if (gameEnded) return;
        if (IsChecking) return;
        if (card == null) return;

        if (firstCard == null)
        {
            firstCard = card;
            Debug.Log("Kartu pertama dibuka: " + card.id);
            return;
        }

        if (card == firstCard) return;

        secondCard = card;
        Debug.Log("Kartu kedua dibuka: " + card.id);

        StartCoroutine(CheckCards());
    }

    private IEnumerator CheckCards()
    {
        IsChecking = true;

        yield return new WaitForSeconds(checkDelay);

        if (firstCard != null && secondCard != null && firstCard.id == secondCard.id)
        {
            firstCard.SetMatched();
            secondCard.SetMatched();

            matchedPairs++;
            AddScore(10);

            if (matchedPairs >= totalMatching)
            {
                LevelComplete();
            }
        }
        else
        {
            if (firstCard != null)
                firstCard.CloseCard();

            if (secondCard != null)
                secondCard.CloseCard();

            if (wrongPenalty > 0)
                AddScore(-wrongPenalty);
        }

        firstCard = null;
        secondCard = null;
        IsChecking = false;
    }

    private void AddScore(int amount)
    {
        score += amount;

        if (score < 0)
            score = 0;

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;

        if (!useTimer)
        {
            timerText.gameObject.SetActive(false);
            return;
        }

        timerText.gameObject.SetActive(true);
        timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);
    }

    private void LevelComplete()
    {
        if (gameEnded) return;

        gameEnded = true;

        Debug.Log("Level Complete");

        UnlockNextLevel();

        if (winPanel != null)
            winPanel.SetActive(true);
    }

    private void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;

        Debug.Log("Game Over");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    private void UnlockNextLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        int nextLevel = Mathf.Clamp(currentLevel + 1, 1, maxLevel);

        if (nextLevel > unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", nextLevel);
            PlayerPrefs.Save();

            Debug.Log("Level " + nextLevel + " terbuka.");
        }
    }

    public void NextLevel()
    {
        int nextLevel = currentLevel + 1;

        if (nextLevel > maxLevel)
        {
            GoHome();
            return;
        }

        string sceneName = nextLevel == 1 ? "Level" : "Level" + nextLevel;
        SceneManager.LoadScene(sceneName);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);

            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}