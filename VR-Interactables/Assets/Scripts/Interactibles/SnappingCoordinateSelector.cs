using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SnappingCoordinateSelector : SnappingInteractible<Vector2>
{

    [SerializeField] Vector2 areasize;

    protected override void SetSettings()
    {}

    protected override void FixedUpdate()
    {
        Vector3 pos = handle.localPosition;
        Vector3 v = handle.InverseTransformDirection(rb.linearVelocity);
        if (pos.x > areasize.x/2){
            pos.x = areasize.x/2;
            v.x = 0;
        }
        if (pos.x < -areasize.x/2){
            pos.x = -areasize.x/2;
            v.x = 0;
        }
        if (pos.z > areasize.y/2){
            pos.z = areasize.y/2;
            v.z = 0;
        }
        if (pos.z < -areasize.y/2){
            pos.z = -areasize.y/2;
            v.z = 0;
        }

        handle.localPosition = pos;
        rb.linearVelocity = handle.TransformDirection(v);
        base.FixedUpdate();
    }

    protected override float CalculateDistance(Vector2 v1, Vector2 v2)
    {
        return Vector3.Distance(v1, v2);
    }

    protected override Vector2 CalculateValue(Transform reference)
    {
        Vector3 pos = reference.localPosition;
        return new Vector2(Mathf.InverseLerp(-areasize.x/2, areasize.x/2, pos.x - anchorOffset.x), Mathf.InverseLerp(-areasize.y/2, areasize.y/2, pos.z - anchorOffset.z));
    }

    protected override PosRot CalculateLocalTransform(Vector2 value)
    {
        Vector3 pos = new Vector3(Mathf.Lerp(-areasize.x/2, areasize.x/2, value.x), 0, Mathf.Lerp(-areasize.y/2, areasize.y/2, value.y)) + anchorOffset;
        return new PosRot(pos, displayHandle.localRotation);
    }

    protected override List<Vector3> GetBoundaryPoints()
    {
        return new List<Vector3>{
            new Vector3( areasize.x/2, 0,  areasize.x/2) + anchorOffset,
            new Vector3( areasize.x/2, 0, -areasize.x/2) + anchorOffset,
            new Vector3(-areasize.x/2, 0, -areasize.x/2) + anchorOffset,
            new Vector3(-areasize.x/2, 0,  areasize.x/2) + anchorOffset
        };
    }
}
