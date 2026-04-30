using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.CompareTag("Player")
                     || (other.transform.parent != null && other.transform.parent.CompareTag("Player"))
                     || other.transform.root.CompareTag("Player");

        if (!isPlayer) return;

        if (GameManager.instance != null)
        {
            GameManager.instance.TriggerWin();
        }
    }
}