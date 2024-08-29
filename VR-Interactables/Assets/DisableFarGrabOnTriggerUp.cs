using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DisableFarGrabOnTriggerUp : MonoBehaviour
{
    [SerializeField] InputActionReference triggerButton;
    NearFarInteractor nearFarInteractor;

    void Awake(){
        nearFarInteractor = GetComponent<NearFarInteractor>();
    }

    void Update(){
        nearFarInteractor.enableFarCasting = triggerButton.action.ReadValue<float>() > 0.5f;
    }
}
