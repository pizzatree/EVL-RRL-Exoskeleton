// Handles the motions playback
// Goes to frames based on elapsed time, not elapsed frames
// Changing fps while a motion is running will cause a jump

using System;
using UnityEngine;

[RequireComponent(typeof(ControlDataHolder))]
public class DataPlayer : MonoBehaviour
{
    public event Action<int> OnUpdatedFrame;

    [Header("Settings")] 
    [SerializeField] private float frameRate = 120f;
    [SerializeField] private bool loop = true;

    private bool  isPlaying;
    private int   currentFrame;
    private float internalTime;

    private int frameCount;

    private void OnEnable()
    {
        isPlaying    = false;
        currentFrame = 1;
        internalTime = 0f;

        frameCount                                  =  -1;
        GetComponent<ControlDataHolder>().OnSuccessfulLoad += UpdateFrameCount;
    }

    private void OnDisable()
        => GetComponent<ControlDataHolder>().OnSuccessfulLoad -= UpdateFrameCount;

    private void UpdateFrameCount(int num)
        => frameCount = num;

    private void Update()
    {
        if(!isPlaying)
            return;

        internalTime += Time.deltaTime;
        currentFrame =  (int)(internalTime * frameRate);

        if(loop && frameCount > 0 && currentFrame >= frameCount)
            internalTime = 0;

        OnUpdatedFrame?.Invoke(currentFrame);
    }

    public void SetLooping(bool active)
        => loop = active;

    public void SetFramerate(string input)
    {
        if(int.TryParse(input, out var fps))
            frameRate = fps;
        else
            Debug.LogWarning($"FPS on {name} is not being sent an integer.");
    }

    public void SetCurrentFrame(int frameNumber)
    {
        currentFrame = frameNumber;
        internalTime = frameNumber / frameRate;
        OnUpdatedFrame?.Invoke(currentFrame);
    }

    // Called via inspector Unity Events
    public void SetCurrentFrame(string frameNumber) => SetCurrentFrame(int.Parse(frameNumber));
    public void SetCurrentFrame(float  frameNumber) => SetCurrentFrame((int)frameNumber);

    public void TogglePlay()        => SetPlay(!isPlaying);
    public void SetPlay(bool value) => isPlaying = value;
}