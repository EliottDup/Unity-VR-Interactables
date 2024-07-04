using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RecorderMenu : MenuPage
{
    public Recorder activeRecorder;

    public GameObject recorderMenuTemplate;

    public Transform menuParent;
    public GameObject viewFinderMenu;

    void Awake()
    {
        /*
        activeRecorder = GameObject.FindWithTag("Recorder");
        if (activeRecorder == null) return;
        enableGravity.isOn = activeRecorder.GetComponent<Rigidbody>().useGravity;
        enableGravity.onValueChanged.AddListener((bool value) => { activeRecorder.GetComponent<Rigidbody>().useGravity = value; });
        showViewFinder.onClick.AddListener(() => { this.RequestLoadNewMenu(viewFinderMenu); });

        tPHere.onClick.AddListener(() => { activeRecorder.transform.position = transform.position + Vector3.up * 0.2f; });
        startRecording.onClick.AddListener(activeRecorder.GetComponent<RecorderScript>().StartRecording);
        stopRecording.onClick.AddListener(activeRecorder.GetComponent<RecorderScript>().StopRecording);*/
    }

    void Start()
    {
        UpdateRecorderList();
    }

    void UpdateRecorderList()
    {
        foreach (Transform child in menuParent)
        {
            Destroy(child.gameObject);
        }
        RecorderManager.Instance.UpdateRecorderList();
        foreach (Recorder rec in RecorderManager.Instance.recorders)
        {
            GameObject go = Instantiate(recorderMenuTemplate, menuParent);
            RecorderUITemplate elem = go.GetComponent<RecorderUITemplate>();
            elem.recorder = rec;
            elem.viewFinder.texture = rec.targetTexture;
            elem.recorderName.text = rec.name;
            elem.setActiveButton.onClick.AddListener(() => { RecorderManager.Instance.SetActiveRecorder(rec); UpdateBackgroundColor(); });
            elem.overrideEnabledToggle.onValueChanged.AddListener((bool value) => { rec.SetOverride(value); UpdateBackgroundColor(); });
            elem.overrideEnabledToggle.isOn = rec.GetOverride();
            elem.showViewFinderButton.onClick.AddListener(() => { this.RequestLoadNewMenu(viewFinderMenu); manager.menuStack.Peek().GetComponent<ViewFinderMenu>().image.texture = rec.targetTexture; });                                            //TODO: implement this
            elem.gravityToggle.onValueChanged.AddListener((bool value) => { rec.GetComponent<Rigidbody>().useGravity = value; });
            elem.gravityToggle.isOn = rec.GetComponent<Rigidbody>().useGravity;
            elem.hideViewFinderToggle.onValueChanged.AddListener((bool value) => { rec.SetViewFinderVisibility(value); });
            elem.hideViewFinderToggle.isOn = rec.GetViewFinderVisibility();
            elem.teleportHereButton.onClick.AddListener(() => rec.transform.position = transform.position + Vector3.up * .2f);
            elem.backgroundImg.color = rec.GetIsEnabled() ? elem.activeCol : (rec.GetOverride()) ? elem.overrideCol : elem.inactiveCol;

        }
    }

    void UpdateBackgroundColor()
    {
        foreach (Transform child in menuParent)
        {
            RecorderUITemplate elem = child.GetComponent<RecorderUITemplate>();
            elem.backgroundImg.color = elem.recorder.GetIsEnabled() ? elem.activeCol : (elem.recorder.GetOverride()) ? elem.overrideCol : elem.inactiveCol;
        }
    }


}
