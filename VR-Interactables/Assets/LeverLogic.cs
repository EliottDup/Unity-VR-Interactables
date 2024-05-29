using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LeverLogic : MonoBehaviour
{

    public float minAngle = -45, maxAngle = 45;
    public float minValue = -1, maxValue = 1;

    public Vector3 rotationAxis = Vector3.right;

    [Header("Tmp")]
    public bool _isHeld = false;
    public Transform _target;

    Quaternion initialRotation;
    Vector3 initialForward;

    float _leverValue;

    void Start()
    {
        initialRotation = transform.localRotation;
        initialForward = transform.forward;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, rotationAxis, Color.red);
        Debug.DrawRay(transform.position, initialForward, Color.blue);
        Debug.DrawRay(transform.position, transform.up, Color.green);

        if (_isHeld)
        {
            // Calculate the direction from the lever to the hand
            Vector3 directionToHand = _target.position - transform.position;

            // Project this direction onto the rotation axis plane
            Vector3 projectedDirection = Vector3.ProjectOnPlane(directionToHand, rotationAxis);

            // Calculate the signed angle between the initial forward vector and the projected direction
            float angle = Vector3.SignedAngle(initialForward, projectedDirection, rotationAxis);

            // Clamp the angle within the min and max range
            float clampedAngle = Mathf.Clamp(angle, minAngle, maxAngle);

            Quaternion targetRot = Quaternion.LookRotation(projectedDirection, rotationAxis);
            // Apply the clamped rotation
            transform.rotation = Quaternion.RotateTowards(initialRotation, targetRot, Mathf.Abs(clampedAngle));

            float _leverValue = GetNormalizedLeverPosition(clampedAngle);

            Debug.Log("Angle: " + angle + ", clamped Angle: " + clampedAngle + ", value: " + _leverValue);

            //Debug.Log("Lever Position: " + normalizedLeverPosition);
        }
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
        Debug.Log(_leverValue);
    }

    float GetNormalizedLeverPosition(float clampedAngle)
    {
        // Map the clamped angle to the range -1 to 1
        float t = Mathf.InverseLerp(minAngle, maxAngle, clampedAngle);
        return Mathf.Lerp(minValue, maxValue, t);
    }

    public float GetLeverValue()
    {
        return _leverValue;
    }
}
