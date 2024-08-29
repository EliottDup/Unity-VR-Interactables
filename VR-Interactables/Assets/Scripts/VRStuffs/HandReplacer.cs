using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandReplacer : MonoBehaviour
{
    [SerializeField] GameObject handReplacement;
    [SerializeField] GameObject controller;
    public void OnSelectEnter(BaseInteractionEventArgs args)
    {
        handReplacement.SetActive(true);
        controller.SetActive(false);
    }

    public void OnSelectExit(BaseInteractionEventArgs args)
    {
        handReplacement.SetActive(false);
        controller.SetActive(true);
    }
}
