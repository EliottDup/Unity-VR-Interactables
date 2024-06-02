using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooridinateSelectorVisualizer : MonoBehaviour
{
    [SerializeField] Slider sliderX, sliderY;

    public void UpdateSliders(Vector2 value)
    {
        sliderX.value = value.x;
        sliderY.value = value.y;
    }
}
