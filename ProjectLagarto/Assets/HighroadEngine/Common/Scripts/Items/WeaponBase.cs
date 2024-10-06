using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] protected int _maxAmmo = 30;
    [SerializeField] protected int _currentAmmo;
    [SerializeField] protected float _reloadTime = 2f;
    [SerializeField] protected float _fireRate = 0.2f;
    [SerializeField] protected Transform _firePoint;  // El punto desde donde dispara el arma
    [SerializeField] protected LayerMask _hitLayers;  // Capas que pueden ser impactadas por los disparos
    [SerializeField] protected LineRenderer lineRenderer;  // Componente para dibujar la bala
    [SerializeField] protected float lineDuration = 0.05f;  // Duración de la línea (visible brevemente)

    protected bool _isReloading = false;
    protected float _nextTimeToFire = 0f;

    private void Start()
    {
        _currentAmmo = _maxAmmo;
    }

    public bool CanShoot => _currentAmmo > 0 && !_isReloading && Time.time >= _nextTimeToFire;
    public bool CanReload => _currentAmmo < _maxAmmo && !_isReloading;

    public virtual void Shoot()
    {
        if (CanShoot)
        {
            _currentAmmo--;
            _nextTimeToFire = Time.time + _fireRate;

            // Raycast para detección del disparo
            RaycastHit hit;
            if (Physics.Raycast(_firePoint.position, _firePoint.forward, out hit, Mathf.Infinity, _hitLayers))
            {
                Debug.Log($"Impacto en: {hit.collider.name}");

                // Si el objeto impactado implementa IDamageReceiver, aplicamos daño
                IDamageReceiver damageReceiver = hit.collider.GetComponent<IDamageReceiver>();
                if (damageReceiver != null)
                {
                    damageReceiver.ReceiveDamage(10);  // Ajusta el valor de daño según el arma
                }

                // Mostrar la trayectoria del disparo
                StartCoroutine(ShowLine(hit.point));
            }
        }
    }

    private IEnumerator ShowLine(Vector3 hitPoint)
    {
        lineRenderer.SetPosition(0, _firePoint.position);  // Inicio de la línea
        lineRenderer.SetPosition(1, hitPoint);  // Fin de la línea

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(lineDuration);  // Duración de la línea visible

        lineRenderer.enabled = false;
    }

    public virtual void Reload()
    {
        if (CanReload)
        {
            _isReloading = true;
            Debug.Log("Recargando...");
            Invoke(nameof(FinishReload), _reloadTime);
        }
    }

    private void FinishReload()
    {
        _currentAmmo = _maxAmmo;
        _isReloading = false;
        Debug.Log("Recarga completada.");
    }
}