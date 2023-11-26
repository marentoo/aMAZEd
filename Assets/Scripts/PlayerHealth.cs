using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private float currentHealth;
    private HUDManager hudManager;
    //public float CurrentHealth { get { return currentHealth; } }

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

        hudManager.SetHealth(currentHealth);


    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        Debug.Log($"PlayerHealth: {currentHealth}");
        hudManager.OnHealthChanged(currentHealth);


        if (currentHealth <= 0)
        {
            if (DeathScreen.Instance != null)
            {
                DeathScreen.Instance.ShowDeathScreen();
            }
            else
            {
                Debug.LogError("DeathScreen component not assigned.");
            }
        }
    }

    public void RestoreHealth(float amount)
    {
        currentHealth += amount;

        hudManager.OnHealthChanged(currentHealth);
    }
}