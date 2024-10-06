using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;  // Referencia al slider de la barra de vida
    [SerializeField] private Transform _target;     // El transform del jugador u objeto que tendrá la barra de vida
    [SerializeField] private Vector3 _offset = new Vector3(0, 2, 0);  // Desplazamiento de la barra de vida respecto al jugador

    /// <summary>
    /// Inicializa la barra de vida con los valores de salud iniciales.
    /// </summary>
    /// <param name="maxHealth">El valor máximo de vida</param>
    public void InitializeHealthBar(int maxHealth, Transform target)
    {
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = maxHealth;
        _target = target;  // Asignar el objetivo (jugador o enemigo)
    }

    /// <summary>
    /// Actualiza la barra de vida con el valor actual de salud.
    /// </summary>
    /// <param name="currentHealth">El valor actual de vida</param>
    public void UpdateHealthBar(int currentHealth)
    {
        _healthSlider.value = currentHealth;
    }

    // Actualiza la posición de la barra de vida sobre el objetivo
    private void LateUpdate()
    {
        if (_target != null)
        {
            // Mantener la barra de vida en una posición fija encima del objetivo
            transform.position = _target.position + _offset;
            transform.LookAt(Camera.main.transform);  // Para que la barra de vida siempre mire hacia la cámara
        }
    }
}
