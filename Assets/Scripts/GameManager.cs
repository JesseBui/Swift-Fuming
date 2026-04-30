using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float startingTime = 60f;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI checkpointMessage;
    public GameObject endPanel;
    public TextMeshProUGUI endPanelText;

    public AudioSource backgroundMusic;
    public AudioClip gameOverSound;
    public AudioClip youWinSound;

    private int score = 0;
    private float timeLeft;
    private bool gameIsOver = false;
    private AudioSource audioSource;

    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        timeLeft = startingTime;

        if (endPanel != null) endPanel.SetActive(false);
        if (checkpointMessage != null) checkpointMessage.gameObject.SetActive(false);

        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }

        UpdateScoreUI();
        UpdateTimerUI();
    }

    private void SaveHighScore()
    {
        // 1. Get the current saved highscore (default to 0 if none)
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);

        // 2. Check if the current game score is higher
        if (score > currentHighScore)
        {
            // Save the new score
            PlayerPrefs.SetInt("HighScore", score);

            // Save the name of the player who got it
            string playerName = PlayerPrefs.GetString("PlayerName", "Unknown");
            PlayerPrefs.SetString("HighScoreName", playerName);

            PlayerPrefs.Save();
            Debug.Log("New Highscore Saved: " + score);
        }
    }

    void Update()
    {
        if (gameIsOver) return;

        timeLeft -= Time.deltaTime;
        UpdateTimerUI();

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            TriggerGameOver();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void AddTime(float amount)
    {
        timeLeft += amount;
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeLeft / 60f);
            int seconds = Mathf.FloorToInt(timeLeft % 60f);
            timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
            timerText.color = timeLeft <= 10f ? Color.red : Color.white;
        }
    }

    public void ShowCheckpointMessage()
    {
        StopCoroutine("HideCheckpointMessage");
        StartCoroutine("HideCheckpointMessage");
    }

    IEnumerator HideCheckpointMessage()
    {
        checkpointMessage.text = "Checkpoint Reached! +10pts +10s";
        checkpointMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        checkpointMessage.gameObject.SetActive(false);
    }

    void TriggerGameOver()
    {
        SaveHighScore();
        gameIsOver = true;
        if (backgroundMusic != null) backgroundMusic.Stop();
        if (audioSource != null && gameOverSound != null)
            audioSource.PlayOneShot(gameOverSound);

        endPanelText.color = Color.red;
        endPanelText.text = "Game Over!\nFinal Score: " + score;
        endPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void TriggerWin()
    {
        SaveHighScore();
        if (gameIsOver) return;
        gameIsOver = true;
        if (backgroundMusic != null) backgroundMusic.Stop();
        if (audioSource != null && youWinSound != null)
            audioSource.PlayOneShot(youWinSound);

        endPanelText.color = Color.green;
        endPanelText.text = "You Win!\nFinal Score: " + score;
        endPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // --- HOME BUTTON ---

    public void GoToHomeScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HomeScene");
    }
}