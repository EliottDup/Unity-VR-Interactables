using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandReplacer : MonoBehaviour
{
    [SerializeField] GameObject handReplacement;
    public void OnSelectEnter(BaseInteractionEventArgs args)
    {
        handReplacement.SetActive(true);
    }

    public void OnSelectExit(BaseInteractionEventArgs args)
    {
        handReplacement.SetActive(false);
    }
}
