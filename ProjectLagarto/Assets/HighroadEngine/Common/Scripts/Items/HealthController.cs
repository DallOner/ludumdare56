using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Events;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class HealthController : MonoBehaviour, IDamageReceiver, IHealReceiver
{
    [Header("Health Settings")]
    [SerializeField] private int _initialHealth = 100;  // Vida inicial máxima
    private int _currentHealth;                         // Vida actual

    [Header("HUD and Animation")]
    [SerializeField] private Animator _animator;        // Referencia al Animator para manejar animaciones
    [SerializeField] private HealthBar _healthBar;      // Referencia a la barra de vida UI

    private bool _isDead = false;  // Estado de muerte para evitar la repetición de acciones post-muerte

    /// <summary>
    /// Inicializa la vida y la barra de vida cuando el objeto comienza.
    /// </summary>
    private void Start()
    {
        ResetHealth();
        _healthBar.InitializeHealthBar(_initialHealth, this.transform);  // Inicializa la barra de vida
    }

    /// <summary>
    /// Restablece la vida al valor inicial y actualiza el HUD.
    /// </summary>
    public void ResetHealth()
    {
        _currentHealth = _initialHealth;
        _isDead = false;
        UpdateHUD();
        _animator.SetBool("IsDeath", false);  // Asegura que la animación de muerte no esté activa
    }

    /// <summary>
    /// Aplica daño al objeto, actualiza el HUD y verifica si el jugador ha muerto.
    /// </summary>
    /// <param name="damageAmount">La cantidad de daño recibida.</param>
    public void ReceiveDamage(int damageAmount)
    {
        if (_isDead) return;  // Evitar procesamiento si ya está muerto

        _currentHealth -= Mathf.Clamp(damageAmount, 0, _currentHealth);  // Restar la cantidad de daño recibida
        UpdateHUD();

        if (_currentHealth <= 0)
        {
            PlayerDeath();
        }
    }

    /// <summary>
    /// Aplica curación al jugador, sin exceder la vida máxima.
    /// </summary>
    /// <param name="healAmount">La cantidad de curación recibida.</param>
    public void Heal(int healAmount)
    {
        if (_isDead) return;  // No curar si ya está muerto

        _currentHealth = Mathf.Clamp(_currentHealth + healAmount, 0, _initialHealth);  // Curar sin exceder la vida máxima
        UpdateHUD();
    }

    /// <summary>
    /// Actualiza el HUD con el estado actual de la vida.
    /// </summary>
    private void UpdateHUD()
    {
        if (_healthBar != null)
        {
            _healthBar.UpdateHealthBar(_currentHealth);  // Actualiza la barra de vida
        }
    }

    /// <summary>
    /// Maneja la muerte del jugador, activando animaciones y desactivando lógica adicional.
    /// </summary>
    private void PlayerDeath()
    {
        _isDead = true;
        _animator.SetBool("IsDeath", true);  // Activar la animación de muerte

        // Lógica adicional que se puede agregar cuando el jugador muere, como desactivar controles, etc.
        Debug.Log($"{gameObject.name} ha muerto.");
    }
}