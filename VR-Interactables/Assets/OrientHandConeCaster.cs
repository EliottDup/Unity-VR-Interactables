using UnityEngine;

public class OrientHandConeCaster : MonoBehaviour
{
    [SerializeField] Transform cam;

    void Update(){
        transform.LookAt(2*transform.position - cam.position);
    }
}
