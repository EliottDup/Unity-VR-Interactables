using System.Collections.Generic;
using UnityEngine;

public class SnappingSlider : SnappingInteractible<float>
{

    Vector3 StartPoint
    {
        get
        {
            return anchorOffset - transform.forward * _sliderLength / 2;
        }
    }
    Vector3 EndPoint
    {
        get
        {
            return anchorOffset + transform.forward * _sliderLength / 2;
        }
    }

    [Header("Slider Specific: ")]
    [SerializeField] float _sliderLength = 0.2f;
    
    protected override void SetSettings(){
        base.SetSettings();
        SoftJointLimit linearLimit = new()
        {
            limit = _sliderLength / 2
        };
        cj.linearLimit = linearLimit;
    }

    protected override float CalculateValue(Transform reference){
        return Vector3.Distance(reference.localPosition, StartPoint) / _sliderLength;
    }
    
    protected override PosRot CalculateLocalTransform(float value)
    {
        Vector3 position = Vector3.Lerp(StartPoint, EndPoint, value);
        return new PosRot(position, displayHandle.localRotation);
    }

    protected override List<Vector3> GetBoundaryPoints()
    {
        return new List<Vector3> { StartPoint, EndPoint};
    }

    protected override float CalculateDistance(float v1, float v2)
    {
        return Mathf.Abs(v1 - v2) * _sliderLength;
    }
}
