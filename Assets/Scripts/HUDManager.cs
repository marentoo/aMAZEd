using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Slider healthSlider; // Reference to the UI slider for health.

    // This method updates the health slider's value.
    public void SetHealth(float health)
    {
        if(healthSlider != null)
        {
            Debug.Log("Updating health slider value to: " + health);
            healthSlider.value = health;
        }
        else
        {
            Debug.LogError("Health slider reference not set in the HUD Manager.");
        }
    }

    // Call this method when the player's health changes.
    public void OnHealthChanged(float currentHealth)
    {
        SetHealth(currentHealth);
    }

    // Additional methods to manage other HUD elements...
}
