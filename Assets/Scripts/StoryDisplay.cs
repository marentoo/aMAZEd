using UnityEngine;
using TMPro;
using System.Collections;
using System.IO;

public class StoryDisplay : MonoBehaviour
{
    public TextMeshProUGUI storyText; // Reference to your TextMeshPro component
    public string fileName;

    public void DisplayStory()
    {

        string path = Application.dataPath + "/Backstory/" + fileName;
        //private static string path = Application.dataPath + "/Saves/savefile.json";

        if (File.Exists(path))
        {
            string story = File.ReadAllText(path);
            storyText.text = story;
            storyText.gameObject.SetActive(true); // Activate TextMeshPro
            gameObject.SetActive(true); // Activate Canvas

            StartCoroutine(HideStoryAfterDelay(10f)); // Hide after 15 seconds
        }
        else
        {
            Debug.Log("File not found: " + path);
        }
    }

    private IEnumerator HideStoryAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        storyText.gameObject.SetActive(false); // Deactivate TextMeshPro
        gameObject.SetActive(false); // Deactivate Canvas
    }


}
