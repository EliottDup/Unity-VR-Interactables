using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recorder : MonoBehaviour
{
    [SerializeField] Camera cam;
    public RenderTexture targetTexture;
    bool cameraOverrideEnabled = false;

    [SerializeField] List<RawImage> viewfinders = new List<RawImage>();

    bool hideViewFinder = false;

    [SerializeField]
    Material offMaterial, activeMaterial, overrideMaterial;
    [SerializeField] GameObject statusIndicator;

    public bool GetIsEnabled()
    {
        return RecorderManager.Instance.GetActiveRecorder() == this;
    }

    public void SetOverride(bool value)
    {
        cameraOverrideEnabled = value;
        UpdateCamStatus();
    }

    public bool GetOverride()
    {
        return cameraOverrideEnabled;
    }
    public bool GetViewFinderVisibility()
    {
        return hideViewFinder;
    }

    public void UpdateCamStatus()
    {
        cam.enabled = cameraOverrideEnabled || GetIsEnabled();
        UpdateViewFinders();
        statusIndicator.GetComponent<MeshRenderer>().material = GetIsEnabled() ? activeMaterial : (cameraOverrideEnabled ? overrideMaterial : offMaterial);
    }

    void Start()
    {
        targetTexture = new RenderTexture(1920, 1080, 24);
        cam.targetTexture = targetTexture;
        UpdateCamStatus();
    }

    public void SetViewFinderVisibility(bool value)
    {
        hideViewFinder = value;
        UpdateViewFinders();
    }

    void UpdateViewFinders()
    {
        foreach (RawImage viewfinder in viewfinders)
        {
            viewfinder.gameObject.SetActive(!hideViewFinder && (GetIsEnabled() || cameraOverrideEnabled));
            viewfinder.texture = targetTexture;
        }
    }
}
