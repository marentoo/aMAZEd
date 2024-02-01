using UnityEngine;

public class Footsteps : MonoBehaviour
{
    AudioSource footstepSound;

    void Start()
    {
        footstepSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) && !footstepSound.isPlaying) // Example condition
        {
            footstepSound.Play();
        }
        else if (Input.GetKey(KeyCode.S) && !footstepSound.isPlaying)
        {
            footstepSound.Play();
        }
        else if (Input.GetKey(KeyCode.A) && !footstepSound.isPlaying)
        {
            footstepSound.Play();
        }
        else if (Input.GetKey(KeyCode.D) && !footstepSound.isPlaying)
        {
            footstepSound.Play();
        }
    }
}
