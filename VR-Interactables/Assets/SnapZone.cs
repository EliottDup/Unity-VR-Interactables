using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class SnapZone : MonoBehaviour
{
    public List<string> tags = new List<string>();

    public bool containsObject = false;
    public Transform heldObject;

    public Transform snapPosition;

    public bool hadNonKinematicRigidBody;


    void OnTriggerEnter(Collider other)
    {
        Snappable snappableInfo = other.GetComponent<Snappable>();
        if (snappableInfo != null)
        {
            bool canSnap = tags.Intersect(snappableInfo.tags).Any();
            if (canSnap) snappableInfo.currentSnapZone = this;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Snappable snappableInfo = other.GetComponent<Snappable>();
        if (snappableInfo != null)
        {
            if (snappableInfo.currentSnapZone == this) snappableInfo.currentSnapZone = null;
        }
    }

    public bool TryAttachObject(Transform obj)
    {
        if (containsObject && heldObject != null) return false;
        AttachObject(obj);
        return true;
    }

    public void AttachObject(Transform obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null && rb.isKinematic)
        {
            hadNonKinematicRigidBody = true;
            rb.isKinematic = true;
        }
        else hadNonKinematicRigidBody = false;

        obj.parent = snapPosition;
        obj.localPosition = Vector3.zero;
        obj.localEulerAngles = Vector3.zero;
        heldObject = obj;
        containsObject = true;
    }

    public void DetachObject()
    {
        if (heldObject != null)
        {
            heldObject.parent = null;
            if (hadNonKinematicRigidBody)
            {
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            heldObject.GetComponent<Snappable>().currentSnapZone = null;
            heldObject = null;
            containsObject = false;
        }
    }
}
