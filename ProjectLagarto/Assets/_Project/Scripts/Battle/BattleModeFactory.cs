using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleModeFactory : IBattleModeFactory
{
    private readonly float battleDuration;
    private readonly bool isTeamMode;

    public BattleModeFactory(float battleDuration, bool isTeamMode)
    {
        this.battleDuration = battleDuration;
        this.isTeamMode = isTeamMode;
    }

    public IBattleMode CreateBattleMode(BattleType type)
    {
        switch (type)
        {
            case BattleType.FreeForAll:
                return new FreeForAllBattleMode(battleDuration, isTeamMode);
            case BattleType.LeastDeaths:
                return new LeastDeathsBattleMode(battleDuration, isTeamMode);
            // Agregar más casos para nuevos modos
            default:
                throw new ArgumentException("Tipo de batalla desconocido");
        }
    }
}
