using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] float _deadTime = 0.5f;
    bool _deadTimeActive = false;

    [SerializeField] GameObject buttonPresser;

    public UnityEvent OnPressed, OnReleased;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Button" && !_deadTimeActive)
        {
            OnPressed.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Button" && !_deadTimeActive)
        {
            OnReleased.Invoke();

        }
    }

    IEnumerator WaitForDeadTime()
    {
        _deadTimeActive = true;
        yield return new WaitForSeconds(_deadTime);
        _deadTimeActive = false;
    }
}
