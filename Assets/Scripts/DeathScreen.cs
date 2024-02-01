using UnityEngine;
using System.Collections;
using TMPro; // If you're using TextMeshPro

public class DeathScreen : MonoBehaviour
{
    public GameObject loadGameButton;
    public GameObject restartGameButton;
    public GameObject quitGameButton;
    //public AudioSource deathAudioSource; // Uncomment and assign this if you want to play audio

    public static DeathScreen Instance { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
         if (Input.GetMouseButtonDown(1)) //left click = 0; right clisk = 1
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

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
        //deathAudioSource = GetComponent<AudioSource>(); // Make sure this GameObject has an AudioSource component if you're using this line
    }

    public void ShowDeathScreen()
    {
        gameObject.SetActive(true); // Show the death screen
        StartCoroutine(ShowOptionsAfterDelay(10f)); // Check if the delay is too long for UX
    }
    
    public void HideDeathScreen()
    {
        gameObject.SetActive(false); 
        //loadGameButton.SetActive(false);
        //restartGameButton.SetActive(false);
        //quitGameButton.SetActive(false);
    }

    private IEnumerator ShowOptionsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Ensure that these buttons are not behind any other UI elements and are set to be interactable
        loadGameButton.SetActive(true);
        restartGameButton.SetActive(true);
        quitGameButton.SetActive(true);

        // Make sure the AudioSource has an audio clip assigned if this is uncommented
        //if (deathAudioSource != null)
        //{
        //    deathAudioSource.Play(); // Play the death screen sound
        //}
    }
}
