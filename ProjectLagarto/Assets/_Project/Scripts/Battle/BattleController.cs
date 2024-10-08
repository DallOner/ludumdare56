using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.HighroadEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleController : MonoBehaviour
{
    [Header("Battle Settings")]
    public BattleType battleType;
    public bool isTeamMode; // Indica si es modo equipo o individual
    public int initialCountdown = 5;
    public float battleDuration = 60f;

    private IBattleMode battleMode;
    private List<PlayerController> players;
    private bool battleStarted = false;
    public LeaderboardUI leaderboardUI; // Asignar en el Inspector

    private IEnumerator Start()
    {
        // Espera hasta el final del frame para asegurar que todos los objetos se hayan inicializado
        yield return new WaitForEndOfFrame();

        // Obtener todos los jugadores en la escena
        players = FindObjectsOfType<PlayerController>().ToList();
        Debug.Log($"Número de jugadores encontrados: {players.Count}");

        // Crear la fábrica de modos de batalla
        IBattleModeFactory battleModeFactory = new BattleModeFactory(battleDuration, isTeamMode);

        // Crear el modo de batalla usando la fábrica
        battleMode = battleModeFactory.CreateBattleMode(battleType);

        // Suscribirse a eventos del modo de batalla
        battleMode.OnBattleEnded += OnBattleEnded;
        battleMode.OnLeaderboardUpdated += UpdateLeaderboard;

        

        // Iniciar la cuenta regresiva para la batalla
        StartCoroutine(StartBattleCountdown());
    }

    private void Update()
    {
        if (battleStarted && !battleMode.IsBattleOver)
        {
            battleMode.UpdateBattle(Time.deltaTime);
        }
    }

    private IEnumerator StartBattleCountdown()
    {
        // Inicializar el modo de batalla con los jugadores
        battleMode.Initialize(players);

        SetPlayersActive(false);

        for (int i = initialCountdown; i > 0; i--)
        {
            Debug.Log($"La batalla comienza en: {i}");
            // Aquí se podría actualizar una UI de cuenta regresiva
            yield return new WaitForSeconds(1);
        }

        SetPlayersActive(true);
        battleMode.StartBattle();
        battleStarted = true;
    }

    private void SetPlayersActive(bool isActive)
    {
        foreach (var player in players)
        {
            player.CanAct = isActive;
            // Desactivar/activar otros componentes si es necesario
        }
    }

    private void OnBattleEnded(BattleResult result)
    {
        battleStarted = false;

        if (result.IsTie)
        {
            Debug.Log("La batalla terminó en empate.");

            if (result.WinningPlayers != null)
            {
                foreach (var player in result.WinningPlayers)
                {
                    Debug.Log($"Jugador: {player.name}");
                }
            }
            else if (result.WinningTeams != null)
            {
                foreach (var teamID in result.WinningTeams)
                {
                    Debug.Log($"Equipo: {teamID}");
                }
            }
        }
        else
        {
            if (result.WinningPlayers != null)
            {
                foreach (var player in result.WinningPlayers)
                {
                    Debug.Log($"El jugador {player.name} ha ganado la batalla.");
                }
            }
            else if (result.WinningTeams != null)
            {
                foreach (var teamID in result.WinningTeams)
                {
                    Debug.Log($"El equipo {teamID} ha ganado la batalla.");
                }
            }
        }

        if (!string.IsNullOrEmpty(result.Message))
        {
            Debug.Log(result.Message);
        }

        // Aquí se puede implementar lógica adicional, como mostrar una pantalla de victoria
    }

    private void UpdateLeaderboard(List<LeaderboardEntry> entries)
    {
        if (leaderboardUI != null)
        {
            leaderboardUI.UpdateLeaderboard(entries);
        }
    }
}