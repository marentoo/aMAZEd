using UnityEngine;
using UnityEngine.UI;
using TMPro; // Required for TextMeshPro elements

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI keyCountText; // Assign this in the inspector

    public void UpdateKeyDisplay(int keyCount)
    {
        keyCountText.text = "Keys: " + keyCount.ToString(); // Update the TextMeshPro component
    }
}

