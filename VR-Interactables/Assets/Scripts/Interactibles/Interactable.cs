using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable<T> : MonoBehaviour
{
    protected T _value;

    public T GetValue(){
        return _value;
    }

    public void SetValue(){
        throw new NotImplementedException();
    }

    public UnityEvent<T> OnValueChanged;
    }
