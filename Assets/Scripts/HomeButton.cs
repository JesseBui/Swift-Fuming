using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnHomeButtonClick()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);

        Time.timeScale = 1f;
        SceneManager.LoadScene("HomeScene");
    }
}