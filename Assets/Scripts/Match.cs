using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{
    public float restoreTime = 60f; // Time added to the torch's burn time
    
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void UseMatch(Torch torch) {
        torch.RestoreLight(restoreTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player playerComponent = other.GetComponent<Player>();
            if (playerComponent != null)
            {
                playerComponent.AddMatch(this);

                audioSource.Play();

                // Destroy the key GameObject after the sound finished playing
                Destroy(gameObject);//, audioSource.clip.length);
            }
        }
    }
}


