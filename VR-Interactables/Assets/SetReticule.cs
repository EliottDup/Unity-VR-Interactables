using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SetReticule : MonoBehaviour
{
    [SerializeField] GameObject reticulePrefab;
    Transform reticule;
    Transform reticuleParent;

    NearFarInteractor interactor;

    void Awake(){
        interactor = GetComponent<NearFarInteractor>();
        interactor.hoverEntered.AddListener(OnHoverEntered);
        interactor.hoverExited.AddListener(OnHoverExited);
    }

    void Update(){
        if(reticule != null && reticuleParent != null){
            reticule.position = reticuleParent.position;
        }
    }

    void OnHoverEntered(HoverEnterEventArgs args){
        reticule = Instantiate(reticulePrefab).transform;
        reticuleParent = args.interactableObject.transform;
    }

    void OnHoverExited(HoverExitEventArgs args){
        Destroy(reticule.gameObject);
        reticuleParent = null;
    }
}
