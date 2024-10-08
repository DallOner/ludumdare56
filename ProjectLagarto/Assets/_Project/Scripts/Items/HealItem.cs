using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

public class HealItem : MonoBehaviour
{
    [SerializeField] private int healAmount = 25;  // Cantidad de vida que otorga el objeto
    private bool _isConsumed = false;  // Evitar múltiples activaciones del Trigger en un solo frame

    /// <summary>
    /// Maneja la entrada en el Trigger con otros objetos. Si el objeto puede recibir curación, aplica la curación.
    /// </summary>
    /// <param name="other">El objeto con el que entra en el Trigger.</param>
    private void OnTriggerEnter(Collider other)  // Cambiado a OnTriggerEnter para detectar colisiones con Trigger
    {
        if (_isConsumed) 
        {
            Debug.LogWarning($"[HealItem] El objeto ya ha sido consumido y no puede curar nuevamente: {gameObject.name}");
            return;  // Evitar que el objeto procese múltiples intersecciones simultáneamente
        }

        var healReceiver = other.GetComponent<IHealReceiver>();  // Verifica si el objeto puede recibir curación
        if (healReceiver != null)
        {
            Debug.Log($"[HealItem] Se detectó un objeto que puede recibir curación: {other.gameObject.name}");
            ApplyHeal(healReceiver);  // Aplica la curación
        }
        else
        {
            Debug.Log($"[HealItem] La colisión no fue con un objeto que puede recibir curación: {other.gameObject.name}");
        }
    }

    /// <summary>
    /// Aplica la curación al receptor y destruye el objeto de curación.
    /// </summary>
    /// <param name="receiver">El receptor de la curación.</param>
    private void ApplyHeal(IHealReceiver receiver)
    {
        _isConsumed = true;  // Marca el objeto como consumido
        receiver.Heal(healAmount);
        Debug.Log($"[HealItem] Se ha aplicado una curación de {healAmount} al objeto: {receiver}.");
        
        Destroy(gameObject);  // Destruye el objeto de curación
        Debug.Log($"[HealItem] El objeto {gameObject.name} ha sido destruido después de aplicar la curación.");
    }
}