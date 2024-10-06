using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class Rifle : WeaponBase
{
    public override void Shoot()
    {
        base.Shoot();
        Debug.Log("Disparando con el rifle.");
        // Aquí puedes agregar características de ráfaga o disparos más rápidos.
    }
}
