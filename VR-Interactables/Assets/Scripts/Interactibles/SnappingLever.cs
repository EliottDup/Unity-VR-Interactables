using System;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class SnappingLever : SnappingInteractible<float>
{

    [SerializeField] Vector2 range;
    [SerializeField] float handleHeight;

    protected override void SetSettings()
    {
        SoftJointLimit minX = new(){
            limit = Mathf.Min(range.x, range.y) + 90f
        };
        SoftJointLimit maxX = new(){
            limit = Mathf.Max(range.x, range.y) + 90f
        };
        cj.highAngularXLimit = maxX;
        cj.lowAngularXLimit = minX;
    }

    protected override void OnDrawGizmosSelected()
    {
        if (_debugDrawGizmos){
            Vector3 dirHigh = Quaternion.AngleAxis(range.y - 90, transform.right) * transform.forward;
            Vector3 dirLow = Quaternion.AngleAxis(range.x - 90, transform.right) * transform.forward;
            foreach (float p in snapPoints){
                Vector3 pos = transform.position + Vector3.Slerp(dirLow, dirHigh, p) * handleHeight;
                Gizmos.DrawCube(pos, 2 * snapDistance * Vector3.one);
            }
            Gizmos.DrawWireSphere(transform.position + transform.forward * handleHeight, 0.05f);
        }
    }

    protected override void FixedUpdate()
    {
        if (_isHeld && _hand != null){
            Vector3 targetDir = (_hand.position - handle.position).normalized;
            rb.AddForce(targetDir * pullForce, ForceMode.Force);
            rb.AddForceAtPosition(targetDir * pullForce, displayHandle.position + displayHandle.forward * handleHeight);
        }

        UpdateVisualPosition();
        UpdateValue();
    }

    protected override float CalculateDistance(float v1, float v2)
    {
        return Mathf.Abs(v1 - v2);
    }

    protected override PosRot CalculateLocalTransform(float value)
    {
        float angle = Mathf.Lerp(range.x - 90, range.y - 90, value);
        Quaternion rot = Quaternion.AngleAxis(angle, transform.right);
        
        return new PosRot(Vector3.zero, rot);
    }

    protected override float CalculateValue(Transform reference)
    {
        float angle = Vector3.SignedAngle(transform.up, reference.forward, reference.right);
        return Mathf.InverseLerp(range.x, range.y, angle);
    }

    protected override List<Vector3> GetBoundaryPoints()
    {
        return new();
    }
}
