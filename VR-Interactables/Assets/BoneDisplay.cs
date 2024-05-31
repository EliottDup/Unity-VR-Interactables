using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoneDisplay : MonoBehaviour
{
    void OnDrawGizmos()
    {
        DrawBones(transform, 0);
    }

    void DrawBones(Transform parent, int n)
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(parent.position, 0.0025f);
        foreach (Transform child in parent)
        {
            Gizmos.DrawLine(parent.position, child.position);
            DrawBones(child, n + 1);
        }
    }
}
