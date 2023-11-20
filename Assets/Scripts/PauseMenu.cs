using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public bool isPaused;

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
        Debug.Log("ResumeGame function called."); // This line will confirm the function is invoked
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        isPaused = false;
        //EventSystem.current.SetSelectedGameObject(null);

    }

    public void QuitGame()
    {
        Application.Quit();
        EventSystem.current.SetSelectedGameObject(null);

    }
}
