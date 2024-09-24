using UnityEngine;

public class RaycastMagazine : MonoBehaviour, IMagazine
{
    [Header("Magasine Stats")]
    [SerializeField] int maxBullets;
    int remainingBullets;

    [Header("Bullet Stats")]
    [SerializeField] float bulletDamage;
    [SerializeField] float bulletImpactForce;

    void Awake(){
        remainingBullets = maxBullets;
    }

    public int GetRemainingBullets()
    {
        return remainingBullets;
    }

    public void Shoot(Vector3 origin, Vector3 direction)
    {
        //play animation(if necessary)
        if (remainingBullets <= 0){
            Debug.Log("out of ammo");
            return;
        }
        Debug.Log("pew");
        remainingBullets -= 1;
        //shoot
        if (Physics.Raycast(origin, direction, out RaycastHit hit, 100f)){
            OnHit(hit, direction);
        }
    }

    protected void OnHit(RaycastHit hit, Vector3 direction){
        if (hit.rigidbody == null){
            return;
        }
        hit.rigidbody.AddForceAtPosition(direction * bulletImpactForce, hit.point);
    }
}
