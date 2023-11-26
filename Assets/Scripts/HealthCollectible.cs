using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public float healthAmount = 20f; // Amount of health to restore

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.RestoreHealth(healthAmount);
                // You can also play a sound effect or particle effect here to indicate the collection.
                Destroy(gameObject);
            }
        }
    }
}
