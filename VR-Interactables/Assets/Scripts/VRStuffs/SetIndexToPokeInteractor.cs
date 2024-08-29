using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetIndexToPokeInteractor : MonoBehaviour
{
    [SerializeField] Transform indexTransform;
    void Start()
    {
        transform.parent.parent.GetComponentInChildren<UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor>().attachTransform = indexTransform;
    }
}
