using UnityEngine;
using UnityEngine.UI; // or TMPro;
using System.Collections;

public class DeathScreen : MonoBehaviour
{
    public GameObject loadGameButton;
    public GameObject restartGameButton;
    public GameObject quitGameButton;
    public static DeathScreen Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        gameObject.SetActive(true); // Show the dark panel
        StartCoroutine(ShowOptionsAfterDelay(4f)); // Wait 10 seconds to show options
    }

    private IEnumerator ShowOptionsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        loadGameButton.SetActive(true); // Show load game button
        restartGameButton.SetActive(true); // Show restart game button
        quitGameButton.SetActive(true); // Show quit game button
    }
}
