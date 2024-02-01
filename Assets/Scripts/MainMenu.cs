using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuUI;
    public AudioSource backgroundAudioSource; // Assign this in the inspector
    public GameObject continueButton;
    public GameObject fadeImage;

    private SaveLevel saveLevelInstance;

    private void Awake()
    {
        saveLevelInstance = gameObject.AddComponent<SaveLevel>();
        PlayBackgroundMusic();
        UpdateButtonStates();
        
        Invoke("FadeIn", 1f);
        
    }

    private void FadeIn(){
        fadeImage.SetActive(false);
    }

    private void PlayBackgroundMusic()
    {
        if (backgroundAudioSource != null && !backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Play();
        }
    }

    public void LoadNewGame()
    {
        saveLevelInstance.Delete();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        saveLevelInstance.SaveLvl(1);
        saveLevelInstance.SetEndLevelNumber();
        SceneManager.LoadScene("DockThing");
        // Optional: Stop music if it shouldn't play in the next scene
    }

    public void ContinueGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("MazeScene");
        // Optional: Stop music if it shouldn't play in the next scene
    }

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

    public void QuitGame()
    {
        Debug.Log("Quit function called.");
        Application.Quit();
        EventSystem.current.SetSelectedGameObject(null);
        // Optional: Stop music here if needed for any reason
    }

    private void UpdateButtonStates()
    {
        int lvl = saveLevelInstance.LoadLvl();
        Debug.Log($"{lvl}");
        if (lvl != 0)
        {
            continueButton.SetActive(true); // Show the continue button
        }
        else
        {
            continueButton.SetActive(false); // Hide the continue button
        }
    }
}
