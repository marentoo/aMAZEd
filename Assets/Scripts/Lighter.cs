using UnityEngine;

public class Lighter : MonoBehaviour {
    public float restoreTime = 120f; // Time added to the torch's burn time
    
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void UseLighter(Torch torch) {
        torch.RestoreLight(restoreTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player playerComponent = other.GetComponent<Player>();
            if (playerComponent != null)
            {
                //playerComponent.AddLighter(this);

                audioSource.Play();

                // Destroy the key GameObject after the sound finished playing
                Destroy(gameObject);//, audioSource.clip.length);
            }
        }
    }
}

