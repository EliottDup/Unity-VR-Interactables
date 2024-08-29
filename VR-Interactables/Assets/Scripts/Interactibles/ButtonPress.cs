using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonPress : MonoBehaviour
{
    [SerializeField]
    float _deadTime = 0.5f;
    bool _deadTimeIsActive = false;
    bool _isFrozen = false;
    [SerializeField] float _resetSpeed = 3;


    [SerializeField] Transform _visualTarget;
    Vector3 _visualTargetInitialPosition;
    [SerializeField]
    Vector3 _localAxis = Vector3.down;

    private Vector3 _offset;
    private Transform _pokeAttachTransform;

    bool _isFollowing = false;

    [Header("Events")]
    public UnityEvent onButtonDown, onButtonUp;

    void Start()
    {
        _visualTargetInitialPosition = _visualTarget.localPosition;
    }

    void Update()
    {
        if (_isFrozen) return;
        if (_isFollowing)
        {
            Vector3 localTargetPosition = _visualTarget.InverseTransformPoint(_pokeAttachTransform.position + _offset);
            Vector3 constrainedLocalTargetPosition = Vector3.Project(localTargetPosition, _localAxis);
            _visualTarget.position = _visualTarget.TransformPoint(constrainedLocalTargetPosition);
        }
        else
        {
            _visualTarget.localPosition = Vector3.Lerp(_visualTarget.localPosition, _visualTargetInitialPosition, Time.deltaTime * _resetSpeed);
        }
    }

    public void StartPress(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor)args.interactorObject;
            _isFollowing = true;
            _isFrozen = false;
            _pokeAttachTransform = interactor.attachTransform;
            _offset = _visualTarget.position - _pokeAttachTransform.position;
        }
    }


    public void EndPress(BaseInteractionEventArgs args)
    {
        _isFrozen = false;
        if (args.interactorObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor)args.interactorObject;
            _isFollowing = false;
            if (!_deadTimeIsActive)
            {
                StartCoroutine(nameof(ResetDeadTime));
                onButtonUp.Invoke();
            }
        }
    }

    public void Freeze(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor)args.interactorObject;
            if (!_deadTimeIsActive)
            {
                _isFrozen = true;
                if (!_deadTimeIsActive)
                {
                    onButtonDown.Invoke();
                }
            }
        }
    }

    IEnumerator ResetDeadTime()
    {
        _deadTimeIsActive = true;
        yield return new WaitForSeconds(_deadTime);
        _deadTimeIsActive = false;
    }
}
