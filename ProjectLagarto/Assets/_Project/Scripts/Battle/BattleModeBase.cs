using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleModeBase : IBattleMode
{
    protected List<PlayerController> players;
    protected bool battleOver;

    public bool IsBattleOver => battleOver;

    public virtual void Initialize(List<PlayerController> players)
    {
        this.players = players;
    }

    public virtual void StartBattle()
    {
        battleOver = false;
    }

    public abstract void UpdateBattle(float deltaTime);

    public virtual void EndBattle()
    {
        battleOver = true;
    }

    public event Action<BattleResult> OnBattleEnded;
    public event Action<List<LeaderboardEntry>> OnLeaderboardUpdated;

    protected void RaiseBattleEnded(BattleResult result)
    {
        battleOver = true;
        OnBattleEnded?.Invoke(result);
    }

    protected void RaiseLeaderboardUpdated(List<LeaderboardEntry> entries)
    {
        OnLeaderboardUpdated?.Invoke(entries);
    }
}