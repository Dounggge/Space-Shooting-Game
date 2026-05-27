using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Pools bulletPool; 
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask targetLayer; 

    [Header("Settings")]
    [SerializeField] private float fireRate = 1.5f;

    private float nextFireTime;

    private void Update()
    {

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (bulletPool == null || firePoint == null) return;

        GameObject bulletObj = bulletPool.SetObject();
        if (bulletObj == null) return;


        bulletObj.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);


        if (bulletObj.TryGetComponent<Bullet>(out var bullet))
        {
            bullet.InitBullet(targetLayer);
        }

        bulletObj.SetActive(true);
    }
}