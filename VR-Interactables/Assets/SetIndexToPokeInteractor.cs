using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetIndexToPokeInteractor : MonoBehaviour
{
    [SerializeField] Transform indexTransform;
    void Start()
    {
        transform.parent.parent.GetComponentInChildren<XRPokeInteractor>().attachTransform = indexTransform;
    }
}
