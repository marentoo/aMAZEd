using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineMazeContorl : MonoBehaviour
{
    // Reference to your cameras
    public Camera[] enterCameras;
    public Camera[] leaveCameras;

    // Reference to your PlayableDirectors
    public PlayableDirector directorIn;
    public PlayableDirector directorOut;

    void Start()
    {
        // Initially, disable all cameras
        SetCamerasActive(enterCameras, false);
        SetCamerasActive(leaveCameras, false);

        // Subscribe to the PlayableDirector's stopped event to know when the timeline finishes playing
        directorIn.stopped += OnDirectorInStopped;
        directorOut.stopped += OnDirectorOutStopped;

        // Start the first timeline and activate the "Enter" cameras
        SetCamerasActive(enterCameras, true);
        directorIn.Play();
    }

    void OnDirectorInStopped(PlayableDirector director)
    {
        // When the first timeline stops, disable "Enter" cameras
        SetCamerasActive(enterCameras, false);
        
        // Enable the "Leave" cameras and start the second timeline
        SetCamerasActive(leaveCameras, true);
        directorOut.Play();
    }

    void OnDirectorOutStopped(PlayableDirector director)
    {
        // When the second timeline stops, disable "Leave" cameras
        SetCamerasActive(leaveCameras, false);
        
        // Here you could activate the main gameplay camera or any other post-cutscene logic
    }

    void SetCamerasActive(Camera[] cameras, bool state)
    {
        foreach (Camera cam in cameras)
        {
            cam.enabled = state;
        }
    }

    void OnDestroy()
    {
        if (directorIn != null)
        {
            directorIn.stopped -= OnDirectorInStopped;
        }
        if (directorOut != null)
        {
            directorOut.stopped -= OnDirectorOutStopped;
        }
    }
}

