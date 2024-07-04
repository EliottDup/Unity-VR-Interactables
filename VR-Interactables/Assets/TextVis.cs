using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextVis : MonoBehaviour
{
    public TextMeshPro textMeshPro;

    public void SetValue(float val)
    {
        textMeshPro.text = val.ToString();
    }
}
