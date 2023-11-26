using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public bool isPaused;
    public GameManager gameManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Debug.Log("ResumeGame function called.");
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        isPaused = false;
        //EventSystem.current.SetSelectedGameObject(null);

    }
    public void SaveGame()
    {
        Debug.Log("Save function called.");

        if (gameManager != null)
        {
            gameManager.SaveGame();
            ResumeGame();

        }
        else
        {
            Debug.LogError("GameManager is not set in the PauseMenu");
        }
    }
    public void LoadGame()
    {
        Debug.Log("Load function called.");

        if (gameManager != null)
        {
            gameManager.LoadGame();
            ResumeGame();
        }
        else
        {
            Debug.LogError("GameManager is not set in the PauseMenu");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit function called.");

        Application.Quit();
        EventSystem.current.SetSelectedGameObject(null);

    }
}
