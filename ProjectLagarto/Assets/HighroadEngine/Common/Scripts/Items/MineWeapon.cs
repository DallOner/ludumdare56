using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineWeapon : WeaponBase
{
    [SerializeField] private GameObject minePrefab;    // Prefab de la mina

    public override void Shoot()
    {
        if (minePrefab == null)
        {
            Debug.LogError("El prefab de la mina no está asignado. No se puede colocar la mina.");
            return;
        }

        if (_firePoint == null)
        {
            Debug.LogError("El punto de disparo no está asignado.");
            return;
        }

        if (CanShoot)
        {
            _currentAmmo--;
            _nextTimeToFire = Time.time + _fireRate;

            // Instanciar la mina en el punto de disparo
            Instantiate(minePrefab, _firePoint.position, Quaternion.identity);
            Debug.Log("Mina colocada.");
        }
        else
        {
            Debug.Log("No se puede colocar la mina en este momento.");
        }
    }
}