using UnityEngine;

public class CarEngineSound : MonoBehaviour
{
    public AudioSource engineSound;
    public float minPitch = 0.8f;
    public float maxPitch = 2f;

    private PrometeoCarController car;

    void Start()
    {
        car = GetComponent<PrometeoCarController>();
        engineSound.loop = true;
        engineSound.Play();
    }

    void Update()
    {
        float speed = Mathf.Abs(car.carSpeed);
        engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, speed / 120f);
    }
}