using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;

public class SplashScreenManager : MonoBehaviour
{
    public new string name;
    private Animator animator; // Reference to the Animator component
    private float delayTimer = 1.5f;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSceneWithDelay();
    }

    public void ChangeSceneWithDelay()
    {
        Invoke("LoadScene", 4f); // Calls the LoadScene method after 1 second
        //delayTimer = 1f;
    }

    private void LoadScene()
    {
        delayTimer -= Time.deltaTime;
        animator.SetTrigger("Fade");
        if (delayTimer <= 0f)
        {
            SceneManager.LoadScene(name);
        }
    }
}

