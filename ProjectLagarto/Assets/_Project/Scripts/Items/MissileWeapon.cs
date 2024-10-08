using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileWeapon : WeaponBase
{
    [SerializeField] private GameObject missilePrefab;  // Prefab del misil
    [SerializeField] private Transform launchPoint;     // Punto de lanzamiento del misil
    [SerializeField] private float detectionRange = 100f; // Rango máximo de detección de enemigos
    [SerializeField] private LayerMask enemyLayerMask;  // Máscara de capa para identificar a los enemigos

    public override void Shoot()
    {
        if (missilePrefab == null)
        {
            Debug.LogError("El prefab del misil no está asignado. No se puede disparar.");
            return;
        }

        if (launchPoint == null)
        {
            Debug.LogError("El punto de lanzamiento no está asignado.");
            return;
        }

        if (CanShoot)
        {
            _currentAmmo--;
            _nextTimeToFire = Time.time + _fireRate;

            // Instanciar el misil en el punto de lanzamiento
            GameObject missileObject = Instantiate(missilePrefab, launchPoint.position, launchPoint.rotation);
            Missile missile = missileObject.GetComponent<Missile>();

            if (missile != null)
            {
                // Asignar el objetivo al misil
                Transform enemyTransform = FindClosestEnemy();
                if (enemyTransform != null)
                {
                    missile.SetTarget(enemyTransform);
                }
                else
                {
                    Debug.LogWarning("No se encontró ningún enemigo. El misil volará hacia adelante.");
                }

                // Llamar a Launch() para iniciar el misil y comenzar el contador de vida
                missile.Launch();
            }
            else
            {
                Debug.LogError("El objeto instanciado no tiene un componente Missile.");
            }
        }
        else
        {
            Debug.Log("No se puede disparar en este momento.");
        }
    }

    private Transform FindClosestEnemy()
    {
        // Encontrar todos los colliders en el rango de detección que estén en la capa de enemigos
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, enemyLayerMask);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider hit in hits)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = hit.transform;
            }
        }

        return closestEnemy;
    }
}