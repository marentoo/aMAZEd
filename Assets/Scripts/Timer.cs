using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    private float elapsedTime = 0f;
    private bool isTimerRunning = false;

    // Update is called once per frame
    void Update() {
        if (isTimerRunning) {
            elapsedTime += Time.deltaTime;
        }
    }

    public void StartTimer() {
        isTimerRunning = true;
    }

    public void StopTimer() {
        isTimerRunning = false;
    }

    public float GetElapsedTime() {
        return elapsedTime;
    }

    public void ResetTimer() {
        elapsedTime = 0f;
    }
}
