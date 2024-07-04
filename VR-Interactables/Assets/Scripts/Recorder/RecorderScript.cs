using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class RecorderScript : MonoBehaviour
{
    public bool isRecording { get; private set; }

    public UnityEvent<string> StartRecord;
    public UnityEvent StopRecord;
    public Camera recorderCamera;

    public void OnActivate(ActivateEventArgs args)
    {
        if (!isRecording)
        {
            StartRecording();
        }
        else
        {
            StopRecording();
        }
    }

    public void StartRecording()
    {
        isRecording = true;

        string path = Path.Combine(Application.persistentDataPath, "Recordings");
        path = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Creating Record Folder: " + path);
        }

        StartRecord?.Invoke(path);
    }

    public void StopRecording()
    {
        isRecording = false;
        StopRecord?.Invoke();
    }
}
