using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketCollisionIgnore : MonoBehaviour
{
    [SerializeField] Transform parentTransform;

    void Awake(){
        XRSocketInteractor _socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();

        _socket.selectEntered.AddListener(OnSelectEntered);
        _socket.selectExited.AddListener(OnSelectExited);
    }

    void OnSelectEntered(SelectEnterEventArgs args){
        Transform other = args.interactableObject.transform;
        SocketCollisionsIgnored(other, true);
    }

    void OnSelectExited(SelectExitEventArgs args){
        Transform other = args.interactableObject.transform;
        SocketCollisionsIgnored(other, false);
    }

    void SocketCollisionsIgnored(Transform other, bool flag){
        Collider[] ownColliders = parentTransform.GetComponentsInChildren<Collider>();
        Collider[] otherColliders = other.GetComponentsInChildren<Collider>();

        foreach (Collider cA in ownColliders){
            foreach (Collider cB in otherColliders){
                Physics.IgnoreCollision(cA, cB, flag);
            }
        }
    }
}
