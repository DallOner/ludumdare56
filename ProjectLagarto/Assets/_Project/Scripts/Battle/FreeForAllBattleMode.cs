using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Modo de batalla Todos Contra Todos, jugable en modo individual o por equipos.
/// </summary>
public class FreeForAllBattleMode : BattleModeBase
{
    private float battleDuration;
    private float elapsedTime;
    private bool isTeamMode;

    public FreeForAllBattleMode(float duration, bool isTeamMode)
    {
        battleDuration = duration;
        this.isTeamMode = isTeamMode;
    }

    public override void Initialize(List<PlayerController> players)
    {
        base.Initialize(players);

        // Suscribirse a los eventos de muerte de cada jugador
        foreach (var player in players)
        {
            var healthController = player.GetComponent<HealthController>();
            if (healthController != null)
            {
                var capturedPlayer = player; // Captura para evitar problemas con closures
                healthController.OnPlayerDeath += (player) => OnPlayerDeath(player);
            }
        }
    }

    public override void StartBattle()
    {
        base.StartBattle();
        elapsedTime = 0f;
    }

    public override void UpdateBattle(float deltaTime)
    {
        if (IsBattleOver)
            return;

        elapsedTime += deltaTime;

        // Verificar si solo queda un jugador o equipo vivo
        if (isTeamMode)
        {
            CheckTeamVictory();
        }
        else
        {
            CheckIndividualVictory();
        }

        // Si el tiempo de batalla ha terminado
        if (elapsedTime >= battleDuration)
        {
            if (isTeamMode)
            {
                DetermineTeamWinnerByHealth();
            }
            else
            {
                DetermineIndividualWinnerByHealth();
            }
            EndBattle();
        }
    }

    private void OnPlayerDeath(PlayerController player)
    {
        // En el momento de la muerte, podemos verificar si hay un ganador
        if (isTeamMode)
        {
            CheckTeamVictory();
        }
        else
        {
            CheckIndividualVictory();
        }
    }

    private void CheckTeamVictory()
    {
        var aliveTeams = players.Where(p => p.GetComponent<HealthController>().IsAlive)
                                .Select(p => p.TeamID)
                                .Distinct()
                                .ToList();

        if (aliveTeams.Count == 1)
        {
            var result = new BattleResult
            {
                IsTie = false,
                WinningTeams = aliveTeams
            };
            RaiseBattleEnded(result);
        }
        else if (aliveTeams.Count == 0)
        {
            var result = new BattleResult
            {
                IsTie = true,
                Message = "Todos los equipos han sido eliminados. No hay ganador."
            };
            RaiseBattleEnded(result);
        }
    }

    private void CheckIndividualVictory()
    {
        var alivePlayers = players.Where(p => p.GetComponent<HealthController>().IsAlive).ToList();
        if (alivePlayers.Count == 1)
        {
            var result = new BattleResult
            {
                IsTie = false,
                WinningPlayers = alivePlayers
            };
            RaiseBattleEnded(result);
        }
        else if (alivePlayers.Count == 0)
        {
            var result = new BattleResult
            {
                IsTie = true,
                Message = "Todos los jugadores han sido eliminados. No hay ganador."
            };
            RaiseBattleEnded(result);
        }
    }

    private void DetermineIndividualWinnerByHealth()
    {
        var alivePlayers = players.Where(p => p.GetComponent<HealthController>().IsAlive).ToList();

        if (alivePlayers.Count == 0)
        {
            var result = new BattleResult
            {
                IsTie = true,
                Message = "No quedan jugadores vivos. No hay ganador."
            };
            RaiseBattleEnded(result);
            return;
        }

        // Obtener el valor máximo de salud entre los jugadores vivos
        int maxHealth = alivePlayers.Max(p => p.GetComponent<HealthController>().CurrentHealth);

        // Encontrar todos los jugadores con la salud máxima
        var topPlayers = alivePlayers.Where(p => p.GetComponent<HealthController>().CurrentHealth == maxHealth).ToList();

        if (topPlayers.Count == 1)
        {
            var result = new BattleResult
            {
                IsTie = false,
                WinningPlayers = topPlayers
            };
            RaiseBattleEnded(result);
        }
        else
        {
            // Empate entre jugadores con la misma salud máxima
            var result = new BattleResult
            {
                IsTie = true,
                WinningPlayers = topPlayers
            };
            RaiseBattleEnded(result);
        }
    }

    private void DetermineTeamWinnerByHealth()
    {
        var aliveTeams = players.Where(p => p.GetComponent<HealthController>().IsAlive)
                                .GroupBy(p => p.TeamID)
                                .Select(group => new
                                {
                                    TeamID = group.Key,
                                    TotalHealth = group.Sum(p => p.GetComponent<HealthController>().CurrentHealth)
                                })
                                .ToList();

        if (aliveTeams.Count == 0)
        {
            var result = new BattleResult
            {
                IsTie = true,
                Message = "No quedan equipos vivos. No hay ganador."
            };
            RaiseBattleEnded(result);
            return;
        }

        // Obtener el valor máximo de salud total entre los equipos vivos
        int maxTeamHealth = aliveTeams.Max(t => t.TotalHealth);

        // Encontrar todos los equipos con la salud total máxima
        var topTeams = aliveTeams.Where(t => t.TotalHealth == maxTeamHealth).ToList();

        if (topTeams.Count == 1)
        {
            var result = new BattleResult
            {
                IsTie = false,
                WinningTeams = new List<int> { topTeams.First().TeamID }
            };
            RaiseBattleEnded(result);
        }
        else
        {
            // Empate entre equipos con la misma salud total máxima
            var result = new BattleResult
            {
                IsTie = true,
                WinningTeams = topTeams.Select(t => t.TeamID).ToList()
            };
            RaiseBattleEnded(result);
        }
    }
}