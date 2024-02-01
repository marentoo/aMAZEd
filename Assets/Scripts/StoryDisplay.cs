using UnityEngine;
using TMPro;
using System.Collections;
using System.IO;

public class StoryDisplay : MonoBehaviour
{
    public TextMeshProUGUI storyText; // Reference to your TextMeshPro component
    public string message;

    //public Text storyText; // Assume this is a reference to a UI Text element that displays the story

    public int textNumber;

    // Backstory array
    private string[] storyLines = {
        "There was a scientist who once had a family: a father, a wife and two children. But an epidemic broke out, during which everyone got sick, and the only survivor was this scientist.",
        "After all this suffering, he went crazy and decided that he would develop a cure for death. He built a huge house, with a huge multi-level basement, and assembled a team of researchers to develop a formula that would resurrect his loved ones.",
        "They worked there for several years and seemed to have finally succeeded, as they developed a serum that actually raised the dead.",
        "Only, after a month, it became clear that something was wrong. The resurrected gradually stopped talking, eating and eventually showing any life activities at all. Despite this, however, they continued to walk and theoretically lived.",
        "Eventually, however, the first attack occurred, resulting in the death of one of the bodyguards. It turned out that the serum, however, does not work as it should, and on contact with human tissues turns them into zombies. The doctor then reached for a solution of last resort: in the Dark Web he bought an ancient book that would help him discover the secret to living and reclaiming his family.",
        "As soon as he got his hands on it, he began to study it and eventually performed a ritual that would transform the zombies, including the scientist's family, back into humans. However, something went wrong and during the ceremony he summoned a being who, amused by his attempts, gave him the chance to fulfill his goal, but not for free.",
        "The scientist was simply supposed to leave the house. However, it turned out to be not so easy at all: the creature turned the house into an infinite prison, full of past experiments and puzzles created by it.He did not manage to leave the house and from that moment it became a trap for anyone who entered it, although who knows if jednal is not some chance to get out of it...."
    };


/*    public void DisplayStory()
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
*/

    public void DisplayStory()
    {
        storyText.text = storyLines[textNumber];
        storyText.gameObject.SetActive(true); // Activate TextMeshPro
        gameObject.SetActive(true); // Activate Canvas

        StartCoroutine(HideStoryAfterDelay(10f)); // Hide after 10 seconds 
    }

    public void DisplayMessage()
    {
        string story = message;
        storyText.text = story;
        storyText.gameObject.SetActive(true); // Activate TextMeshPro
        gameObject.SetActive(true); // Activate Canvas

        StartCoroutine(HideStoryAfterDelay(3f)); // Hide after 3 seconds
    }

    private IEnumerator HideStoryAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        storyText.gameObject.SetActive(false); // Deactivate TextMeshPro
        gameObject.SetActive(false); // Deactivate Canvas
    }


}
