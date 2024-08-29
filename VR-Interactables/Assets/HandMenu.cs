using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class HandMenu : MonoBehaviour
{
    public bool GrabbingIsAllowed { get; private set; }

    public static HandMenu Instance;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadTransform();
        GrabbingIsAllowed = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().enabled;
    }

    public void SaveTransform()
    {
        PlayerPrefs.SetFloat("posX", transform.localPosition.x);
        PlayerPrefs.SetFloat("posY", transform.localPosition.y);
        PlayerPrefs.SetFloat("posZ", transform.localPosition.z);
        PlayerPrefs.SetFloat("rotX", transform.localEulerAngles.x);
        PlayerPrefs.SetFloat("rotY", transform.localEulerAngles.y);
        PlayerPrefs.SetFloat("rotZ", transform.localEulerAngles.z);
    }

    public void LoadTransform()
    {
        if (PlayerPrefs.HasKey("posX"))
        {
            float posX = PlayerPrefs.GetFloat("posX");
            float posY = PlayerPrefs.GetFloat("posY");
            float posZ = PlayerPrefs.GetFloat("posZ");
            float rotX = PlayerPrefs.GetFloat("rotX");
            float rotY = PlayerPrefs.GetFloat("rotY");
            float rotZ = PlayerPrefs.GetFloat("rotZ");

            transform.localPosition = new Vector3(posX, posY, posZ);
            transform.localEulerAngles = new Vector3(rotX, rotY, rotZ);
        }
    }

    public void DeleteSavedTransform()
    {
        if (PlayerPrefs.HasKey("posX"))
        {
            PlayerPrefs.DeleteKey("posX");
            PlayerPrefs.DeleteKey("posY");
            PlayerPrefs.DeleteKey("posZ");
            PlayerPrefs.DeleteKey("rotX");
            PlayerPrefs.DeleteKey("rotY");
            PlayerPrefs.DeleteKey("rotZ");
        }
    }

    public void SetAllowGrabbing(bool value)
    {
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().enabled = value;
        GrabbingIsAllowed = value;
    }
}
