using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.HighroadEngine
{
	/// <summary>
/// Extensión de comportamiento que maneja los sonidos del vehículo.
/// </summary>
public class SolidSoundBehaviour : SolidBehaviourExtender
{
    [Header("Sounds")]
    public AudioClip EngineSound;
    public AudioClip CrashSound;

    [Range(0.1f, 10f)]
    public float EngineVolume = 0.5f;

    [Range(0f, 5f)]
    public float MinimalCrashSpeed = 2.0f;

    [Range(1f, 10f)]
    public float EngineMaxPitch = 5.5f;

    [Range(0f, 1f)]
    public float SpeedFactor = 0.1f;

    protected SoundManager _soundManager;
    protected AudioSource _engineSound;
    protected float _engineSoundPitch;

    private bool _soundsEnabled = true;  // Controla si los sonidos están habilitados

    /// <summary>
    /// Inicializa los sonidos del vehículo.
    /// </summary>
    public override void Initialization()
    {
        if (EngineSound != null)
        {
            _soundManager = FindObjectOfType<SoundManager>();
            if (_soundManager != null)
            {
                _engineSound = _soundManager.PlayLoop(EngineSound, transform.position);

                if (_engineSound != null)
                {
                    _engineSoundPitch = _engineSound.pitch;
                    _engineSound.volume = EngineVolume;
                }
            }
            else
            {
                Debug.LogWarning("Missing SoundManager Object in scene. Please add one.");
            }
        }
    }

    /// <summary>
    /// Actualiza el sonido del motor según la velocidad.
    /// </summary>
    public override void Update()
    {
        if (_engineSound == null || !_soundsEnabled)
        {
            return;
        }
        _engineSound.pitch = Mathf.Min(EngineMaxPitch, Mathf.Max(_engineSoundPitch, _engineSoundPitch * _controller.Speed * SpeedFactor));
    }

    /// <summary>
    /// Maneja los sonidos al colisionar.
    /// </summary>
    /// <param name="other">El objeto con el que colisionó.</param>
    public override void OnVehicleCollisionEnter(Collision other)
    {
        if (!_soundsEnabled)
            return;

        if (CrashSound != null)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                if (_soundManager != null)
                {
                    if (other.relativeVelocity.magnitude >= MinimalCrashSpeed)
                    {
                        _soundManager.PlaySound(CrashSound, transform.position, true);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Habilita los sonidos del vehículo.
    /// </summary>
    public void EnableSounds()
    {
        _soundsEnabled = true;
        if (_engineSound != null)
        {
            _engineSound.Play();
        }
    }

    /// <summary>
    /// Deshabilita los sonidos del vehículo.
    /// </summary>
    public void DisableSounds()
    {
        _soundsEnabled = false;
        if (_engineSound != null)
        {
            _engineSound.Pause();
        }
    }
}
}
