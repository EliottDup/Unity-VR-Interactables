using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Snappable : MonoBehaviour
{
    public List<string> tags = new List<string>();

    public SnapZone currentSnapZone;
    bool isSnapped = false;

    public void OnSelectExit(BaseInteractionEventArgs args)
    {
        if (currentSnapZone != null)
        {
            isSnapped = currentSnapZone.TryAttachObject(transform);
        }
        else isSnapped = false;
    }

    public void OnSelectEnter(BaseInteractionEventArgs args)
    {
        if (isSnapped)
        {
            currentSnapZone.DetachObject();
            isSnapped = false;
        }
    }
}
