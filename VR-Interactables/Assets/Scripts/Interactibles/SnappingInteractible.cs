using System;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class SnappingInteractible<T> : Interactable<T>
{

    protected struct PosRot{
        public PosRot(Vector3 pos, Quaternion rot){
            this.position = pos;
            this.rotation = rot;
        }

        public Vector3 position;
        public Quaternion rotation;
    }

    protected enum SnappingBehavior{
        None = -1,
        Always = 0x00,
        OnRelease = 0x10,
        Distance = 0x01,
        OnReleaseDistance = 0x11
    }

    [SerializeField] protected T startingValue;

    public Transform handle;
    public Transform displayHandle;

    [Tooltip("None - no snapping; Always - moving the handle makes it snap between different points; OnRelease - same as Always, but smooth while moving and snaps when released; Distance - moves smooth between points, but snaps when near a point; OnReleaseDistance - same as Distance, but only snaps when released")]
    [SerializeField] protected SnappingBehavior snappingBehavior = SnappingBehavior.None;
    [SerializeField] protected float snapDistance = 0.01f;
    [SerializeField] protected List<T> snapPoints = new();

    protected bool _isHeld = false;
    protected Transform _hand;
    [SerializeField] protected Vector3 anchorOffset;

    protected Vector3 LocalAnchor {
        get { return transform.TransformDirection(anchorOffset); }
    }

    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected ConfigurableJoint cj;
    [SerializeField] protected bool _debugDrawGizmos = false;
    [SerializeField] protected float pullForce = 200;

    private void Start()
    {
        SetSettings();
        SetValue(startingValue);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (_debugDrawGizmos)
        {
            List<Vector3> corners = new List<Vector3>();
            GetBoundaryPoints().ForEach((Vector3 p) => {
                corners.Add(transform.TransformPoint(p));
                });
            
            Gizmos.DrawLineStrip(corners.ToArray(), true);

            foreach (Vector3 corner in corners){
                Gizmos.DrawSphere(corner, 0.025f);
            }

            foreach (T point in snapPoints){
                Gizmos.DrawCube(transform.TransformPoint(CalculateLocalTransform(point).position), 2 * snapDistance * Vector3.one);
            }
        }
    }

    protected virtual void FixedUpdate(){
        if (_isHeld && _hand != null){
            Vector3 targetDir = (_hand.position - handle.position).normalized;
            rb.AddForce(targetDir * pullForce, ForceMode.Force);
        }

        UpdateVisualPosition();
        UpdateValue();
    }

    /// <summary>
    /// called on Start to set the settings of the configurable joint
    /// SHOULD BE EXPANDED
    /// </summary>
    protected virtual void SetSettings()
    {
        cj.anchor = LocalAnchor;
    }


    /// <summary>
    /// Calculate new value and invoke onvaluechanged if neccecary
    /// </summary>
    protected void UpdateValue(){
        T newValue = CalculateValue(displayHandle);

        if (!newValue.Equals(_value)){
            OnValueChanged.Invoke(newValue);
        }
        _value = newValue;
    }

    /// <summary>
    /// Called to grab handle
    /// </summary>
    /// <param name="args"></param>
    public void OnSelectEntered(BaseInteractionEventArgs args)
    {
        _isHeld = true;
        _hand = args.interactorObject.transform;
        handle.SetPositionAndRotation(displayHandle.position, displayHandle.rotation);
    }
    
    /// <summary>
    /// Called to let go of handle
    /// </summary>
    /// <param name="args"></param>
    public void OnSelectExit(BaseInteractionEventArgs args)
    {
        UpdateVisualPosition();
        _isHeld = false;
        handle.SetPositionAndRotation(displayHandle.position, displayHandle.rotation);
    }

    /// <summary>
    /// Depending on snappingBehavior, either snaps or does not snap
    /// </summary>
    protected void UpdateVisualPosition(){

        switch (snappingBehavior){
            case SnappingBehavior.None:{
                displayHandle.SetPositionAndRotation(handle.position, handle.rotation);
                break;
            }
            case SnappingBehavior.Always:{
                SnapImmediate();
                break;
            }
            case SnappingBehavior.Distance:{
                SnapDistance();
                break;
            }
            case SnappingBehavior.OnRelease:{
                if (_isHeld){
                    displayHandle.SetPositionAndRotation(handle.position, handle.rotation);
                    break;
                }
                SnapImmediate();
                break;
            }
            case SnappingBehavior.OnReleaseDistance:{
                if (_isHeld){
                    displayHandle.SetPositionAndRotation(handle.position, handle.rotation);
                    break;
                }
                SnapDistance();
                break;
            }
        }
    }

    /// <summary>
    /// Snaps to the closest snap position
    /// </summary>
    private void SnapImmediate(){
        T tempVal = CalculateValue(handle);
        T snapTransform = CalculateValue(displayHandle);
        float closest = 1000f;

        foreach(T snapValue in snapPoints){
            float dist = CalculateDistance(tempVal, snapValue);
            if (dist < closest){
                snapTransform = snapValue;
                closest = dist;
            }
        }
        Snap(snapTransform);
    }

    /// <summary>
    /// Snaps to a snappoint that is within snapping range, otherwise does nothing
    /// </summary>
    private void SnapDistance(){
        T tempVal = CalculateValue(handle);
        foreach(T snapValue in snapPoints){
            if (CalculateDistance(tempVal, snapValue) < snapDistance){
                Snap(snapValue);
                return;
            }
        }
        Snap(tempVal);
    }
    
    /// <summary>
    /// Snaps the displayHandle transform to the specified value
    /// </summary>
    /// <param name="snapTrans">the value to snap to</param>
    private void Snap(T snapTrans){
        PosRot tr = CalculateLocalTransform(snapTrans);
        displayHandle.SetLocalPositionAndRotation(tr.position, tr.rotation);
    }

    /// <summary>
    /// Calculates a value from a Transform
    /// </summary>
    /// <param name="reference">The Transform to calculate from</param>
    /// <returns>The value of that Transform</returns>
    protected abstract T CalculateValue(Transform reference);

    /// <summary>
    /// Calculates an absolute distance between v1 and v2
    /// </summary>
    /// <param name="v1">The first value</param>
    /// <param name="v2">The second value</param>
    /// <returns></returns>
    protected abstract float CalculateDistance(T v1, T v2);

    //returns a list with all corners/endpoints to move to, return empty list if unused
    /// <summary>
    /// this function creates a list of corners or endpoints if applicable
    /// </summary>
    /// <returns>a list of corners/endpoints</returns>
    protected abstract List<Vector3> GetBoundaryPoints();

    /// <summary>
    /// Calculates a transform for a specific value
    /// </summary>
    /// <param name="value">The value for which to calculate a Transform</param>
    /// <returns>The transform calculated for the value</returns>
    protected abstract PosRot CalculateLocalTransform(T value);

    /// <summary>
    /// Places the handle in the position of the value.
    /// </summary>
    /// <param name="value">the value at which to place the handle</param>
    /// <exception cref="NotImplementedException"></exception>
    protected void SetValue(T value)
    {
        PosRot posRot = CalculateLocalTransform(value);
        handle.SetLocalPositionAndRotation(posRot.position, posRot.rotation);
    }
}
