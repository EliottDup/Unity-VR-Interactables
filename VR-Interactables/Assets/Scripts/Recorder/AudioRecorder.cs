using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class AudioRecorder : MonoBehaviour
{
    public UnityEvent<string> OnFinishAudioRecording;

    private AudioClip audioClip;
    private string filePath;

    float startTime;


    public void StartRecording(string path)
    {
        filePath = Path.Combine(path, "audio.wav");
        audioClip = Microphone.Start(null, true, 300, 44100);
        startTime = Time.time;
    }

    public void StopRecording()
    {
        Microphone.End(null);
        SaveAudioClip(audioClip, filePath);
        OnFinishAudioRecording?.Invoke(filePath);
    }

    private void SaveAudioClip(AudioClip clip, string path)
    {
        float totalTime = Time.time - startTime;
        int sampleCount = Mathf.FloorToInt(totalTime * 44100);
        sampleCount = Mathf.Min(sampleCount, 300 * 44100);
        int length;
        byte[] wavFile = WavUtility.FromAudioClip(clip, sampleCount, out length);
        WavUtility.Save(path, wavFile);
    }
}
