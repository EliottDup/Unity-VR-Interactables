using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class Gun : MonoBehaviour
{
    [SerializeField] private string gunName;
    [SerializeField] Transform origin;

    [Tooltip("The amount of bullets shot when pressing the trigger")]
    [SerializeField] private int burstSize;

    [Tooltip("The max amount of bursts per second")]
    [SerializeField] private int timeBetweenBursts;
    [Tooltip("The time between shots inside a burst")]
    [SerializeField] float timeBetweenShots;
    [SerializeField] private XRSocketInteractor magazineSocket;

    [SerializeField] private IMagazine loadedMag;

    private void Awake(){
        magazineSocket.selectEntered.AddListener(OnMagazineLoaded);
        magazineSocket.selectExited.AddListener(OnMagazineUnloaded);
    }

    private void OnMagazineLoaded(SelectEnterEventArgs args){
        Debug.Log("loaded mag: " + args.interactableObject.transform.name);
        args.interactableObject.transform.TryGetComponent<IMagazine>(out loadedMag);
    }

    private void OnMagazineUnloaded(SelectExitEventArgs args){
        loadedMag = null;
    }

    public void Shoot(ActivateEventArgs args){
        if (loadedMag == null){
            return; //Todo: maybe add support for "one in the chamber"
        }
        loadedMag.Shoot(origin.position, origin.forward);
    }

}
