using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonLogic : Interactable<bool>
{
    [SerializeField] bool _isToggle = false;

    private bool _value = false;
    public bool buttonValue
    {
        get
        {
            return _value;
        }

        private set
        {
            if (value != _value)
            {
                if (value)
                {
                    OnValueTrue.Invoke();
                }
                else
                {
                    OnValueFalse.Invoke();
                }
                OnValueChanged.Invoke(value);
            }
            _value = value;
        }
    }

    [Header("Events")]

    public UnityEvent OnValueTrue;
    public UnityEvent OnValueFalse;

    public void OnPress(BaseInteractionEventArgs args)
    {
        if (_isToggle)
        {
            buttonValue = !buttonValue;
        }
        else
        {
            buttonValue = true;
        }
    }

    public void OnRelease(BaseInteractionEventArgs args)
    {
        if (!_isToggle)
        {
            buttonValue = false;
        }
    }
}
