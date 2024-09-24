    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketTagCheck : XRSocketInteractor
{
    public string targetTag = String.Empty;

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && MatchUsingTag((XRBaseInteractable)interactable);
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && MatchUsingTag((XRBaseInteractable)interactable);
    }

    private bool MatchUsingTag(XRBaseInteractable interactable){
        return interactable.CompareTag(targetTag);
    }
}
