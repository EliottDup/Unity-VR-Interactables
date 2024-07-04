using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraViewFinder : MonoBehaviour
{
    Transform _mainCamTransform;
    RawImage _image;


    void Start()
    {
        _image = GetComponent<RawImage>();
        _mainCamTransform = Camera.main.transform;
    }

    void Update()
    {
        if (Vector3.Dot(transform.forward, _mainCamTransform.position - transform.position) < 0)
        {
            _image.enabled = false;
        }
        else
        {
            _image.enabled = true;
        }
    }

}
