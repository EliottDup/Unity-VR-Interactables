using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class LongGrabInteractor : XRDirectInteractor
{
    [Header("Far Grabbing Settings")]
    [SerializeField] Transform shoulder;
    [SerializeField] float minimumDistance = 0.08f;
    [SerializeField] [Range(0, 1)] float dotCutoff = 0.125f;
    [SerializeField] float dotExp = 4;
    [SerializeField] GameObject reticlePrefab;
    Transform activeReticle;

    public override void GetValidTargets(List<IXRInteractable> targets)
    {
        targets.Clear();
        targets = GetSortedPossibleFarTargets();
        if (targets.Count > 0){
            SetReticlePosition(targets[0].transform.position);
        }
        else if(activeReticle != null){
            Destroy(activeReticle.gameObject);
        }
    }

    void OnDrawGizmos(){
        if (interactionManager == null) return;
        foreach(var target in GetSortedPossibleFarTargets()){
            Gizmos.DrawSphere(target.transform.position, 0.125f);
        }
    }

    List<IXRInteractable> GetSortedPossibleFarTargets(){
        Debug.Log("a");
        List<IXRInteractable> targets = new();
        interactionManager.GetRegisteredInteractables(targets);
        Dictionary<IXRInteractable, float> scores = new();

        for (int i = targets.Count-1; i >= 0; i--){
            IXRInteractable target = targets[i];
            if (!CanSelect(target as IXRSelectInteractable) || Vector3.Distance(target.transform.position, transform.position) < minimumDistance || Vector3.Dot((transform.position - target.transform.position).normalized, (shoulder.transform.position - transform.position).normalized) < dotCutoff){
                continue;
            }
            float score = Vector3.Distance(target.transform.position, transform.position)/Mathf.Pow(Vector3.Dot((transform.position - target.transform.position).normalized, (shoulder.transform.position - transform.position).normalized), dotExp);
            scores.Add(target, score);
        }
        List<IXRInteractable> validTargets = scores.OrderBy(pair => pair.Value).Select(pair => pair.Key).ToList();
        return validTargets;
    }

    void SetReticlePosition(Vector3 position){
        if (activeReticle == null){
            activeReticle = Instantiate(reticlePrefab, position, Quaternion.identity).transform;
        }
        else{
            activeReticle.position = position;
        }
    }
}
