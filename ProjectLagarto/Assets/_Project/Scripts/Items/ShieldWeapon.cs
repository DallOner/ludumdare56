using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldWeapon : WeaponBase
{
    [SerializeField] private float shieldDuration = 5f;         // Duración del escudo en segundos
    [SerializeField] private GameObject shieldEffectPrefab;     // Prefab del efecto visual del escudo

    public override void Shoot()
    {
        if (CanShoot)
        {
            _currentAmmo--;
            _nextTimeToFire = Time.time + _fireRate;

            // Activar el escudo en el jugador
            HealthController healthController = GetComponentInParent<HealthController>();
            if (healthController != null)
            {
                healthController.ActivateShield(shieldDuration, shieldEffectPrefab);
                Debug.Log("Escudo activado por ShieldWeapon.");
            }
            else
            {
                Debug.LogError("No se encontró el componente HealthController en los padres.");
            }
        }
        else
        {
            Debug.Log("No se puede activar el escudo en este momento.");
        }
    }
} 
