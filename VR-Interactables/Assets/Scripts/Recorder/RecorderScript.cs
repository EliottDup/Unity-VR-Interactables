using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class RecorderScript : MonoBehaviour
{
    bool isRecording = false;

    public UnityEvent<string> StartRecord;
    public UnityEvent StopRecord;
    public bool startRec = false;
    public bool endRec = false;

    void Update()
    {
        if (startRec && !isRecording)
        {
            startRec = false;
            StartRecording();
        }
        if (endRec && isRecording)
        {
            endRec = false;
            StopRecording();
        }
    }

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

    void StartRecording()
    {
        isRecording = true;

        string path = Path.Combine(Application.dataPath, "Recordings");
        path = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Creating Record Folder: " + path);
        }

        StartRecord?.Invoke(path);
    }

    void StopRecording()
    {
        isRecording = false;
        StopRecord?.Invoke();
    }
}
