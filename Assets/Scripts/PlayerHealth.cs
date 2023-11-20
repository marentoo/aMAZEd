using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private float currentHealth;
    private HUDManager hudManager;

    private void Start()
    {
        currentHealth = maxHealth;

        // Find the HUDManager in the scene
        hudManager = FindObjectOfType<HUDManager>();
        if (hudManager == null)
        {
            Debug.LogError("HUDManager not found in the scene!");
            return;
        }

        // Initialize the health UI with the starting health
        hudManager.SetHealth(currentHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        Debug.Log($"PlayerHealth: {currentHealth}");
        hudManager.OnHealthChanged(currentHealth);

        if (currentHealth <= 0)
        {
            //handle respawn here
            Debug.LogError($"Game over! PlayerHealth: {currentHealth}");
        }
    }
}
