using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class Pistol : WeaponBase
{
    public override void Shoot()
    {
        base.Shoot();
        Debug.Log("Disparando con la pistola.");
        // Aquí podrías agregar efectos visuales y de sonido específicos para la pistola.
    }
}
