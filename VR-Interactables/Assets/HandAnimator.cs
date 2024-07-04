using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class HandAnimator : MonoBehaviour
{
    float trigger, grip, thumb;


    [SerializeField] InputActionReference gripInputActionReference;
    [SerializeField] InputActionReference triggerInputActionReference;
    [SerializeField] InputActionReference thumbInputActionReference;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        trigger = triggerInputActionReference.action.ReadValue<float>();
        animator.SetFloat("Trigger", trigger);

        grip = gripInputActionReference.action.ReadValue<float>();
        animator.SetFloat("Grip", grip);

        if (thumbInputActionReference.action.ReadValue<float>() != 0f)
        {
            thumb = Mathf.MoveTowards(thumb, 1f, 5f * Time.deltaTime);
        }
        else
        {
            thumb = Mathf.MoveTowards(thumb, 0f, 5f * Time.deltaTime);
        }

        animator.SetFloat("Thumb", thumb);
    }
}
