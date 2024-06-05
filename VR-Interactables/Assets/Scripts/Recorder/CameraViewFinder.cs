using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraViewFinder : MonoBehaviour
{
    [SerializeField] Camera _recorderCamera;
    Transform _mainCamTransform;
    RawImage _image;


    void Start()
    {
        _image = GetComponent<RawImage>();
        _mainCamTransform = Camera.main.transform;
    }

    void Update()
    {
        _image.texture = _recorderCamera.targetTexture;
        if (Vector3.Dot(transform.forward, _mainCamTransform.forward) > 0)
        {
            _image.enabled = false;
        }
        else
        {
            _image.enabled = true;
        }
    }

}
