using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.HighroadEngine;
using UnityEngine.Events;

/// <summary>
/// Controlador principal que maneja la lógica de cada jugador, incluyendo acciones, equipo, salud y armas.
/// </summary>
[RequireComponent(typeof(HealthController))]
[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(SolidController))]
[RequireComponent(typeof(SolidSoundBehaviour))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [Tooltip("Indica si el jugador puede realizar acciones actualmente.")]
    [SerializeField]
    private bool canAct = false;

    [Tooltip("ID del equipo al que pertenece el jugador (para batallas en equipo).")]
    [SerializeField]
    private int teamID = 0;

    private HealthController _healthController;
    private WeaponController _weaponController;
    private SolidController _solidController;
    private SolidSoundBehaviour _soundBehaviour;

    /// <summary>
    /// Evento que se dispara cuando el jugador muere.
    /// </summary>
    public UnityEvent OnPlayerDeath = new UnityEvent();

    /// <summary>
    /// Propiedad para acceder o modificar el estado de si el jugador puede actuar.
    /// </summary>
    public bool CanAct
    {
        get => canAct;
        set
        {
            canAct = value;
            if (canAct)
            {
                EnableActions();
            }
            else
            {
                DisableActions();
            }
        }
    }

    /// <summary>
    /// Propiedad para obtener o establecer el ID del equipo del jugador.
    /// </summary>
    public int TeamID
    {
        get => teamID;
        set => teamID = value;
    }

    /// <summary>
    /// Propiedad que indica si el jugador está vivo.
    /// </summary>
    public bool IsAlive => _healthController?.IsAlive ?? false;

    /// <summary>
    /// Propiedad para obtener la salud actual del jugador.
    /// </summary>
    public int Health => _healthController?.CurrentHealth ?? 0;

    /// <summary>
    /// Método inicial para configurar componentes y suscribirse a eventos.
    /// </summary>
    private void Awake()
    {
        InitializeComponents();
    }

    /// <summary>
    /// Inicializa y valida los componentes requeridos del jugador.
    /// </summary>
    private void InitializeComponents()
    {
        // Obtiene referencias a los componentes necesarios.
        _healthController = GetComponent<HealthController>();
        _weaponController = GetComponent<WeaponController>();
        _solidController = GetComponent<SolidController>();
        _soundBehaviour = GetComponent<SolidSoundBehaviour>();

        // Verifica que los componentes existan.
        if (_healthController == null)
            Debug.LogError($"{gameObject.name} no tiene un HealthController asignado.");

        if (_weaponController == null)
            Debug.LogError($"{gameObject.name} no tiene un WeaponController asignado.");

        if (_solidController == null)
            Debug.LogError($"{gameObject.name} no tiene un SolidController asignado.");

        if (_soundBehaviour == null)
            Debug.LogError($"{gameObject.name} no tiene un SolidSoundBehaviour asignado.");

        // Se suscribe al evento de muerte del jugador.
        if (_healthController != null)
            _healthController.OnPlayerDeath += (player) => HandlePlayerDeath();
    }

    /// <summary>
    /// Actualiza el estado del jugador cada frame.
    /// </summary>
    private void Update()
    {
        if (!canAct || !IsAlive)
        {
            // Si el jugador no puede actuar o está muerto, deshabilita las acciones.
            DisableActions();
            return;
        }

        // Procesa las acciones del jugador.
        ProcessPlayerActions();
    }

    /// <summary>
    /// Procesa las acciones del jugador, como movimiento y disparo.
    /// </summary>
    private void ProcessPlayerActions()
    {
        // Procesa la entrada de movimiento.
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (_solidController != null)
        {
            _solidController.CurrentSteeringAmount = horizontalInput;
            _solidController.CurrentGasPedalAmount = verticalInput;
        }

        // Procesa la entrada de disparo.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _weaponController?.Shoot();
        }

        // Procesa la entrada de recarga.
        if (Input.GetKeyDown(KeyCode.R))
        {
            _weaponController?.Reload();
        }
    }

    /// <summary>
    /// Habilita las acciones del jugador y sus componentes asociados.
    /// </summary>
    private void EnableActions()
    {
        _weaponController?.EnableWeapon();
        _solidController?.EnableControl();
        _soundBehaviour?.EnableSounds();
    }

    /// <summary>
    /// Deshabilita las acciones del jugador y sus componentes asociados.
    /// </summary>
    private void DisableActions()
    {
        _weaponController?.DisableWeapon();
        _solidController?.DisableControl();
        _soundBehaviour?.DisableSounds();
    }

    /// <summary>
    /// Maneja la lógica cuando el jugador muere.
    /// </summary>
    private void HandlePlayerDeath()
    {
        // Deshabilita las acciones del jugador.
        CanAct = false;

        // Notifica que el jugador ha muerto.
        Debug.Log($"{gameObject.name} ha muerto.");

        // Dispara el evento de muerte para que otros componentes puedan reaccionar.
        OnPlayerDeath.Invoke();
    }

    /// <summary>
    /// Resetea el estado del jugador, restaurando su salud y permitiéndole actuar.
    /// </summary>
    public void Respawn()
    {
        // Restablece la salud del jugador.
        //_healthController?.ResetHealth();

        // Habilita las acciones del jugador.
        CanAct = true;

        // Puedes agregar lógica adicional aquí, como reposicionar al jugador.
        _solidController.Respawn();
    }
}