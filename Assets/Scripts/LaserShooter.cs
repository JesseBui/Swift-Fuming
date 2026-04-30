using System.Collections;
using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    public float laserRange = 50f;
    public KeyCode shootKey = KeyCode.F;
    public Transform shootPoint;           
    public float laserVisibleDuration = 0.1f;
    public Color laserColor = Color.red;    
    public float laserWidth = 0.1f;         

    public AudioSource laserSound;

    private LineRenderer lineRenderer;

    void Start()
    {
        // Add and set up the LineRenderer on the shoot point
        lineRenderer = shootPoint.gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false; // Hidden until we shoot
    }

    void Update()
    {
        if (Input.GetKeyDown(shootKey))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (laserSound != null)
            laserSound.Play();

        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        RaycastHit hit;

        Vector3 endPoint;

        if (Physics.Raycast(ray, out hit, laserRange))
        {
            endPoint = hit.point; // Laser stops at what it hits

            ObstacleHit obstacle = hit.collider.GetComponentInParent<ObstacleHit>();
            if (obstacle != null)
            {
                obstacle.TakeHit();
            }
        }
        else
        {
            endPoint = shootPoint.position + shootPoint.forward * laserRange; 
        }

        // Show the laser line
        lineRenderer.SetPosition(0, shootPoint.position);
        lineRenderer.SetPosition(1, endPoint);

        StartCoroutine(ShowLaser());
    }

    IEnumerator ShowLaser()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(laserVisibleDuration);
        lineRenderer.enabled = false;
    }
}