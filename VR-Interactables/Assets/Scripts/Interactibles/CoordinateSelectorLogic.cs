using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class CoordinateSelectorLogic : MonoBehaviour
{
    [SerializeField] Vector2 _size;
    [SerializeField] Vector3 _centerOffset;
    Vector3 _startingLocalPosition;
    Vector3 _startingPosition;

    [SerializeField] float _pullForce = 200;

    // [HideInInspector]
    public Vector2 value = Vector2.zero;

    Rigidbody rb;

    [Header("Events")]
    public UnityEvent<Vector2> OnValueChanged;

    public bool _debugDrawGizmos = false;

    bool _isHeld = false;
    Transform _target;

    void OnDrawGizmos()
    {
        if (!_debugDrawGizmos) return;

        Vector3 centerPos = _startingPosition + _centerOffset;
        if (!Application.isPlaying)
        {
            centerPos += transform.position;
        }
        Vector3 p1 = centerPos + transform.forward * _size.y / 2 + transform.right * _size.x / 2;
        Vector3 p2 = centerPos + transform.forward * _size.y / 2 - transform.right * _size.x / 2;
        Vector3 p3 = centerPos - transform.forward * _size.y / 2 - transform.right * _size.x / 2;
        Vector3 p4 = centerPos - transform.forward * _size.y / 2 + transform.right * _size.x / 2;

        Vector3[] points = new Vector3[8]{
            p1, p2,
            p2, p3,
            p3, p4,
            p4, p1
        };

        Gizmos.DrawLineList(points);
    }

    void Start()
    {
        _startingLocalPosition = transform.localPosition;
        _startingPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (_isHeld)
        {
            Vector3 dirToHand = _target.position - transform.position;
            Vector3 projectedDirection = Vector3.ProjectOnPlane(dirToHand, transform.up);
            rb.AddForce(projectedDirection * _pullForce);
        }

        float maxX = _startingLocalPosition.x + _centerOffset.x + _size.x / 2;
        float minX = _startingLocalPosition.x + _centerOffset.x - _size.x / 2;
        float maxZ = _startingLocalPosition.z + _centerOffset.z + _size.y / 2;
        float minZ = _startingLocalPosition.z + _centerOffset.z - _size.y / 2;

        Vector3 localVel = transform.InverseTransformDirection(rb.velocity);

        float localX = transform.localPosition.x;
        float localZ = transform.localPosition.z;

        if (transform.localPosition.x > maxX)
        {
            localX = maxX;
            localVel.x *= -0f;
        }
        else if (transform.localPosition.x < minX)
        {
            localX = minX;
            localVel.x *= -0f;
        }

        if (transform.localPosition.z > maxZ)
        {
            localZ = maxZ;
            localVel.z *= -0f;
        }
        else if (transform.localPosition.z < minZ)
        {
            localZ = minZ;
            localVel.z *= -0f;
        }
        transform.localPosition = new Vector3(localX, _startingLocalPosition.y, localZ);
        rb.velocity = transform.TransformDirection(localVel);

        float xValue = Mathf.InverseLerp(minX, maxX, localX);
        float zValue = Mathf.InverseLerp(minZ, maxZ, localZ);

        Vector2 newValue = new(xValue, zValue);
        if (value != newValue)
        {
            OnValueChanged.Invoke(newValue);
        }
        value = newValue;
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
