using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private IWeapon _currentWeapon;

    // Método para equipar un arma
    public void EquipWeapon(IWeapon newWeapon)
    {
        _currentWeapon = newWeapon;
        Debug.Log($"Arma equipada: {newWeapon.GetType().Name}");
    }

    private void Update()
    {
         // Si se presiona la tecla "Espacio", el arma dispara
    if (Input.GetKeyDown(KeyCode.Space) && _currentWeapon != null)
    {
        _currentWeapon.Shoot();
    }

    // Si se presiona la tecla "R", el arma se recarga
    if (Input.GetKeyDown(KeyCode.R) && _currentWeapon != null)
    {
        _currentWeapon.Reload();
    }
    }
}
