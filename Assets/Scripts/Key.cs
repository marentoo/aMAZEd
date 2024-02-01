using UnityEngine;

public class Key : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player playerComponent = other.GetComponent<Player>();
            if (playerComponent != null)
            {
                playerComponent.AddKey(this);

                audioSource.Play();

                // Destroy the key GameObject after the sound finished playing
                Destroy(gameObject, audioSource.clip.length);
            }
        }
    }
}
