using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int scoreToAdd = 10;
    public float timeToAdd = 10f;

    public float spinSpeed = 90f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.2f;

    public AudioClip checkpointSound;

    private float startY;
    private bool collected = false;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        if (collected) return;

        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);

        float newY = startY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        // Check the collider itself, its parent, or its root for the Player tag
        bool isPlayer = other.CompareTag("Player") 
                     || (other.transform.parent != null && other.transform.parent.CompareTag("Player"))
                     || other.transform.root.CompareTag("Player");

        if (!isPlayer) return;

        collected = true;

        if (GameManager.instance != null)
        {
            GameManager.instance.AddScore(scoreToAdd);
            GameManager.instance.AddTime(timeToAdd);
            GameManager.instance.ShowCheckpointMessage();
        }
        else
        {
            Debug.LogWarning("Checkpoint: No GameManager found in the scene!");
        }

        if (checkpointSound != null)
        {
            AudioSource.PlayClipAtPoint(checkpointSound, transform.position);
        }

        gameObject.SetActive(false);
    }
}