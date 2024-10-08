using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;           // Velocidad del misil
    [SerializeField] private float turnSpeed = 20f;       // Velocidad de giro del misil
    [SerializeField] private float lifeTime = 10f;        // Tiempo de vida máximo del misil
    [SerializeField] private LineRenderer lineRenderer;   // Componente Line Renderer para la traza

    private Transform target;                             // El objetivo (enemigo)
    private bool isLaunched = false;                      // Indica si el misil ha sido lanzado

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    /// <summary>
    /// Lanza el misil y comienza el contador de vida.
    /// </summary>
    public void Launch()
    {
        isLaunched = true;
        Destroy(gameObject, lifeTime); // Destruir el misil después de lifeTime segundos desde que es lanzado
    }

    private void Start()
    {
        // Inicializar la posición de la línea con la posición actual del misil
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, transform.position);
        }
    }

    private void Update()
    {
        if (!isLaunched)
            return; // Si el misil no ha sido lanzado, no hacemos nada

        if (target != null)
        {
            // Calcular la dirección hacia el objetivo
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Rotar hacia el objetivo
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
        }

        // Mover el misil hacia adelante
        transform.position += transform.forward * speed * Time.deltaTime;

        // Actualizar la traza de la trayectoria
        UpdateTrail();

        // Si el misil está cerca del enemigo, explota y se destruye
        if (target != null && Vector3.Distance(transform.position, target.position) < 2f)
        {
            Impact();
        }
    }

    /// <summary>
    /// Actualiza la traza de la trayectoria usando el Line Renderer.
    /// </summary>
    private void UpdateTrail()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position);
        }
    }

    /// <summary>
    /// Maneja el impacto del misil.
    /// </summary>
    private void Impact()
    {
        // Aquí puedes añadir efectos visuales, sonido o daño al enemigo
        Debug.Log("Misil impactado en el enemigo.");

        // Si el enemigo tiene un componente de IDamageReceiver, aplicamos daño
        IDamageReceiver damageReceiver = target.GetComponent<IDamageReceiver>();
        if (damageReceiver != null)
        {
            damageReceiver.ReceiveDamage(50);  // Ajusta el valor de daño según tus necesidades
        }

        Destroy(gameObject);  // Destruir el misil después del impacto
    }
}