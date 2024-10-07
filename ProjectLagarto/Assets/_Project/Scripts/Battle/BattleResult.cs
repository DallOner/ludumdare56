using System.Collections.Generic;

public class BattleResult
{
    public bool IsTie { get; set; }
    public List<PlayerController> WinningPlayers { get; set; }
    public List<int> WinningTeams { get; set; }
    public string Message { get; set; } // Mensaje opcional
}