using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    private StoryDisplay storyDisplay;
    public string storyFileName;

    void Start()
    {
        storyDisplay = FindObjectOfType<StoryDisplay>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (storyDisplay != null)
            {
                storyDisplay.fileName = storyFileName;
                storyDisplay.DisplayStory();

                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("StoryDisplay not found in the scene.");
            }
        }
    }

}