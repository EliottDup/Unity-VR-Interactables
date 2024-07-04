using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecorderUITemplate : MonoBehaviour
{
    [Header("Linked Recorder")]
    public Recorder recorder;

    [Header("ViewFinder")]
    public RawImage viewFinder;

    [Header("RecorderName")]
    public TextMeshProUGUI recorderName;

    [Header("Buttons and Toggles")]
    public Button setActiveButton;
    public Toggle overrideEnabledToggle;
    public Button showViewFinderButton;
    public Toggle gravityToggle;
    public Button teleportHereButton;
    public Toggle hideViewFinderToggle;

    [Header("Background Shenanigans")]
    public Image backgroundImg;
    public Color inactiveCol;
    public Color activeCol;
    public Color overrideCol;
}
