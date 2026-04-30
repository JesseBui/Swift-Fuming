using System.Collections;
using UnityEngine;

public class ObstacleHit : MonoBehaviour
{
    public int hitsToDestroy = 3;          
    public float flashDuration = 0.2f;     

    public AudioClip hitSound;             

    private int currentHits = 0;
    private Renderer[] renderers;          
    private Color[] originalColors;
    private AudioSource audioSource;

    void Start()
    {
        // Get all renderers on this object and its children
        renderers = GetComponentsInChildren<Renderer>();

        // Save all original colors
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void TakeHit()
    {
        currentHits++;

        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        if (currentHits >= hitsToDestroy)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    IEnumerator FlashRed()
    {
        // Turn all renderers red
        foreach (Renderer r in renderers)
            r.material.color = Color.red;

        yield return new WaitForSeconds(flashDuration);

        // Restore original colors
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = originalColors[i];
    }
}