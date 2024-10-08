using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

public class WeaponPickup : MonoBehaviour
{
    
    [SerializeField] private List<WeaponBase> availableWeapons;  // Lista de armas disponibles para recoger
    [SerializeField] private Transform weaponHoldPoint;  // Punto donde se sujetará el arma en el jugador

    private void OnTriggerEnter(Collider other)
    {
        var weaponController = other.GetComponent<WeaponController>();
        if (weaponController != null && availableWeapons.Count > 0)
        {
            // Seleccionar un arma aleatoriamente de la lista
            WeaponBase randomWeapon = availableWeapons[Random.Range(0, availableWeapons.Count)];

            // Calcular la posición de aparición del arma: 6 unidades adelante en el eje Z
            Vector3 spawnPosition = transform.position + new Vector3(0, 0, 6);

            // Instanciar el arma en la nueva posición calculada
            WeaponBase weaponInstance = Instantiate(randomWeapon, spawnPosition, transform.rotation);

            // Hacer que el arma siga al jugador (convertir en hija del punto de sujeción)
            weaponInstance.transform.SetParent(weaponHoldPoint);

            // Equipar el arma al controlador de armas del jugador
            weaponController.EquipWeapon(weaponInstance);

            Debug.Log($"Arma recogida: {randomWeapon.GetType().Name} en la posición {spawnPosition}");
            Destroy(gameObject);  // Destruir el objeto de recogida en la escena
        }
    }
}
