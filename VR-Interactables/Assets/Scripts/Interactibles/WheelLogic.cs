using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class WheelLogic : MonoBehaviour
{
    //[SerializeField] bool lockRotation = false;

    UnityEvent<float> OnValueChanged;
    public float _pullForce = 200;

    Vector3 _initialUp;

    Rigidbody rb;
    private Dictionary<Transform, Vector3> _handPositionPairs = new Dictionary<Transform, Vector3>();

    void Start()
    {
        _initialUp = transform.up;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        foreach (Transform target in _handPositionPairs.Keys)
        {
            Vector3 localGrabPosition = _handPositionPairs[target];
            Vector3 dirToHand = transform.TransformPoint(localGrabPosition) - target.position;
            rb.AddForceAtPosition(dirToHand * _pullForce, transform.TransformPoint(localGrabPosition));
        }
    }

    float GetValue()
    {
        return Vector3.SignedAngle(_initialUp, transform.up, transform.forward);
    }

    public void OnSelectEnter(BaseInteractionEventArgs args)
    {
        Vector4 localGrabPosition = -Vector3.ProjectOnPlane(transform.InverseTransformPoint(args.interactorObject.transform.position), transform.forward);
        _handPositionPairs.Add(args.interactorObject.transform, localGrabPosition);
    }

    public void OnSelectExit(BaseInteractionEventArgs args)
    {
        _handPositionPairs.Remove(args.interactorObject.transform);
    }
}
