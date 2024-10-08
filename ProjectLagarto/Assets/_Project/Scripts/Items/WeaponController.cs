using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.IO;
using System;
using System.Text.RegularExpressions;

// <summary>
/// Controlador que maneja el equipamiento y uso de armas del jugador.
/// </summary>
public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private IWeapon _currentWeapon;

    private bool _canUseWeapon = true;  // Indica si el jugador puede usar su arma

    /// <summary>
    /// Método para equipar un arma.
    /// </summary>
    /// <param name="newWeapon">El arma nueva a equipar.</param>
    public void EquipWeapon(IWeapon newWeapon)
    {
        _currentWeapon = newWeapon;
        Debug.Log($"Arma equipada: {newWeapon.GetType().Name}");
    }

    /// <summary>
    /// Habilita el uso del arma.
    /// </summary>
    public void EnableWeapon()
    {
        _canUseWeapon = true;
    }

    /// <summary>
    /// Deshabilita el uso del arma.
    /// </summary>
    public void DisableWeapon()
    {
        _canUseWeapon = false;
    }

    /// <summary>
    /// Dispara el arma si es posible.
    /// </summary>
    public void Shoot()
    {
        if (!_canUseWeapon || _currentWeapon == null)
            return;

        _currentWeapon.Shoot();
    }

    /// <summary>
    /// Recarga el arma si es posible.
    /// </summary>
    public void Reload()
    {
        if (!_canUseWeapon || _currentWeapon == null)
            return;

        _currentWeapon.Reload();
    }

    public void ResetWeapons()
    {
        // Lógica para quitar todas las armas al jugador
        // Por ejemplo, limpiar la lista de armas y equipar un arma básica o ninguna
        //equippedWeapons.Clear();
        _currentWeapon = null;
    }

}