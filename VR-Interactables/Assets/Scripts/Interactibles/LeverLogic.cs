using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class LeverLogic : MonoBehaviour
{

    public float startAngle = -45, endAngle = 45;
    public float minValue = -1, maxValue = 1;
    [SerializeField]
    float pullDistance = 0.2f;

    [SerializeField] float pullForce = 200;

    [Header("Events")]
    public UnityEvent<float> OnValueChanged;

    bool _isHeld = false;
    Transform _target;

    float currentAngle;

    Vector3 rotationAxis;

    Rigidbody rb;
    HingeJoint hj;

    Vector3 InitialForward;


    void Start()
    {
        InitialForward = transform.forward;
        rb = GetComponent<Rigidbody>();
        hj = GetComponent<HingeJoint>();
        rotationAxis = transform.right;
    }

    void FixedUpdate()
    {
        if (_isHeld)
        {
            Vector3 dir2Hand = _target.position - transform.position;
            dir2Hand = Vector3.ProjectOnPlane(dir2Hand, rotationAxis);
            rb.AddForceAtPosition(dir2Hand * pullForce, transform.position + transform.forward * pullDistance);
        }
        float newAngle = GetAngle();
        if (newAngle != currentAngle)
        {
            OnValueChanged.Invoke(GetNormalizedLeverPosition());
            currentAngle = newAngle;
            //Debug.Log("test");
        }
        //Debug.Log("angle: " + currentAngle + ", value: " + GetNormalizedLeverPosition());
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + transform.forward * pullDistance, 0.05f);
    }

    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        Debug.Log(args.interactorObject.transform.parent.name + " has Selected! :3");
        _isHeld = true;
        _target = args.interactorObject.transform.parent;
    }

    public void OnSelectExit(SelectExitEventArgs args)
    {
        _isHeld = false;
        Debug.Log(GetNormalizedLeverPosition());
    }

    float GetAngle()
    {
        return Vector3.SignedAngle(InitialForward, transform.forward, rotationAxis);
    }

    float GetNormalizedLeverPosition()
    {
        float angle = GetAngle();
        // Map the clamped angle to the range -1 to 1
        float t = Mathf.InverseLerp(startAngle, endAngle, angle);
        return Mathf.Lerp(minValue, maxValue, t);
    }

    public float GetLeverValue()
    {
        return GetNormalizedLeverPosition();
    }
}
