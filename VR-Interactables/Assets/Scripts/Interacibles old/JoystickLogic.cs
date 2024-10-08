using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class JoystickLogic : Interactable<Vector2>
{
    [SerializeField] float _maxAngle = 30f;

    Vector3 _initialForward;
    Vector3 _initialUp;
    Vector3 _initialRight;

    [SerializeField] float _holdHeight = 0.15f;
    [SerializeField] float _pullForce = 200;

    Rigidbody rb;

    bool _isHeld;
    Transform _target;

    public UnityEvent<Vector2> _onValueChanged;

    void Start()
    {
        _initialUp = transform.up;
        _initialForward = transform.forward;
        _initialRight = transform.right;

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        if (_isHeld)
        {
            Vector3 vec2Hand = _target.position - transform.position;
            rb.AddForceAtPosition(vec2Hand * _pullForce, transform.position + transform.up * _holdHeight);
        }

        float currentAngle = Quaternion.Angle(Quaternion.identity, transform.localRotation);
        if (currentAngle > _maxAngle)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.identity, currentAngle - _maxAngle);
        }

        Vector3 xyProjected = Vector3.ProjectOnPlane(transform.up, _initialForward);
        Vector3 zyProjected = Vector3.ProjectOnPlane(transform.up, _initialRight);

        float xAngle = Vector3.SignedAngle(_initialUp, xyProjected, _initialForward);
        float zAngle = Vector3.SignedAngle(_initialUp, zyProjected, _initialRight);

        float xVal = Mathf.Lerp(-1, 1, Mathf.InverseLerp(-_maxAngle, _maxAngle, xAngle));
        float zVal = Mathf.Lerp(-1, 1, Mathf.InverseLerp(-_maxAngle, _maxAngle, zAngle));
        Vector2 newVal = new Vector2(xVal, zVal);
        if (_value != newVal)
        {
            _value = new Vector2(xVal, zVal);
            _onValueChanged.Invoke(_value);
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + transform.up * _holdHeight, 0.025f);
    }

    public void OnSelectEnter(BaseInteractionEventArgs args)
    {
        _isHeld = true;
        _target = args.interactorObject.transform;
    }

    public void OnSelectExit(BaseInteractionEventArgs args)
    {
        _isHeld = false;
    }
}
