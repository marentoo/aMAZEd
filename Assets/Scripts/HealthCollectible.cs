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
                float PalyerHealthValue = playerHealth.getHealthValue();
                if((PalyerHealthValue + healthAmount) > 100){
                    playerHealth.SetHealthTo100();
                    //Debug.Log($"Yes, it is above 100!");
                    //Debug.Log($"Player health = {PalyerHealthValue}");
                }
                else{
                    playerHealth.RestoreHealth(healthAmount);
                    //Debug.Log($"Player health = {PalyerHealthValue}");
                }
                
                // You can also play a sound effect or particle effect here to indicate the collection.
                Destroy(gameObject);
            }
        }
    }
}
