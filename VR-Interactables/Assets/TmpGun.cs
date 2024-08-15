using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TmpGun : MonoBehaviour
{
    [SerializeField] float recoilForce = 10f;
    [SerializeField] float shootForce = 10f;
    [SerializeField] Transform magasinePosition;
    [SerializeField] Transform barrelEnd;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        XRGrabInteractable interactable = GetComponent<XRGrabInteractable>();
        interactable.activated.AddListener((ActivateEventArgs args) => {Shoot();});   
    }

    void Update(){
        Debug.DrawRay(barrelEnd.position, barrelEnd.forward, Color.white);
    }

    private void Shoot(){
        RaycastHit hit;
        if (Physics.Raycast(barrelEnd.position, barrelEnd.forward, out hit)){
            if (hit.transform.TryGetComponent<Rigidbody>(out var hitrb))
            {
                hitrb.AddForceAtPosition(barrelEnd.forward * shootForce, hit.point);
            }
        }
        Debug.Log("pew");
        rb.AddForce(barrelEnd.forward * -recoilForce);
    }
}
