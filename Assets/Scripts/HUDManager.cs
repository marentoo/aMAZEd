using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public Slider healthSlider; // Reference to the UI slider for health.
    public TMP_Text keyCountText;
    public TMP_Text lighterCountText;
    public TMP_Text matchesCountText;
    private Player player;    // Reference to the Player script


    // This method updates the health slider's value.
    public void SetHealth(float health)
    {
        if (healthSlider != null)
        {
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

    public void UpdateKeyDisplay()
    {
        player = FindObjectOfType<Player>();

        if (keyCountText != null)
        {
            keyCountText.text = $" {player.keysCollected}/{GameManager.numberOfKeys} ";
        }
        else
        {
            Debug.LogError("Key count Text reference or Player reference not set in HUD Manager.");
        }
    }
    
    public void UpdateLighterDisplay()
    {
        player = FindObjectOfType<Player>();

        if (lighterCountText != null)
        {
            lighterCountText.text = $" {player.lightersCollected} ";
        }
        else
        {
            Debug.LogError("Lighter count Text reference or Player reference not set in HUD Manager.");
        }
    }
    public void UpdateMatchesDisplay()
    {
        player = FindObjectOfType<Player>();

        if (matchesCountText != null)
        {
            matchesCountText.text = $" {player.matchesCollected} ";
        }
        else
        {
            Debug.LogError("Match count Text reference or Player reference not set in HUD Manager.");
        }
    }

    void Update()
    {
        if (player == null)
        {
            // Try to find the Player instance in the scene.
            player = FindObjectOfType<Player>();
            if (player == null)
            {
                return; // Player not found yet, return and check again in the next frame.
            }
        }

        UpdateKeyDisplay();
        //UpdateLighterDisplay();
        UpdateMatchesDisplay();
    }
}