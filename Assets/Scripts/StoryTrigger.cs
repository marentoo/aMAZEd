using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    private StoryDisplay storyDisplay;
    public int number;

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
                storyDisplay.textNumber = number;
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