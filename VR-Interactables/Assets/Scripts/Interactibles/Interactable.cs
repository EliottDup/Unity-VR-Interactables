using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable<T> : MonoBehaviour
{
    protected T value;

    public T GetValue()
    {
        return value;
    }

    public UnityEvent<T> OnValueChanged;
}
