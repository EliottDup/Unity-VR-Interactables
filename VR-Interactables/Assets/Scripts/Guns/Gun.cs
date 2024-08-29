using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Gun : MonoBehaviour
{
    [SerializeField] string gunName;

    [Tooltip("The amount of bullets shot when pressing the trigger")]
    [SerializeField] int burstSize;

    [Tooltip("The max amount of bursts per second")]
    [SerializeField] int timeBetweenBursts;
    [Tooltip("The time between shots inside a burst")]
    [SerializeField] string timeBetweenShots;
    [SerializeField] UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor MagasineHolder;
    void StartBurst(){
    }
}
