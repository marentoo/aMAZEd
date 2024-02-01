using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;

public class TutorialMenu : MonoBehaviour
{
    public new string name;
    private Animator animator; // Reference to the Animator component
    private float delayTimer = 1.5f;
    private bool trigger = false;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate(){
        if(trigger == true){  
            delayTimer -= Time.deltaTime;
            if (delayTimer <= 0f)
            {
                SceneManager.LoadScene(name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSceneWithDelay();
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

    public void ChangeSceneWithDelay()
    {
        if (Input.GetKeyDown(KeyCode.Return)){
            Invoke("LoadScene", 1f); // Calls the LoadScene method after 1 second
        }
        
        //delayTimer = 1f;
    }

    private void LoadScene()
    {
        //delayTimer -= Time.deltaTime;
        trigger = true;
        animator.SetTrigger("Fade");
        
    }
}
