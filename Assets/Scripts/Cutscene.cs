using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    // Start is called before the first frame update
    public string sceneName;
    public void Trigger()
    {
        SceneManager.LoadScene(sceneName);
    }
}