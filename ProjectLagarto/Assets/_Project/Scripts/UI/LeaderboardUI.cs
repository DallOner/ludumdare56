using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform contentTransform;

    public void UpdateLeaderboard(List<LeaderboardEntry> entries)
    {
        // Eliminar entradas anteriores
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        // Crear nuevas entradas
        foreach (var entryData in entries)
        {
            var entry = Instantiate(entryPrefab, contentTransform);
            var text = entry.GetComponentInChildren<Text>();
            text.text = $"{entryData.Name}: Muertes - {entryData.DeathCount}";
        }
    }
}

