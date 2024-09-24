using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SliderScript : Interactable<float>
{ 
    private enum SnappingBehavior{
        None = -1,
        Always = 0x00,
        OnRelease = 0x10,
        Distance = 0x01,
        OnReleaseDistance = 0x11
    }

    public Transform handle;
    public Transform displayHandle;

    [Tooltip("None - no snapping; Always - moving the handle makes it snap between different points; OnRelease - same as Always, but smooth while moving and snaps when released; Distance - moves smooth between points, but snaps when near a point; OnReleaseDistance - same as Distance, but only snaps when released")]
    [SerializeField] SnappingBehavior snappingBehavior = SnappingBehavior.None;
    [SerializeField] float snapDistance = 0.01f;
    [SerializeField] List<float> snapPoints = new();

    [SerializeField] float _sliderLength;
    [SerializeField] Vector3 anchorOffset;
    [SerializeField] float pullForce = 200;

    Vector3 StartPoint
    {
        get
        {
            return transform.position + anchorOffset - transform.forward * _sliderLength / 2;
        }
    }
    Vector3 EndPoint
    {
        get
        {
            return transform.position + anchorOffset + transform.forward * _sliderLength / 2;
        }
    }

    Rigidbody rb;
    ConfigurableJoint cj;

    bool _isHeld = false;
    Transform _hand;

    [SerializeField] bool _debugDrawGizmos = false;
    public UnityEvent<float> _onValueChanged;

    void Start()
    {
        rb = handle.GetComponent<Rigidbody>();
        cj = handle.GetComponent<ConfigurableJoint>();
        SetSettings();
    }

    private void OnDrawGizmosSelected()
    {
        if (_debugDrawGizmos)
        {
            Gizmos.DrawSphere(StartPoint, 0.025f);
            Gizmos.DrawSphere(EndPoint, 0.025f);
            Gizmos.DrawLine(StartPoint, EndPoint);
            foreach (float point in snapPoints){
                Gizmos.DrawCube(Vector3.Lerp(StartPoint, EndPoint, point), Vector3.one * snapDistance * 2 * _sliderLength);
            }
        }
    }

    protected void SetSettings()
    {
        cj.anchor = -anchorOffset;
        SoftJointLimit linearLimit = new SoftJointLimit();
        linearLimit.limit = _sliderLength / 2;
        cj.linearLimit = linearLimit;
    }

    public void OnSelectEntered(BaseInteractionEventArgs args)
    {
        _isHeld = true;
        _hand = args.interactorObject.transform;
        handle.position = displayHandle.position;
    }

    public void OnSelectExit(BaseInteractionEventArgs args)
    {
        UpdateVisualPosition();
        handle.position = displayHandle.position;
        _isHeld = false;
    }

    private void FixedUpdate(){
        if (_isHeld && _hand != null){
            Vector3 targetDir = StartPoint + Vector3.Project(_hand.position -StartPoint, EndPoint - StartPoint) - handle.position;
            rb.AddForce(targetDir * pullForce);
        }

        UpdateVisualPosition();
        UpdateValue();
    }

    void UpdateValue(){
        float newValue = CalculateValue(displayHandle);

        if (newValue != _value){
            _onValueChanged.Invoke(newValue);
        }
        _value = newValue;
    }

    private float CalculateValue(Transform reference){
        return Vector3.Distance(reference.position, StartPoint) / _sliderLength;
    }

    public void SetValue(float _value){
        Vector3 newPos = Vector3.Lerp(StartPoint, EndPoint, _value);
        handle.position = newPos;
        UpdateVisualPosition();
        UpdateValue();
    }

    private void UpdateVisualPosition(){

        switch (snappingBehavior){
            case SnappingBehavior.None:{
                displayHandle.position = handle.position;
                break;
            }
            case SnappingBehavior.Always:{
                Snap();
                break;
            }
            case SnappingBehavior.Distance:{
                break;
            }
            case SnappingBehavior.OnRelease:{
                if (_isHeld){
                    displayHandle.position = handle.position;
                    break;
                }
                Snap();
                break;
            }
            case SnappingBehavior.OnReleaseDistance:{
                if (_isHeld){
                    displayHandle.position = handle.position;
                    break;
                }
                SnapDistance();
                break;
            }

        }
    }

    private void Snap(){
        float tempVal = CalculateValue(handle);
        Vector3 snapPos = displayHandle.position;
        float closest = 1000f;

        foreach(float snapValue in snapPoints){
            float dist = Mathf.Abs(tempVal - snapValue);
            if (dist < closest){
                snapPos = Vector3.Lerp(StartPoint, EndPoint, snapValue);
                closest = dist;
            }
        }
        displayHandle.position = snapPos;
    }

    private void SnapDistance(){                                        //Todo: fix
        float tempVal = CalculateValue(handle);
        Vector3 snapPos = handle.position;
        float closest = 1000f;

        foreach(float snapValue in snapPoints){
            float dist = Mathf.Abs(tempVal - snapValue);
            if (dist < closest && dist < snapDistance * _sliderLength){
                snapPos = Vector3.Lerp(StartPoint, EndPoint, snapValue);
                closest = dist;
            }
        }
        displayHandle.position = snapPos;
    }

}
