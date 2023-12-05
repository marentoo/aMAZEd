using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenManager : MonoBehaviour
{
    public float splashScreenDuration = 3f; // Duration of the splash screen in seconds
    public string mainSceneName = "MainGame"; // The name of the main game scene

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(splashScreenDuration); // Wait for the duration of the splash screen
        SceneManager.LoadScene(mainSceneName); // Load the main game scene
    }
}
