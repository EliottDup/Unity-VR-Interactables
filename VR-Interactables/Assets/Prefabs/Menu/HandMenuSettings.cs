using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class HandMenuSettings : MenuPage
{
    public Toggle AllowMovement;
    public Button SaveTransform;
    public Button LoadTransform;
    public Button DeleteSavedTransform;

    void Awake()
    {
        HandMenu instance = HandMenu.Instance;

        AllowMovement.isOn = instance.GrabbingIsAllowed;
        AllowMovement.onValueChanged.AddListener(instance.SetAllowGrabbing);

        SaveTransform.onClick.AddListener(instance.SaveTransform);
        LoadTransform.onClick.AddListener(instance.LoadTransform);
        DeleteSavedTransform.onClick.AddListener(instance.DeleteSavedTransform);
    }
}
