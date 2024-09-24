using UnityEngine;

public interface IMagazine
{
    void Shoot(Vector3 origin, Vector3 direction);
    int GetRemainingBullets();
}
