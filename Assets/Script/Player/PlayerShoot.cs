using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Pools bulletPool;
    [SerializeField] private Transform firePoint;

    [Header("Settings")]
    [SerializeField] private float fireRate = 0.2f;

    private Coroutine shootCoroutine;
    private float nextFireTime;

    public void StartShooting()
    {
        if (shootCoroutine != null) return;
        shootCoroutine = StartCoroutine(ShootContinuously());
    }

    public void StopShooting()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }
    }

    private IEnumerator ShootContinuously()
    {
        while (Time.time < nextFireTime)
        {
            yield return null;
        }


        while (true)
        {
            Shoot();
            GetComponent<PlayerSound>()?.PlayShoot();
            nextFireTime = Time.time + fireRate;
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void Shoot()
    {
        if (bulletPool == null || firePoint == null) return;

        GameObject bulletObj = bulletPool.SetObject();
        if (bulletObj == null) return;

        bulletObj.transform.position = firePoint.position;
        bulletObj.transform.rotation = firePoint.rotation;
        bulletObj.SetActive(true);
    }
}