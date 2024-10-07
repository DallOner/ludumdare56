using System;
using System.Collections.Generic;

public interface IBattleMode
{
    void Initialize(List<PlayerController> players);
    void StartBattle();
    void UpdateBattle(float deltaTime);
    bool IsBattleOver { get; }
    void EndBattle();

    event Action<BattleResult> OnBattleEnded;
    event Action<List<LeaderboardEntry>> OnLeaderboardUpdated;
}
