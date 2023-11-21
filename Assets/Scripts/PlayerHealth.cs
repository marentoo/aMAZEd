using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public Slider healthSlider;
    private float currentHealth;
    public HUDManager hudManager;

    private void Start()
    {
        currentHealth = maxHealth;
        //healthSlider.maxValue = maxHealth;
        //healthSlider.value = currentHealth;
        hudManager.SetHealth(currentHealth);
    }


    

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        //healthSlider.value = currentHealth;

        Debug.Log($"PlayerHealth: {currentHealth}");
        hudManager.OnHealthChanged(currentHealth);

        if (currentHealth <= 0)
        {
            //handle respawn here
            Debug.LogError($"Game over! PlayerHealth: {currentHealth}");
        }
    }
}
