using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderMovement : MonoBehaviour
{
    [SerializeField] float heightOffset = 0.2f;
    [SerializeField] Transform cam;

    void Update(){
        transform.SetPositionAndRotation(cam.position + Vector3.up * heightOffset, Quaternion.Euler(0, cam.rotation.eulerAngles.y, 0));
    }
}
