using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; private set; }
    public TextMeshProUGUI enterText; // Assign this in the inspector

    private void Awake()
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

    public void ShowText()
    {
        enterText.gameObject.SetActive(true); // Show the text
    }
    
    public void HideText()
    {
        enterText.gameObject.SetActive(false); // Hide the text
    }

    public void SetMessage(string message)
    {
        enterText.text = message; // Dynamically set the text
    }
}

