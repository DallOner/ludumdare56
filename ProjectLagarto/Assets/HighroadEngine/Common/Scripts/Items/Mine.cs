using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 5f;        // Radio de detección de enemigos
    [SerializeField] private float explosionDelay = 0.5f;       // Retraso antes de la explosión
    [SerializeField] private float explosionRadius = 10f;       // Radio de la explosión
    [SerializeField] private int damage = 100;                  // Daño causado por la explosión
    [SerializeField] private LayerMask _hitLayers;             // Capas que representan a los enemigos
    [SerializeField] private GameObject explosionEffectPrefab;  // Prefab del efecto de explosión

    private bool isTriggered = false; // Indica si la mina ha sido activada

    private void Update()
    {
        if (isTriggered)
            return;

        // Detectar enemigos en el radio de detección
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, _hitLayers);
        if (hits.Length > 0)
        {
            // Un enemigo ha entrado en el radio de detección
            isTriggered = true;
            Debug.Log("Mina activada.");
            Invoke(nameof(Explode), explosionDelay);
        }
    }

    private void Explode()
    {
        // Mostrar efecto de explosión
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Detectar enemigos en el radio de explosión y aplicar daño
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, _hitLayers);
        foreach (Collider hit in hits)
        {
            IDamageReceiver damageReceiver = hit.GetComponent<IDamageReceiver>();
            if (damageReceiver != null)
            {
                damageReceiver.ReceiveDamage(damage);
            }
        }

        Debug.Log("Mina explotó.");
        Destroy(gameObject); // Destruir la mina después de la explosión
    }

    // Opcional: Visualizar los radios en la escena
    private void OnDrawGizmosSelected()
    {
        // Radio de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Radio de explosión
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
