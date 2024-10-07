using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Modo de batalla por menor cantidad de muertes, jugable en modo individual o por equipos.
/// </summary>
public class LeastDeathsBattleMode : BattleModeBase
{
    private float battleDuration;
    private float elapsedTime;
    private bool isTeamMode;

    public LeastDeathsBattleMode(float duration, bool isTeamMode)
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
                healthController.OnPlayerDeath += OnPlayerDeath;
                healthController.ResetDeaths();
                healthController.SetInitialPosition(player.transform.position);
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

        // Aquí se podría actualizar el tiempo restante en la UI

        if (elapsedTime >= battleDuration)
        {
            DetermineWinner();
            EndBattle();
        }
    }

    private void OnPlayerDeath(PlayerController player)
    {
        var healthController = player.GetComponent<HealthController>();
        healthController?.IncrementDeathCount();

        // Respawnear al jugador después de un tiempo
        player.StartCoroutine(RespawnPlayer(player));
    }

    private IEnumerator RespawnPlayer(PlayerController player)
    {
        yield return new WaitForSeconds(3f); // Tiempo de respawn

        var healthController = player.GetComponent<HealthController>();
        var weaponController = player.GetComponent<WeaponController>();

        // Restablecer la salud y estado del jugador
        healthController.ResetHealth();

        // Restablecer las armas del jugador
        weaponController.ResetWeapons();

        // Mover al jugador a su posición inicial
        //player.transform.position = healthController.InitialPosition;
        player.Respawn();

        // Opcional: Restablecer animaciones y estados adicionales
    }

    private void DetermineWinner()
    {
        if (isTeamMode)
        {
            DetermineTeamWinner();
        }
        else
        {
            DetermineIndividualWinner();
        }

        // Mostrar la tabla de clasificación
        UpdateLeaderboard();
    }

    private void DetermineIndividualWinner()
    {
        var sortedPlayers = players.OrderBy(p => p.GetComponent<HealthController>().DeathCount).ToList();
        int minDeaths = sortedPlayers.First().GetComponent<HealthController>().DeathCount;

        // Filtrar jugadores con la menor cantidad de muertes
        var candidates = sortedPlayers.Where(p => p.GetComponent<HealthController>().DeathCount == minDeaths).ToList();

        if (candidates.Count == 1)
        {
            var result = new BattleResult
            {
                IsTie = false,
                WinningPlayers = candidates
            };
            RaiseBattleEnded(result);
        }
        else
        {
            // Empate en muertes, verificar puntos de vida
            int maxHealth = candidates.Max(p => p.GetComponent<HealthController>().CurrentHealth);
            var topCandidates = candidates.Where(p => p.GetComponent<HealthController>().CurrentHealth == maxHealth).ToList();

            if (topCandidates.Count == 1)
            {
                var result = new BattleResult
                {
                    IsTie = false,
                    WinningPlayers = topCandidates
                };
                RaiseBattleEnded(result);
            }
            else
            {
                // Empate total
                var result = new BattleResult
                {
                    IsTie = true,
                    WinningPlayers = topCandidates
                };
                RaiseBattleEnded(result);
            }
        }
    }

    private void DetermineTeamWinner()
    {
        var teamStats = players.GroupBy(p => p.TeamID)
                               .Select(group => new
                               {
                                   TeamID = group.Key,
                                   TotalDeaths = group.Sum(p => p.GetComponent<HealthController>().DeathCount),
                                   TotalHealth = group.Sum(p => p.GetComponent<HealthController>().CurrentHealth)
                               })
                               .OrderBy(t => t.TotalDeaths)
                               .ToList();

        int minDeaths = teamStats.First().TotalDeaths;

        // Filtrar equipos con la menor cantidad de muertes
        var candidateTeams = teamStats.Where(t => t.TotalDeaths == minDeaths).ToList();

        if (candidateTeams.Count == 1)
        {
            var result = new BattleResult
            {
                IsTie = false,
                WinningTeams = new List<int> { candidateTeams.First().TeamID }
            };
            RaiseBattleEnded(result);
        }
        else
        {
            // Empate en muertes, verificar puntos de vida
            int maxHealth = candidateTeams.Max(t => t.TotalHealth);
            var topTeams = candidateTeams.Where(t => t.TotalHealth == maxHealth).ToList();

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
                // Empate total
                var result = new BattleResult
                {
                    IsTie = true,
                    WinningTeams = topTeams.Select(t => t.TeamID).ToList()
                };
                RaiseBattleEnded(result);
            }
        }
    }

    private void UpdateLeaderboard()
    {
        List<LeaderboardEntry> entries;

        if (isTeamMode)
        {
            entries = players.GroupBy(p => p.TeamID)
                             .Select(group => new LeaderboardEntry
                             {
                                 Name = $"Equipo {group.Key}",
                                 DeathCount = group.Sum(p => p.GetComponent<HealthController>().DeathCount)
                             })
                             .OrderBy(e => e.DeathCount)
                             .ToList();
        }
        else
        {
            entries = players.Select(p => new LeaderboardEntry
            {
                Name = p.name,
                DeathCount = p.GetComponent<HealthController>().DeathCount
            })
            .OrderBy(e => e.DeathCount)
            .ToList();
        }

        // Notificar al controlador para actualizar la UI
        RaiseLeaderboardUpdated(entries);
    }
}