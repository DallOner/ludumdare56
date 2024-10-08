using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageItem : MonoBehaviour
{
    [SerializeField] private int damageAmount = 25;  // Cantidad de daño que causa el objeto
    // private bool _isActivated = false;  // Evita múltiples activaciones del Trigger en un solo frame

    /// <summary>
    /// Maneja la entrada en el Trigger con otros objetos. Si el objeto puede recibir daño, aplica el daño.
    /// </summary>
    /// <param name="other">El objeto con el que entra en el Trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        // if (_isActivated)
        // {
        //     Debug.LogWarning($"[DamageItem] El objeto ya ha sido activado y no puede causar daño nuevamente: {gameObject.name}");
        //     return;  // Evita que el objeto procese múltiples intersecciones simultáneamente
        // }

        var damageReceiver = other.GetComponent<IDamageReceiver>();  // Verifica si el objeto puede recibir daño
        if (damageReceiver != null)
        {
            Debug.Log($"[DamageItem] Se detectó un objeto que puede recibir daño: {other.gameObject.name}");
            ApplyDamage(damageReceiver);  // Aplica el daño
        }
        else
        {
            Debug.Log($"[DamageItem] La colisión no fue con un objeto que puede recibir daño: {other.gameObject.name}");
        }
    }

    /// <summary>
    /// Aplica el daño al receptor y destruye el objeto de daño.
    /// </summary>
    /// <param name="receiver">El receptor del daño.</param>
    private void ApplyDamage(IDamageReceiver receiver)
    {
        // _isActivated = true;  // Marca el objeto como activado
        receiver.ReceiveDamage(damageAmount);
        Debug.Log($"[DamageItem] Se ha aplicado un daño de {damageAmount} al objeto: {receiver}.");
        
        // Destroy(gameObject);  // Destruye el objeto de daño
        // Debug.Log($"[DamageItem] El objeto {gameObject.name} ha sido destruido después de aplicar el daño.");
    }
}

