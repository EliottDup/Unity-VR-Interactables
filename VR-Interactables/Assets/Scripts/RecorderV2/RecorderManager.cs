using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RecorderManager : MonoBehaviour
{
    public static RecorderManager Instance { get; private set; }

    public List<Recorder> recorders;
    [SerializeField] Recorder activeRecorder;

    public RawImage displayTexture;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        SetActiveRecorder(activeRecorder);
    }

    public void UpdateRecorderList()
    {
        recorders = new List<Recorder>();
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Recorder");
        recorders = new List<Recorder>();
        foreach (GameObject go in gameObjects)
        {
            recorders.Add(go.GetComponent<Recorder>());
        }

        if (activeRecorder == null & recorders.Count > 0)
        {
            SetActiveRecorder(recorders[0]);
        }

    }

    public void SetActiveRecorder(Recorder recorder)
    {
        if (!recorders.Contains(recorder)){
            if (recorders.Count != 0){
                SetActiveRecorder(recorders[0]);
            }
            return;
        }
        Recorder prevRecorder = activeRecorder;
        activeRecorder = recorder;
        recorder.UpdateCamStatus();
        if (prevRecorder != null)
        {
            prevRecorder.UpdateCamStatus();
        }
        
        displayTexture.texture = recorder.targetTexture;
    }

    public Recorder GetActiveRecorder()
    {
        return activeRecorder;
    }
}
