using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public TextMeshProUGUI highscoreDisplay;
    public TMP_InputField nameInputField;

    private void Start()
    {
        Time.timeScale = 1f;
        RefreshUI();
    }

    void RefreshUI()
    {
        // Load the Highscore (default to 0 if none exists)
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        
        // Load the Name associated with that highscore (default to "None")
        string highscoreName = PlayerPrefs.GetString("HighScoreName", "None");

        // Display it: e.g., "Best: PlayerOne - 500"
        if (highscoreDisplay != null)
        {
            highscoreDisplay.text = $"Highscore: {highscoreName} - {highscore}";
        }

        // Fill the input field with the CURRENT player's name
        if (nameInputField != null)
        {
            nameInputField.text = PlayerPrefs.GetString("PlayerName", "");
        }
    }

    public void SaveCurrentName()
    {
        PlayerPrefs.SetString("PlayerName", nameInputField.text);
        PlayerPrefs.Save();
    }

    public void ResetStats()
    {
        // Clear specific keys
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("HighScoreName");
        PlayerPrefs.DeleteKey("PlayerName");
        PlayerPrefs.Save();

        // Refresh UI to show 0 and empty strings
        RefreshUI();
    }

    public void GoToHomeScene()
    {
        SceneManager.LoadScene("HomeScene");
    }
}