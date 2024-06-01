using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
public class SliderLogic : MonoBehaviour
{

    [SerializeField] float _sliderLength;
    [SerializeField] Vector3 anchorOffset;
    [SerializeField] float pullForce = 200;

    Vector3 _originalPos;

    Vector3 startPoint
    {
        get
        {
            return _originalPos + anchorOffset - transform.forward * _sliderLength / 2;
        }
    }
    Vector3 endPoint
    {
        get
        {
            return _originalPos + anchorOffset + transform.forward * _sliderLength / 2;
        }
    }

    Rigidbody rb;
    ConfigurableJoint cj;

    [SerializeField] float _minValue, _maxValue;

    bool _isHeld = false;
    Transform _target;

    float _value;
    public UnityEvent<float> OnValueChanged;

    [SerializeField] bool _debugDrawGizmos = false;

    void Start()
    {
        _originalPos = transform.position;
        rb = GetComponent<Rigidbody>();
        cj = GetComponent<ConfigurableJoint>();
        SetSettings();
    }

    void OnDrawGizmosSelected()
    {
        if (_debugDrawGizmos)
        {
            Vector3 pos = transform.position;
            if (Application.isPlaying)
            {
                pos = Vector3.zero;
            }
            Gizmos.DrawSphere(pos + startPoint, 0.025f);
            Gizmos.DrawSphere(pos + endPoint, 0.025f);
            Gizmos.DrawLine(pos + startPoint, pos + endPoint);
        }
    }

    void SetSettings()
    {
        print("jej");
        cj.anchor = -anchorOffset;
        SoftJointLimit linearLimit = new SoftJointLimit();
        linearLimit.limit = _sliderLength / 2;
        cj.linearLimit = linearLimit;
    }

    void FixedUpdate()
    {
        if (_isHeld && _target != null)
        {
            Vector3 targetDir = startPoint + Vector3.Project(_target.position - startPoint, endPoint - startPoint) - transform.position;
            rb.AddForce(targetDir * pullForce);
            Debug.DrawRay(transform.position, targetDir, Color.yellow);
        }
        float newValue = GetSliderValue();
        if (newValue != _value)
        {
            _value = newValue;
            OnValueChanged.Invoke(_value);
        }
    }

    float GetSliderValue()
    {
        float normalizedvalue = Vector3.Distance(startPoint, transform.position) / _sliderLength;
        return Mathf.Lerp(_minValue, _maxValue, Mathf.Clamp01(normalizedvalue));
    }

    public void OnSelectEntered(BaseInteractionEventArgs args)
    {
        _isHeld = true;
        _target = args.interactorObject.transform;
    }

    public void OnSelectExit(BaseInteractionEventArgs args)
    {
        _isHeld = false;
    }

}
