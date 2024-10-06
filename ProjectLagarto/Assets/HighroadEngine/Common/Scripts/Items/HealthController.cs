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
    [SerializeField] private int _initialHealth = 100;  // Vida inicial
    [SerializeField] private int _currentHealth;        // Vida actual

    [Header("HUD and Animation")]
    [SerializeField] private Animator _animator;        // Referencia al animador para manejar animaciones de muerte
    [SerializeField] private HealthBar _healthBar;      // Referencia al componente de la barra de vida

    private void Start()
    {
        ResetHealth(); // Restablecer la vida al inicio
        _healthBar.InitializeHealthBar(_initialHealth, this.transform); // Inicializar la barra de vida y pasarle el jugador
    }

    public void ResetHealth()
    {
        _currentHealth = _initialHealth;
        UpdateHUD();
        _animator.SetBool("IsDeath", false);
    }

    public void ReceiveDamage(int quantity)
    {
        _currentHealth -= quantity;
        UpdateHUD();

        if (_currentHealth <= 0)
        {
            PlayerDeath(); // Manejar la muerte del jugador
        }
    }

    public void Heal(int quantityToHeal)
    {
        _currentHealth = math.min(_currentHealth + quantityToHeal, _initialHealth);
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        _healthBar.UpdateHealthBar(_currentHealth); // Actualizar la barra de vida en la UI
    }

    private void PlayerDeath()
    {
        _animator.SetBool("IsDeath", true);
        // Lógica adicional para manejar la muerte
    }
}